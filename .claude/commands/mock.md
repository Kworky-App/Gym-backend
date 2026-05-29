# /mock — Générateur de mocks in-memory

Tu es un assistant expert en architecture hexagonale C# / .NET 10.
Ton rôle est de générer des **implémentations in-memory manuelles** (sans librairie externe)
pour tester la couche `Domain` de GymApp en isolation.

## Contexte du projet

```
GymApp.Domain/           — Entités, interfaces (Ports). Aucune dépendance externe.
GymApp.Infrastructure/   — Adapters réels (persistance, services externes)
GymApp.Tests/            — Tests ; c'est ici que vivent les mocks in-memory
```

Règle absolue : `Domain` ne doit jamais dépendre d'`Infrastructure` ni de librairies
de mock externes (Moq, NSubstitute). Les mocks in-memory **implémentent les interfaces
du Domain** et sont placés dans `GymApp.Tests/Mocks/`.

## Comportement attendu

L'utilisateur décrit en langage humain le mock dont il a besoin.
Voici ce que tu dois faire, dans l'ordre :

### Étape 1 — Comprendre la demande
Pose les questions minimales si la description est ambiguë :
- Quel entity/aggregate est concerné ?
- Quelles opérations doit supporter le mock (lecture, écriture, recherche…) ?
- Y a-t-il un comportement particulier à simuler (ex: retourner null si non trouvé, lever une exception) ?

### Étape 2 — Vérifier si l'interface existe déjà
Cherche dans `GymApp.Domain/` une interface correspondante (ex: `IUserRepository`,
`IWorkoutPlanRepository`). Lis les fichiers existants avant de générer.

### Étape 3a — Si l'interface n'existe pas encore
Génère **les deux** :

**`GymApp.Domain/Ports/I<Nom>Repository.cs`** (ou `I<Nom>Service.cs`) :
```csharp
namespace GymApp.Domain.Ports;

public interface I<Nom>Repository
{
    Task<Entité?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Entité>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Entité entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Entité entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
```
Adapte les méthodes à ce que l'utilisateur décrit. N'inclus que ce qui est demandé.

**`GymApp.Tests/Mocks/InMemory<Nom>Repository.cs`** :

### Étape 3b — Si l'interface existe déjà
Génère uniquement le mock `GymApp.Tests/Mocks/InMemory<Nom>Repository.cs`.

### Étape 4 — Pattern du mock in-memory

Respecte ce patron strictement :

```csharp
namespace GymApp.Tests.Mocks;

public sealed class InMemory<Nom>Repository : I<Nom>Repository
{
    private readonly Dictionary<Guid, <Entité>> _store = new();

    // Accès direct pour les assertions dans les tests
    public IReadOnlyDictionary<Guid, <Entité>> Store => _store;

    public Task<<Entité>?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _store.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public Task<IReadOnlyList<<Entité>>> GetAllAsync(CancellationToken cancellationToken = default)
        => Task.FromResult<IReadOnlyList<<Entité>>>(_store.Values.ToList());

    public Task AddAsync(<Entité> entity, CancellationToken cancellationToken = default)
    {
        _store[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(<Entité> entity, CancellationToken cancellationToken = default)
    {
        _store[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _store.Remove(id);
        return Task.CompletedTask;
    }
}
```

**Règles de style :**
- `sealed` par défaut (les mocks ne sont pas destinés à être hérités)
- Exposer `Store` en lecture seule pour permettre les assertions directes dans les tests
- `Task.FromResult` / `Task.CompletedTask` — pas d'`async`/`await` superflu
- Nullable activé : utiliser `?` correctement
- Namespaces : `GymApp.Domain.Ports`, `GymApp.Tests.Mocks`
- Pas de commentaires sauf si la logique simulée n'est pas évidente

### Étape 5 — Exemple d'utilisation dans un test

Fournis toujours un court exemple (stub de test xUnit) montrant comment utiliser le mock :

```csharp
public class <Nom>ServiceTests
{
    [Fact]
    public async Task <Scénario>_<RésultatAttendu>()
    {
        // Arrange
        var repository = new InMemory<Nom>Repository();
        // Pré-remplir si nécessaire
        await repository.AddAsync(new <Entité> { Id = Guid.NewGuid(), /* … */ });

        var sut = new <Service>(repository);

        // Act
        var result = await sut.<Méthode>();

        // Assert
        Assert.NotNull(result);
    }
}
```

### Étape 6 — Vérification

Après avoir généré les fichiers, exécute :
```bash
dotnet build
```
et corrige toute erreur de compilation avant de considérer la tâche terminée.

## Ce que tu NE dois PAS faire

- Ajouter des dépendances NuGet (Moq, NSubstitute, FakeItEasy, etc.)
- Placer du code de test dans `GymApp.Domain` ou `GymApp.Infrastructure`
- Créer des classes abstraites ou des hiérarchies complexes pour les mocks
- Ajouter de la logique métier dans le mock (le mock simule, il ne calcule pas)
- Omettre le projet `GymApp.Tests` de la solution si xUnit n'est pas encore référencé
  (dans ce cas, ajouter xUnit avant d'écrire les tests)

## Rappel : ajouter xUnit si nécessaire

Si `GymApp.Tests` n'a pas encore de framework de test :
```bash
dotnet add GymApp.Tests/GymApp.Tests.csproj package xunit
dotnet add GymApp.Tests/GymApp.Tests.csproj package xunit.runner.visualstudio
dotnet add GymApp.Tests/GymApp.Tests.csproj package Microsoft.NET.Test.Sdk
```

---
$ARGUMENTS
