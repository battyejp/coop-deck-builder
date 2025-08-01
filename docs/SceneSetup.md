# Scene Setup Guide

This document provides guidance on setting up the Unity scene hierarchy for the cooperative deck-builder game.

## Overview

A well-structured scene hierarchy ensures efficient rendering, logical organization, and easy maintenance. This guide outlines the recommended GameObject structure for the main game scene.

## Main Game Scene Hierarchy

```
GameScene
├── === MANAGERS ===
├── GameManager
├── UIManager
├── AudioManager
├── === LIGHTING ===
├── Main Camera
├── Directional Light
├── === GAME WORLD ===
├── GameArea
│   ├── PlayField
│   │   ├── CenterArea
│   │   │   ├── SharedDecks
│   │   │   │   ├── DrawPile
│   │   │   │   ├── DiscardPile
│   │   │   │   └── SharedCards
│   │   │   ├── ObjectiveArea
│   │   │   │   ├── CurrentObjective
│   │   │   │   └── CompletedObjectives
│   │   │   └── TurnIndicator
│   │   ├── Player1Area
│   │   │   ├── PlayerHand
│   │   │   ├── PlayerDeck
│   │   │   ├── PlayerDiscard
│   │   │   └── PlayerResources
│   │   ├── Player2Area
│   │   │   ├── PlayerHand
│   │   │   ├── PlayerDeck
│   │   │   ├── PlayerDiscard
│   │   │   └── PlayerResources
│   │   ├── Player3Area (Optional)
│   │   │   └── [Same as Player1/2]
│   │   └── Player4Area (Optional)
│   │       └── [Same as Player1/2]
│   └── Background
│       ├── TableSurface
│       └── EnvironmentDecor
├── === UI CANVAS ===
├── MainCanvas (Screen Space - Overlay)
│   ├── GameUI
│   │   ├── TopPanel
│   │   │   ├── TurnCounter
│   │   │   ├── GameTimer
│   │   │   └── ObjectiveText
│   │   ├── PlayerPanels
│   │   │   ├── Player1Panel
│   │   │   │   ├── PlayerName
│   │   │   │   ├── HandCount
│   │   │   │   ├── DeckCount
│   │   │   │   └── Resources
│   │   │   ├── Player2Panel
│   │   │   │   └── [Same structure]
│   │   │   ├── Player3Panel (Optional)
│   │   │   └── Player4Panel (Optional)
│   │   ├── CenterUI
│   │   │   ├── ActionButtons
│   │   │   ├── SharedResourceDisplay
│   │   │   └── TurnActionPanel
│   │   └── BottomPanel
│   │       ├── MenuButton
│   │       ├── SettingsButton
│   │       └── HelpButton
│   ├── CardDetailPanel
│   │   ├── CardImage
│   │   ├── CardTitle
│   │   ├── CardDescription
│   │   ├── CardCost
│   │   └── CloseButton
│   ├── GameMenuPanel
│   │   ├── MenuBackground
│   │   ├── SaveGameButton
│   │   ├── LoadGameButton
│   │   ├── SettingsButton
│   │   ├── MainMenuButton
│   │   └── CloseButton
│   └── NotificationPanel
│       ├── NotificationText
│       └── NotificationTimer
├── WorldCanvas (World Space)
│   ├── CardHoverInfo
│   ├── DeckCounters
│   └── PlayerIndicators
└── === AUDIO ===
    ├── MusicSource
    ├── SFXSource
    └── UIAudioSource
```

## GameObject Descriptions

### Managers
- **GameManager**: Core game logic and state management
- **UIManager**: UI state and interaction management
- **AudioManager**: Sound effect and music coordination

### Camera & Lighting
- **Main Camera**: Positioned to show the entire play area
- **Directional Light**: Main lighting for the scene

### Game World

#### PlayField
The main gameplay area containing all interactive elements.

#### CenterArea
- **SharedDecks**: Common card piles accessible to all players
- **ObjectiveArea**: Current and completed game objectives
- **TurnIndicator**: Visual indicator of current turn state

#### PlayerAreas (1-4)
Each player has a dedicated area containing:
- **PlayerHand**: Cards currently in the player's hand
- **PlayerDeck**: Player's personal draw pile
- **PlayerDiscard**: Player's discard pile
- **PlayerResources**: Player-specific resources and tokens

### UI Canvas

#### MainCanvas (Screen Space - Overlay)
The primary UI layer for game information and controls.

#### TopPanel
- **TurnCounter**: Current turn number
- **GameTimer**: Elapsed game time or turn timer
- **ObjectiveText**: Current objective description

#### PlayerPanels
Information displays for each player showing:
- Player name and status
- Card counts for hand, deck, discard
- Current resources and health

#### CenterUI
- **ActionButtons**: Primary game actions
- **SharedResourceDisplay**: Common resources and game state
- **TurnActionPanel**: Available actions for current turn

#### CardDetailPanel
Modal panel for viewing detailed card information.

#### GameMenuPanel
Pause menu with save/load and settings options.

#### NotificationPanel
System for displaying temporary game messages.

#### WorldCanvas (World Space)
UI elements that exist in 3D space:
- Card hover information
- Deck counters
- Player status indicators

## Setup Instructions

### 1. Create Basic Hierarchy
1. Start with an empty scene
2. Create parent objects for each major section
3. Use empty GameObjects as organizational containers

### 2. Add Manager Scripts
1. Attach manager scripts to their respective GameObjects
2. Configure script references between managers
3. Set up event connections

### 3. Position Game Elements
1. Arrange player areas around the center
2. Position camera to capture all play areas
3. Ensure UI elements don't obstruct game view

### 4. Configure Canvas Settings
1. Set MainCanvas to Screen Space - Overlay
2. Set WorldCanvas to World Space
3. Configure canvas scalers for different screen sizes

### 5. Add Visual Elements
1. Create placeholder sprites or 3D models for game areas
2. Add card slot indicators
3. Include visual feedback elements

## Best Practices

### Naming Conventions
- Use descriptive names for all GameObjects
- Prefix player-specific objects with "Player1", "Player2", etc.
- Group related objects under parent containers

### Organization
- Use empty GameObjects as folders
- Add comments using TextMeshPro components for complex hierarchies
- Maintain consistent indentation and grouping

### Performance Considerations
- Use object pooling for frequently instantiated objects (cards)
- Disable unused UI panels rather than destroying them
- Group static objects under a single parent for batching

### Scalability
- Design for 2-4 players initially
- Make player areas easily duplicatable
- Use prefabs for repeated elements

## Testing Setup

Create a simplified test version with:
1. Basic manager objects
2. Placeholder UI elements
3. Simple card representations
4. Minimal visual styling

This allows testing of core functionality before adding visual polish.

## Multi-Scene Considerations

For larger projects, consider splitting into multiple scenes:
- **GameManagers**: Persistent managers (DontDestroyOnLoad)
- **GameUI**: UI elements loaded additively
- **GameWorld**: 3D game environment loaded additively

This approach allows for faster iteration and better team collaboration.