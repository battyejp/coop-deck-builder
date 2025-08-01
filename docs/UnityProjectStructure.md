# Unity Project Structure

This document outlines the recommended folder structure for implementing the cooperative deck-builder game in Unity.

## Overview

A well-organized project structure is essential for maintaining clean, scalable code and efficient collaboration. This structure follows Unity best practices while accommodating the specific needs of a deck-building game.

## Recommended Folder Structure

```
ProjectRoot/
├── Assets/
│   ├── Scripts/
│   │   ├── Cards/
│   │   │   ├── Card.cs
│   │   │   ├── CardData.cs (ScriptableObject)
│   │   │   ├── CardEffect.cs
│   │   │   └── CardTypes/
│   │   │       ├── AttackCard.cs
│   │   │       ├── DefenseCard.cs
│   │   │       └── UtilityCard.cs
│   │   ├── Decks/
│   │   │   ├── Deck.cs
│   │   │   ├── DeckBuilder.cs
│   │   │   └── DeckManager.cs
│   │   ├── GameFlow/
│   │   │   ├── GameManager.cs
│   │   │   ├── TurnManager.cs
│   │   │   ├── GameState.cs
│   │   │   └── GameEvents.cs
│   │   ├── Players/
│   │   │   ├── Player.cs
│   │   │   ├── PlayerHand.cs
│   │   │   └── PlayerActions.cs
│   │   ├── UI/
│   │   │   ├── UIManager.cs
│   │   │   ├── CardUI.cs
│   │   │   ├── DeckUI.cs
│   │   │   └── HandUI.cs
│   │   ├── Networking/
│   │   │   ├── NetworkManager.cs
│   │   │   └── NetworkEvents.cs
│   │   └── Utilities/
│   │       ├── Extensions.cs
│   │       ├── Constants.cs
│   │       └── Helpers.cs
│   ├── Prefabs/
│   │   ├── Cards/
│   │   │   ├── BaseCard.prefab
│   │   │   └── CardTypes/
│   │   ├── UI/
│   │   │   ├── CardSlot.prefab
│   │   │   ├── DeckArea.prefab
│   │   │   └── PlayerHand.prefab
│   │   └── Game/
│   │       ├── GameManager.prefab
│   │       └── Player.prefab
│   ├── Scenes/
│   │   ├── MainMenu.unity
│   │   ├── GameScene.unity
│   │   └── TestScenes/
│   │       ├── CardTest.unity
│   │       └── DeckTest.unity
│   ├── ScriptableObjects/
│   │   ├── Cards/
│   │   │   ├── CardData/
│   │   │   └── CardSets/
│   │   └── GameSettings/
│   ├── Art/
│   │   ├── Textures/
│   │   │   ├── Cards/
│   │   │   └── UI/
│   │   ├── Materials/
│   │   └── Sprites/
│   ├── Audio/
│   │   ├── Music/
│   │   ├── SFX/
│   │   └── Mixers/
│   └── Resources/
│       ├── Cards/
│       └── Settings/
├── ProjectSettings/
└── Packages/
```

## Folder Descriptions

### Scripts/
The main codebase organized by functionality:

- **Cards/**: All card-related scripts including base classes, data structures, and specific card implementations
- **Decks/**: Deck management, building, and manipulation logic
- **GameFlow/**: Core game state management, turn handling, and game progression
- **Players/**: Player-specific logic, hand management, and player actions
- **UI/**: User interface components and managers
- **Networking/**: Multiplayer networking code (for future implementation)
- **Utilities/**: Helper classes, extensions, and shared utilities

### Prefabs/
Reusable GameObjects organized by category:

- **Cards/**: Card prefab variants and templates
- **UI/**: User interface prefabs and components
- **Game/**: Core game object prefabs (managers, players)

### Scenes/
Unity scenes for different game states:

- **MainMenu.unity**: Main menu and navigation
- **GameScene.unity**: Primary gameplay scene
- **TestScenes/**: Development and testing scenes

### ScriptableObjects/
Data assets for configuration:

- **Cards/**: Card data definitions and card set collections
- **GameSettings/**: Game configuration and balance settings

### Art/
Visual assets organized by type:

- **Textures/**: Image files for cards, UI, and game elements
- **Materials/**: Unity materials for 3D objects
- **Sprites/**: 2D sprites for UI and card art

### Audio/
Sound assets:

- **Music/**: Background music tracks
- **SFX/**: Sound effects for game actions
- **Mixers/**: Audio mixer configurations

## Key Principles

### 1. Separation of Concerns
Each folder has a specific responsibility, making it easy to locate and modify code.

### 2. Scalability
The structure accommodates growth from a simple prototype to a full-featured game.

### 3. Collaboration-Friendly
Clear organization helps team members understand where to find and place different types of assets.

### 4. Unity Best Practices
Follows Unity's recommended practices for project organization and asset management.

## Implementation Notes

### Starting Small
Begin with the core folders (Scripts/Cards, Scripts/Decks, Scripts/GameFlow) and expand as needed.

### Asset References
Use Resources folders sparingly; prefer direct references and ScriptableObject assets for better performance and organization.

### Version Control
Structure supports good version control practices with logical groupings that minimize merge conflicts.

### Testing
Dedicated test scenes allow for isolated testing of specific game systems.

## Migration Guide

If you're working with an existing Unity project:

1. Create the folder structure gradually
2. Move existing scripts to appropriate folders
3. Update script references and using statements
4. Reorganize prefabs and assets
5. Update scene references to moved assets

This structure serves as a guideline - adapt it based on your specific project needs and team preferences.