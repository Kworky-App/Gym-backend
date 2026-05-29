# AGENTS.md — GymApp Backend

Guide destiné aux agents IA (Claude Code, Cursor, etc.) travaillant sur ce dépôt.

## Vue d'ensemble

API REST en **C# / ASP.NET Core (.NET 10)** pour l'application mobile GymApp
(gestion des utilisateurs, plans d'entraînement, exercices, sessions).
Le projet suit une **architecture hexagonale (Ports & Adapters)**.

> État actuel : le projet est au stade d'amorçage (scaffold). Les couches
> contiennent encore des `Class1.cs` de remplacement et l'endpoint `WeatherForecast`
> généré par défaut. Ces éléments sont à remplacer par le vrai code métier.

## Structure

```
GymApp.Api/              # Couche présentation — endpoints REST, OpenAPI/Swagger, Program.cs
GymApp.Domain/           # Cœur métier — entités, logique, interfaces (Ports). AUCUNE dépendance externe.
GymApp.Infrastructure/   # Adapters — persistance, services externes (implémente les Ports du Domain)
GymApp.Tests/            # Tests unitaires et d'intégration
GymApp.sln               # Solution
```

### Règles de dépendances (à respecter impérativement)

- `Domain` ne dépend de **rien** (ni Api, ni Infrastructure, ni framework web).
- `Infrastructure` dépend de `Domain` (il implémente ses interfaces).
- `Api` dépend de `Domain` et `Infrastructure` (composition / injection de dépendances).
- Les flux vont toujours vers l'intérieur : `Api → Infrastructure → Domain`.

## Commandes

```bash
dotnet restore                                   # Restaurer les dépendances
dotnet build                                     # Compiler la solution
dotnet run --project GymApp.Api/GymApp.Api.csproj  # Lancer l'API
dotnet test                                      # Exécuter les tests
dotnet test /p:CollectCoverage=true              # Tests avec couverture
```

L'API écoute sur `https://localhost:5001` / `http://localhost:5000`.
Spec OpenAPI (en Development) : `https://localhost:5001/openapi/v1.json`.

## Conventions de code

- `Nullable` et `ImplicitUsings` sont **activés** sur tous les projets — ne pas
  réintroduire d'usings redondants, et traiter correctement la nullabilité.
- Namespaces alignés sur le nom du projet : `GymApp.Domain`, `GymApp.Api`, etc.
- Style C# standard : PascalCase pour types/méthodes/propriétés, camelCase pour
  variables locales/paramètres, interfaces préfixées `I`.
- Garder la logique métier dans `Domain` ; pas d'appel direct à la base de données
  ou à des dépendances HTTP depuis `Domain`.

## Bonnes pratiques pour l'agent

- Après modification, exécuter `dotnet build` puis `dotnet test`.
- Ne pas committer `bin/` ni `obj/` (déjà couverts par `.gitignore`).
- Respecter l'architecture hexagonale lors de l'ajout de fonctionnalités :
  définir le contrat (interface) dans `Domain`, l'implémenter dans `Infrastructure`,
  l'exposer dans `Api`.
- Le projet `GymApp.Tests` n'a pas encore de framework de test référencé
  (xUnit/NUnit/MSTest) — l'ajouter avant d'écrire des tests.
