# Prefabs Outline

This document outlines the essential prefabs needed for the cooperative deck-builder game, focusing on the Card and Deck systems.

## Overview

Prefabs are reusable GameObjects that form the building blocks of the deck-builder game. This outline provides specifications for the core prefabs, their components, and how they interact with each other.

## Core Card Prefabs

### BaseCard.prefab

The foundation prefab for all cards in the game.

#### Components:
- **Transform**: Position, rotation, scale
- **Card (Script)**: Core card logic and data
- **CardRenderer (Script)**: Visual representation and animations
- **CardInteraction (Script)**: Mouse/touch interaction handling
- **Collider2D**: For click detection and hover events
- **SpriteRenderer**: Card artwork and visual state
- **Canvas**: For UI elements on the card
  - **CardUI (Script)**: UI management for the card

#### Child Objects:
```
BaseCard
├── CardArt
│   └── SpriteRenderer (Card artwork)
├── CardFrame
│   └── SpriteRenderer (Card border/frame)
├── CardUI (Canvas - World Space)
│   ├── TitleText (TextMeshPro)
│   ├── CostText (TextMeshPro)
│   ├── DescriptionText (TextMeshPro)
│   ├── TypeIcon (Image)
│   └── EffectIcons (Image)
└── SelectionHighlight
    └── SpriteRenderer (Highlight effect)
```

#### Key Features:
- Modular design allowing for card type variations
- Built-in animation hooks for card effects
- Accessibility features for screen readers
- Support for different card sizes and orientations

### AttackCard.prefab (Inherits from BaseCard)

Specialized prefab for attack-type cards.

#### Additional Components:
- **AttackCard (Script)**: Attack-specific logic
- **DamageCalculator (Script)**: Damage computation

#### Visual Differences:
- Red-tinted card frame
- Attack power display
- Target selection indicators

### DefenseCard.prefab (Inherits from BaseCard)

Specialized prefab for defense-type cards.

#### Additional Components:
- **DefenseCard (Script)**: Defense-specific logic
- **ShieldCalculator (Script)**: Defense computation

#### Visual Differences:
- Blue-tinted card frame
- Shield value display
- Protection effect indicators

### UtilityCard.prefab (Inherits from BaseCard)

Specialized prefab for utility/support cards.

#### Additional Components:
- **UtilityCard (Script)**: Utility-specific logic
- **EffectResolver (Script)**: Complex effect handling

#### Visual Differences:
- Green-tinted card frame
- Multi-purpose icon display
- Effect duration indicators

## Core Deck Prefabs

### PlayerDeck.prefab

Physical representation of a player's deck pile.

#### Components:
- **Transform**: Deck position and orientation
- **Deck (Script)**: Core deck functionality
- **DeckRenderer (Script)**: Visual representation
- **DeckInteraction (Script)**: Player interaction handling
- **Collider2D**: For click detection

#### Child Objects:
```
PlayerDeck
├── DeckBase
│   └── SpriteRenderer (Deck base/platform)
├── CardStack
│   ├── CardBack1 (SpriteRenderer)
│   ├── CardBack2 (SpriteRenderer)
│   ├── CardBack3 (SpriteRenderer)
│   └── ... (More card backs for visual depth)
├── DeckUI (Canvas - World Space)
│   ├── CardCountText (TextMeshPro)
│   ├── DeckNameText (TextMeshPro)
│   └── ShuffleButton (Button)
└── EffectParticles
    └── ParticleSystem (For shuffle/draw effects)
```

#### Key Features:
- Visual card count representation
- Shuffle and draw animations
- Drag-and-drop support for deck building
- Integration with deck building mechanics

### SharedDeckArea.prefab

Central area for shared card piles.

#### Components:
- **SharedDeckManager (Script)**: Manages multiple shared decks
- **AreaRenderer (Script)**: Visual area representation

#### Child Objects:
```
SharedDeckArea
├── DrawPile (PlayerDeck variant)
├── DiscardPile
│   ├── DeckBase (SpriteRenderer)
│   ├── TopCard (Card prefab instance)
│   └── DiscardUI (Canvas)
├── SharedCards
│   ├── Card1Slot
│   ├── Card2Slot
│   ├── Card3Slot
│   └── Card4Slot
└── AreaBackground
    └── SpriteRenderer (Table/play area)
```

### DiscardPile.prefab

Visual representation of discarded cards.

#### Components:
- **DiscardPile (Script)**: Discard pile logic
- **DiscardRenderer (Script)**: Visual management

