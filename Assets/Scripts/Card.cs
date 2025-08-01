using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoopDeckBuilder.Cards
{
    /// <summary>
    /// Enumeration of different card types in the game.
    /// </summary>
    public enum CardType
    {
        Attack,
        Defense,
        Utility,
        Resource,
        Event
    }

    /// <summary>
    /// Enumeration of card rarities.
    /// </summary>
    public enum CardRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    /// <summary>
    /// Base card class representing a single card in the deck-building game.
    /// This class contains all the fundamental properties and behaviors that cards share.
    /// </summary>
    [System.Serializable]
    public class Card : MonoBehaviour
    {
        [Header("Card Identity")]
        [SerializeField] private string cardName;
        [SerializeField] private string cardId;
        [SerializeField] private CardType cardType;
        [SerializeField] private CardRarity rarity;

        [Header("Card Stats")]
        [SerializeField] private int cost;
        [SerializeField] private int attackValue;
        [SerializeField] private int defenseValue;
        [SerializeField] private int utilityValue;

        [Header("Card Description")]
        [SerializeField] private string description;
        [SerializeField] private string flavorText;

        [Header("Visual Elements")]
        [SerializeField] private Sprite cardArtwork;
        [SerializeField] private Sprite cardFrame;

        [Header("Gameplay Properties")]
        [SerializeField] private bool isPlayable = true;
        [SerializeField] private bool requiresTarget = false;
        [SerializeField] private bool canTargetSelf = false;
        [SerializeField] private bool canTargetAllies = true;
        [SerializeField] private bool canTargetEnemies = false;

        [Header("Card Effects")]
        [SerializeField] private List<string> cardEffects = new List<string>();

        // Events for card state changes
        public static event Action<Card> OnCardPlayed;
        public static event Action<Card> OnCardDrawn;
        public static event Action<Card> OnCardDiscarded;

        // Properties with public accessors
        public string CardName => cardName;
        public string CardId => cardId;
        public CardType Type => cardType;
        public CardRarity Rarity => rarity;
        public int Cost => cost;
        public int AttackValue => attackValue;
        public int DefenseValue => defenseValue;
        public int UtilityValue => utilityValue;
        public string Description => description;
        public string FlavorText => flavorText;
        public Sprite CardArtwork => cardArtwork;
        public Sprite CardFrame => cardFrame;
        public bool IsPlayable => isPlayable;
        public bool RequiresTarget => requiresTarget;
        public bool CanTargetSelf => canTargetSelf;
        public bool CanTargetAllies => canTargetAllies;
        public bool CanTargetEnemies => canTargetEnemies;
        public List<string> CardEffects => new List<string>(cardEffects);

        // Current state properties
        public bool IsInHand { get; private set; } = false;
        public bool IsInDeck { get; private set; } = false;
        public bool IsInDiscard { get; private set; } = false;
        public bool IsInPlay { get; private set; } = false;

        /// <summary>
        /// Initializes the card with basic data. Called when the card is first created.
        /// </summary>
        private void Awake()
        {
            // Generate a unique ID if one doesn't exist
            if (string.IsNullOrEmpty(cardId))
            {
                cardId = GenerateUniqueId();
            }

            // Set default name if empty
            if (string.IsNullOrEmpty(cardName))
            {
                cardName = "Unnamed Card";
            }
        }

        /// <summary>
        /// Plays this card, triggering its effects and notifying the game system.
        /// </summary>
        /// <param name="target">Optional target for the card effect</param>
        /// <returns>True if the card was successfully played</returns>
        public virtual bool PlayCard(GameObject target = null)
        {
            if (!CanPlayCard())
            {
                Debug.LogWarning($"Cannot play card {cardName}: card is not playable in current state");
                return false;
            }

            if (requiresTarget && target == null)
            {
                Debug.LogWarning($"Cannot play card {cardName}: card requires a target");
                return false;
            }

            // Execute card effects
            ExecuteCardEffects(target);

            // Update card state
            SetCardState(CardState.InPlay);

            // Notify game system
            OnCardPlayed?.Invoke(this);

            Debug.Log($"Card played: {cardName}");
            return true;
        }

        /// <summary>
        /// Checks if the card can be played in the current game state.
        /// </summary>
        /// <returns>True if the card can be played</returns>
        public virtual bool CanPlayCard()
        {
            return isPlayable && IsInHand;
        }

        /// <summary>
        /// Executes the card's effects. Override in derived classes for specific card types.
        /// </summary>
        /// <param name="target">Target for the card effect</param>
        protected virtual void ExecuteCardEffects(GameObject target)
        {
            // Base implementation - can be overridden in specific card types
            Debug.Log($"Executing effects for {cardName}");

            // Process each effect in the card's effect list
            foreach (string effect in cardEffects)
            {
                ProcessEffect(effect, target);
            }
        }

        /// <summary>
        /// Processes a single card effect. Can be expanded for complex effect systems.
        /// </summary>
        /// <param name="effect">Effect string to process</param>
        /// <param name="target">Target for the effect</param>
        protected virtual void ProcessEffect(string effect, GameObject target)
        {
            // Basic effect processing - can be expanded with a full effect system
            Debug.Log($"Processing effect: {effect}");
        }

        /// <summary>
        /// Sets the card's current state and location.
        /// </summary>
        /// <param name="state">New state for the card</param>
        public void SetCardState(CardState state)
        {
            // Reset all states
            IsInHand = false;
            IsInDeck = false;
            IsInDiscard = false;
            IsInPlay = false;

            // Set the new state
            switch (state)
            {
                case CardState.InHand:
                    IsInHand = true;
                    break;
                case CardState.InDeck:
                    IsInDeck = true;
                    break;
                case CardState.InDiscard:
                    IsInDiscard = true;
                    break;
                case CardState.InPlay:
                    IsInPlay = true;
                    break;
            }
        }

        /// <summary>
        /// Called when the card is drawn from a deck.
        /// </summary>
        public virtual void OnDrawn()
        {
            SetCardState(CardState.InHand);
            OnCardDrawn?.Invoke(this);
            Debug.Log($"Card drawn: {cardName}");
        }

        /// <summary>
        /// Called when the card is discarded.
        /// </summary>
        public virtual void OnDiscarded()
        {
            SetCardState(CardState.InDiscard);
            OnCardDiscarded?.Invoke(this);
            Debug.Log($"Card discarded: {cardName}");
        }

        /// <summary>
        /// Creates a copy of this card. Useful for deck building and card generation.
        /// </summary>
        /// <returns>A new Card instance with the same properties</returns>
        public virtual Card CreateCopy()
        {
            Card copy = Instantiate(this);
            copy.cardId = GenerateUniqueId(); // Generate new unique ID for the copy
            return copy;
        }

        /// <summary>
        /// Generates a unique identifier for the card.
        /// </summary>
        /// <returns>Unique string ID</returns>
        private string GenerateUniqueId()
        {
            return $"{cardName}_{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        /// <summary>
        /// Returns a string representation of the card for debugging.
        /// </summary>
        /// <returns>String description of the card</returns>
        public override string ToString()
        {
            return $"{cardName} (Cost: {cost}, Type: {cardType}, Rarity: {rarity})";
        }

        /// <summary>
        /// Enumeration for card states/locations.
        /// </summary>
        public enum CardState
        {
            InDeck,
            InHand,
            InPlay,
            InDiscard
        }
    }
}