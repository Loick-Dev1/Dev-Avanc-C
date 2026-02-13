# Guide du Développeur

Ce guide fournit les instructions nécessaires pour configurer l'environnement de développement, exécuter le projet et lancer les tests.

## Prérequis

Avant de commencer, assurez-vous d'avoir installé les outils suivants sur votre machine :

*   **[.NET SDK 10.0](https://dotnet.microsoft.com/download/dotnet/10.0)** : Nécessaire pour compiler et exécuter le projet.
*   **[Git](https://git-scm.com/downloads)** : Pour cloner le dépôt.
*   **[Python](https://www.python.org/downloads/)** (Optionnel) : Requis uniquement si vous souhaitez générer et visualiser la documentation locale avec MkDocs.
*   **Un IDE** : Visual Studio 2022, JetBrains Rider, ou VS Code (avec l'extension C# Dev Kit).

## Installation

1.  **Cloner le dépôt** :
    ```bash
    git clone https://github.com/votre-utilisateur/AdvancedDevSample.git
    cd AdvancedDevSample
    ```

2.  **Restaurer les dépendances** :
    ```bash
    dotnet restore AdvancedDevSample.Domain.slnx
    ```

## Exécution du Projet

L'API se trouve dans le projet `AdvancedDevSample.Api`.

1.  **Lancer l'API** :
    ```bash
    cd AdvancedDevSample.Api
    dotnet run
    ```

2.  **Accéder à Swagger** :
    Une fois l'application lancée, ouvrez votre navigateur à l'adresse indiquée dans la console (par défaut `https://localhost:7192/swagger` ou `http://localhost:5271/swagger`).

## Tests

Le projet contient une suite de tests unitaires et d'intégration.

1.  **Lancer tous les tests** :
    ```bash
    dotnet test
    ```

2.  **Vérifier la couverture de code** (nécessite un outil de couverture configuré) :
    Les tests incluent des scénarios pour les Produits, Commandes, Clients, Fournisseurs et l'Authentification.

## Base de Données

Actuellement, le projet utilise une base de données **In-Memory** (en mémoire) pour simplifier le développement et les tests.
*   Les données sont réinitialisées à chaque redémarrage de l'application.
*   Des données de test (Seed) sont automatiquement insérées au démarrage (ex: un utilisateur admin, quelques produits).

## Documentation (MkDocs)

Pour visualiser cette documentation localement :

1.  **Installer MkDocs et le thème Material** :
    ```bash
    pip install mkdocs-material
    ```

2.  **Lancer le serveur de documentation** :
    ```bash
    mkdocs serve
    ```
    La documentation sera accessible sur `http://127.0.0.1:8000`.

## Authentification

L'API est sécurisée par JWT. Pour tester les endpoints protégés (POST/PUT/DELETE) :
1.  Utilisez l'endpoint `POST /api/Auth/login`.
2.  Récupérez le `token` dans la réponse.
3.  Dans Swagger, cliquez sur **Authorize** et entrez `Bearer <votre_token>`.