#### Child Objects:
```
DiscardPile
├── PileBase
│   └── SpriteRenderer (Discard area)
├── TopCard
│   └── Card (Most recently discarded card)
├── CardSpread
│   ├── Card1 (Slightly offset)
│   ├── Card2 (Slightly offset)
│   └── Card3 (Slightly offset)
└── DiscardUI (Canvas)
    ├── CardCountText (TextMeshPro)
    └── ViewPileButton (Button)
```

## UI Prefabs

### PlayerHand.prefab

Container for cards in a player's hand.

#### Components:
- **PlayerHand (Script)**: Hand management logic
- **HandLayout (Script)**: Card positioning and spacing
- **HandInteraction (Script)**: Hand-specific interactions

#### Child Objects:
```
PlayerHand
├── HandArea
│   └── RectTransform (Hand boundaries)
├── CardSlots
│   ├── Slot1 (CardSlot prefab)
│   ├── Slot2 (CardSlot prefab)
│   ├── ... (Up to max hand size)
│   └── SlotN (CardSlot prefab)
└── HandEffects
    └── ParticleSystem (For card draw effects)
```

### CardSlot.prefab

Individual slot for holding cards.

#### Components:
- **CardSlot (Script)**: Slot logic and card management
- **SlotRenderer (Script)**: Visual feedback

#### Child Objects:
```
CardSlot
├── SlotBase
│   └── Image (Slot background/outline)
├── DropIndicator
│   └── Image (Drop target highlight)
└── SlotEffects
    └── ParticleSystem (For card placement effects)
```

## Interactive Prefabs

### DeckBuilder.prefab

UI interface for building and modifying decks.

#### Components:
- **DeckBuilder (Script)**: Deck building logic
- **DeckBuilderUI (Script)**: UI management

#### Child Objects:
```
DeckBuilder
├── MainPanel (Canvas)
│   ├── AvailableCards (Scroll View)
│   ├── CurrentDeck (Scroll View)
│   ├── DeckStats (Text displays)
│   └── ActionButtons (Save, Load, Clear)
├── CardPreview
│   └── Card (Large preview instance)
└── FilterPanel
    ├── TypeFilter (Dropdown)
    ├── CostFilter (Slider)
    └── SearchField (Input Field)
```

### GameManager.prefab

Central game management prefab.

#### Components:
- **GameManager (Script)**: Core game logic
- **TurnManager (Script)**: Turn sequence management
- **EventManager (Script)**: Game event handling

#### Child Objects:
```
GameManager
├── GameSettings
│   └── GameSettings (ScriptableObject reference)
├── PlayerManager
│   └── PlayerManager (Script)
└── AudioManager
    ├── MusicSource (AudioSource)
    ├── SFXSource (AudioSource)
    └── UIAudioSource (AudioSource)
```

## Effect Prefabs

### CardEffect.prefab

Base prefab for visual card effects.

#### Components:
- **CardEffect (Script)**: Effect logic
- **EffectRenderer (Script)**: Visual rendering

#### Variants:
- **AttackEffect.prefab**: Combat visual effects
- **HealEffect.prefab**: Healing visual effects
- **BuffEffect.prefab**: Enhancement visual effects
- **DebuffEffect.prefab**: Negative effect visuals

## Prefab Usage Guidelines

### Instantiation
- Use object pooling for frequently created/destroyed prefabs (cards, effects)
- Prefer prefab variants over runtime modifications
- Cache prefab references in managers for performance

### Customization
- Modify prefabs through component properties, not structure
- Use ScriptableObjects for data-driven customization
- Create prefab variants for different visual themes

### Performance
- Minimize draw calls by using sprite atlases
- Group similar prefabs for batching
- Use LOD (Level of Detail) for complex prefabs when appropriate

### Testing
- Create simplified test versions of prefabs for debugging
- Include debug visualization components in development builds
- Use Unity's Prefab Mode for isolated testing

## Integration Notes

### Scene Integration
1. Drag prefabs into scenes during setup
2. Configure prefab instances through inspector
3. Connect prefab references between managers
4. Test all prefab interactions before building

### Script Integration
- Prefabs work with the script system outlined in `Assets/Scripts/`
- Each prefab should have corresponding scripts attached
- Use UnityEvents for loose coupling between prefabs

### Asset Pipeline
- Store prefabs in `Assets/Prefabs/` following the folder structure
- Use nested prefabs for complex hierarchies
- Version control prefab files carefully to avoid merge conflicts

This prefab system provides a solid foundation for building a cooperative deck-builder game while maintaining flexibility for future expansion and customization.