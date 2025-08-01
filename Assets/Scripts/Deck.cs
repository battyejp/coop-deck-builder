using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CoopDeckBuilder.Cards;

namespace CoopDeckBuilder.Decks
{
    /// <summary>
    /// Represents a deck of cards with standard deck operations like draw, shuffle, and discard.
    /// Supports both player decks and shared game decks.
    /// </summary>
    [System.Serializable]
    public class Deck : MonoBehaviour
    {
        [Header("Deck Configuration")]
        [SerializeField] private string deckName = "Player Deck";
        [SerializeField] private string deckId;
        [SerializeField] private int maxDeckSize = 60;
        [SerializeField] private int initialHandSize = 5;

        [Header("Deck Contents")]
        [SerializeField] private List<Card> cards = new List<Card>();
        [SerializeField] private List<Card> discardPile = new List<Card>();

        [Header("Shuffle Settings")]
        [SerializeField] private bool autoShuffleWhenEmpty = true;
        [SerializeField] private bool shuffleOnStart = true;

        // Events for deck state changes
        public static event Action<Deck, Card> OnCardDrawn;
        public static event Action<Deck, Card> OnCardAdded;
        public static event Action<Deck, Card> OnCardRemoved;
        public static event Action<Deck> OnDeckShuffled;
        public static event Action<Deck> OnDeckEmpty;

        // Properties
        public string DeckName => deckName;
        public string DeckId => deckId;
        public int CardCount => cards.Count;
        public int DiscardCount => discardPile.Count;
        public int TotalCards => CardCount + DiscardCount;
        public bool IsEmpty => CardCount == 0;
        public bool HasCardsToDiscard => discardPile.Count > 0;

        /// <summary>
        /// Initializes the deck on start.
        /// </summary>
        private void Start()
        {
            // Generate unique ID if not set
            if (string.IsNullOrEmpty(deckId))
            {
                deckId = GenerateUniqueId();
            }

            // Set card states to be in deck
            foreach (Card card in cards)
            {
                if (card != null)
                {
                    card.SetCardState(Card.CardState.InDeck);
                }
            }

            // Shuffle on start if enabled
            if (shuffleOnStart)
            {
                Shuffle();
            }
        }

        /// <summary>
        /// Draws a specified number of cards from the top of the deck.
        /// </summary>
        /// <param name="count">Number of cards to draw</param>
        /// <returns>List of drawn cards</returns>
        public List<Card> DrawCards(int count = 1)
        {
            List<Card> drawnCards = new List<Card>();

            for (int i = 0; i < count; i++)
            {
                Card card = DrawCard();
                if (card != null)
                {
                    drawnCards.Add(card);
                }
                else
                {
                    break; // No more cards available
                }
            }

            return drawnCards;
        }

        /// <summary>
        /// Draws a single card from the top of the deck.
        /// </summary>
        /// <returns>The drawn card, or null if deck is empty</returns>
        public Card DrawCard()
        {
            // Check if deck is empty
            if (IsEmpty)
            {
                // Try to auto-shuffle discard pile back into deck
                if (autoShuffleWhenEmpty && HasCardsToDiscard)
                {
                    ShuffleDiscardIntoDeck();
                }
                else
                {
                    OnDeckEmpty?.Invoke(this);
                    Debug.LogWarning($"Cannot draw from empty deck: {deckName}");
                    return null;
                }
            }

            // Draw from top of deck (index 0)
            Card drawnCard = cards[0];
            cards.RemoveAt(0);

            // Update card state
            if (drawnCard != null)
            {
                drawnCard.OnDrawn();
                OnCardDrawn?.Invoke(this, drawnCard);
                Debug.Log($"Drew card: {drawnCard.CardName} from {deckName}");
            }

            return drawnCard;
        }

        /// <summary>
        /// Shuffles the deck using Fisher-Yates algorithm.
        /// </summary>
        public void Shuffle()
        {
            if (CardCount <= 1)
            {
                return; // No need to shuffle
            }

            // Fisher-Yates shuffle algorithm
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int randomIndex = UnityEngine.Random.Range(0, i + 1);
                Card temp = cards[i];
                cards[i] = cards[randomIndex];
                cards[randomIndex] = temp;
            }

            OnDeckShuffled?.Invoke(this);
            Debug.Log($"Shuffled deck: {deckName}");
        }

        /// <summary>
        /// Adds a card to the deck. Can specify position (top, bottom, or random).
        /// </summary>
        /// <param name="card">Card to add</param>
        /// <param name="position">Where to add the card</param>
        /// <returns>True if card was successfully added</returns>
        public bool AddCard(Card card, DeckPosition position = DeckPosition.Bottom)
        {
            if (card == null)
            {
                Debug.LogWarning("Cannot add null card to deck");
                return false;
            }

            if (cards.Count >= maxDeckSize)
            {
                Debug.LogWarning($"Cannot add card to {deckName}: deck is at maximum size ({maxDeckSize})");
                return false;
            }

            // Add card to specified position
            switch (position)
            {
                case DeckPosition.Top:
                    cards.Insert(0, card);
                    break;
                case DeckPosition.Bottom:
                    cards.Add(card);
                    break;
                case DeckPosition.Random:
                    int randomIndex = UnityEngine.Random.Range(0, cards.Count + 1);
                    cards.Insert(randomIndex, card);
                    break;
            }

            // Update card state
            card.SetCardState(Card.CardState.InDeck);

            OnCardAdded?.Invoke(this, card);
            Debug.Log($"Added card: {card.CardName} to {deckName}");

            return true;
        }

