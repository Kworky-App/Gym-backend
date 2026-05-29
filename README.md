# GymApp Backend API

Backend API pour l'application mobile GymApp permettant la gestion des plans d'entraînement, des exercices et des utilisateurs.

## 📋 Description

GymApp Backend est une API REST développée en **C# avec ASP.NET Core** qui suit une architecture **hexagonale (Ports & Adapters)** afin de garantir une séparation claire entre :
- La logique métier (Domain)
- L'infrastructure (Infrastructure)
- L'exposition API (API)

Cette architecture permet une meilleure maintenabilité, testabilité et évolutivité du projet.

## 🛠️ Stack Technologique

- **Framework** : ASP.NET Core (Net 10.0)
- **Langage** : C#
- **Architecture** : Hexagonale (Ports & Adapters)
- **Documentation API** : OpenAPI / Swagger

## 📁 Structure du Projet

```
Gym-backend/
├── GymApp.Api/              # Couche présentation - Endpoints API
├── GymApp.Domain/           # Couche métier - Logique applicative
├── GymApp.Infrastructure/   # Couche infrastructure - Persistance & Services externes
├── GymApp.Tests/            # Tests unitaires et d'intégration
└── GymApp.sln              # Fichier solution
```

### Couches du projet

**GymApp.Api**
- Expose les endpoints REST
- Gère les requêtes/réponses HTTP
- Swagger/OpenAPI intégré

**GymApp.Domain**
- Contient la logique métier
- Entités métier
- Contrats/Interfaces (Ports)

**GymApp.Infrastructure**
- Implémentation des adaptateurs (Adapters)
- Accès à la base de données
- Intégration avec services externes

**GymApp.Tests**
- Tests unitaires
- Tests d'intégration

## 🚀 Démarrage

### Prérequis
- .NET 10.0 SDK
- Un IDE (Visual Studio, VS Code, JetBrains Rider)

### Installation et exécution

```bash
# Cloner le repository
git clone <repository-url>
cd Gym-backend

# Restaurer les dépendances
dotnet restore

# Compiler le projet
dotnet build

# Lancer l'API
dotnet run --project GymApp.Api/GymApp.Api.csproj
```

L'API sera disponible sur `https://localhost:5001` ou `http://localhost:5000`

### Accéder à Swagger
Une fois l'API lancée, vous pouvez accéder à la documentation Swagger via :
```
https://localhost:5001/openapi/v1.json
```

## 📝 Fonctionnalités

- Gestion des utilisateurs
- Création et gestion des plans d'entraînement
- Gestion des exercices
- Suivi des sessions d'entraînement

## 🧪 Tests

```bash
# Exécuter tous les tests
dotnet test

# Exécuter avec rapport de couverture
dotnet test /p:CollectCoverage=true
```

## 📚 Documentation

### Endpoints

Les endpoints disponibles sont documentés dans Swagger (voir section "Accéder à Swagger").

### Architecture Hexagonale

L'architecture hexagonale permet de :
- Isoler la logique métier des détails techniques
- Faciliter le testing
- Rendre le code plus maintenable et évolutif
- Respecter le SOLID principles

## 🤖 Outils IA

Ce projet est configuré pour Claude Code et Cursor.

### Claude Code

```bash
claude   # lancer depuis la racine du dépôt
```

| Commande | Ce qu'elle fait |
|---|---|
| `/mock <NomRepository>` | Génère l'interface Domain + le mock in-memory dans `GymApp.Tests/Mocks/` |
| `/code-review` | Revue du diff courant — bugs et simplifications |
| `/run` | Lance l'API et vérifie le démarrage |
| `/verify` | Vérifie qu'un changement fonctionne en vrai |
| `/security-review` | Audit de sécurité des changements en cours |

### Cursor

Les règles sont dans `.cursor/rules/` et s'activent automatiquement selon les fichiers ouverts,
ou en les citant avec `@` dans le chat (`Cmd+L`).

| Règle | Ce qu'elle fait |
|---|---|
| `@mock` | Génère un mock in-memory pour le Domain (même logique que `/mock` dans Claude Code) |

---

## 🤝 Contribution

Les contributions sont bienvenues ! N'hésitez pas à soumettre des pull requests.

## 📄 Licence

À définir

## 📞 Contact

Pour toute question, merci de contacter l'équipe de développement.