        /// <summary>
        /// Removes a specific card from the deck.
        /// </summary>
        /// <param name="card">Card to remove</param>
        /// <returns>True if card was found and removed</returns>
        public bool RemoveCard(Card card)
        {
            if (card == null)
            {
                return false;
            }

            bool removed = cards.Remove(card);
            if (removed)
            {
                OnCardRemoved?.Invoke(this, card);
                Debug.Log($"Removed card: {card.CardName} from {deckName}");
            }

            return removed;
        }

        /// <summary>
        /// Discards a card to the discard pile.
        /// </summary>
        /// <param name="card">Card to discard</param>
        public void DiscardCard(Card card)
        {
            if (card == null)
            {
                return;
            }

            // Remove from deck if it's there
            RemoveCard(card);

            // Add to discard pile
            discardPile.Add(card);
            card.OnDiscarded();

            Debug.Log($"Discarded card: {card.CardName} to {deckName} discard pile");
        }

        /// <summary>
        /// Shuffles the discard pile back into the deck.
        /// </summary>
        public void ShuffleDiscardIntoDeck()
        {
            if (!HasCardsToDiscard)
            {
                Debug.LogWarning($"No cards in discard pile to shuffle into {deckName}");
                return;
            }

            // Move all discard cards back to deck
            foreach (Card card in discardPile)
            {
                if (card != null)
                {
                    cards.Add(card);
                    card.SetCardState(Card.CardState.InDeck);
                }
            }

            // Clear discard pile
            discardPile.Clear();

            // Shuffle the deck
            Shuffle();

            Debug.Log($"Shuffled discard pile into {deckName}");
        }

        /// <summary>
        /// Returns the top card without removing it from the deck.
        /// </summary>
        /// <returns>Top card or null if deck is empty</returns>
        public Card PeekTopCard()
        {
            return IsEmpty ? null : cards[0];
        }

        /// <summary>
        /// Returns the bottom card without removing it from the deck.
        /// </summary>
        /// <returns>Bottom card or null if deck is empty</returns>
        public Card PeekBottomCard()
        {
            return IsEmpty ? null : cards[cards.Count - 1];
        }

        /// <summary>
        /// Returns a copy of all cards currently in the deck.
        /// </summary>
        /// <returns>List of cards in deck</returns>
        public List<Card> GetAllCards()
        {
            return new List<Card>(cards);
        }

        /// <summary>
        /// Returns a copy of all cards in the discard pile.
        /// </summary>
        /// <returns>List of cards in discard pile</returns>
        public List<Card> GetDiscardPile()
        {
            return new List<Card>(discardPile);
        }

        /// <summary>
        /// Searches for cards in the deck by name.
        /// </summary>
        /// <param name="cardName">Name to search for</param>
        /// <returns>List of matching cards</returns>
        public List<Card> FindCardsByName(string cardName)
        {
            return cards.Where(card => card != null && card.CardName.Equals(cardName, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// Searches for cards in the deck by type.
        /// </summary>
        /// <param name="cardType">Type to search for</param>
        /// <returns>List of matching cards</returns>
        public List<Card> FindCardsByType(CardType cardType)
        {
            return cards.Where(card => card != null && card.Type == cardType).ToList();
        }

        /// <summary>
        /// Clears all cards from the deck and discard pile.
        /// </summary>
        public void ClearDeck()
        {
            cards.Clear();
            discardPile.Clear();
            Debug.Log($"Cleared all cards from {deckName}");
        }

        /// <summary>
        /// Creates a copy of this deck with the same configuration.
        /// </summary>
        /// <returns>New deck instance</returns>
        public Deck CreateCopy()
        {
            GameObject deckObject = new GameObject($"{deckName}_Copy");
            Deck newDeck = deckObject.AddComponent<Deck>();

            newDeck.deckName = $"{deckName}_Copy";
            newDeck.deckId = GenerateUniqueId();
            newDeck.maxDeckSize = maxDeckSize;
            newDeck.initialHandSize = initialHandSize;
            newDeck.autoShuffleWhenEmpty = autoShuffleWhenEmpty;
            newDeck.shuffleOnStart = shuffleOnStart;

            // Copy all cards
            foreach (Card card in cards)
            {
                if (card != null)
                {
                    newDeck.AddCard(card.CreateCopy());
                }
            }

            return newDeck;
        }

        /// <summary>
        /// Generates a unique identifier for the deck.
        /// </summary>
        /// <returns>Unique string ID</returns>
        private string GenerateUniqueId()
        {
            return $"{deckName}_{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        /// <summary>
        /// Returns string representation of the deck for debugging.
        /// </summary>
        /// <returns>String description of the deck</returns>
        public override string ToString()
        {
            return $"{deckName} (Cards: {CardCount}, Discard: {DiscardCount})";
        }

        /// <summary>
        /// Enumeration for deck positions when adding cards.
        /// </summary>
        public enum DeckPosition
        {
            Top,
            Bottom,
            Random
        }
    }
}