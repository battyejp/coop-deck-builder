using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoopDeckBuilder.Cards;
using CoopDeckBuilder.Decks;

namespace CoopDeckBuilder.GameFlow
{
    /// <summary>
    /// Enumeration of possible game states.
    /// </summary>
    public enum GameState
    {
        MainMenu,
        GameSetup,
        PlayerTurn,
        Processing,
        GameOver,
        Paused
    }

    /// <summary>
    /// Enumeration of turn phases within a player's turn.
    /// </summary>
    public enum TurnPhase
    {
        StartTurn,
        DrawPhase,
        ActionPhase,
        EndTurn
    }

    /// <summary>
    /// Central game manager that controls the overall game flow, state, and coordination
    /// between different game systems in the cooperative deck-builder.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Game Configuration")]
        [SerializeField] private int maxPlayers = 4;
        [SerializeField] private int currentPlayerCount = 2;
        [SerializeField] private float turnTimeLimit = 120f; // 2 minutes per turn
        [SerializeField] private bool enableTurnTimer = false;

        [Header("Game State")]
        [SerializeField] private GameState currentGameState = GameState.MainMenu;
        [SerializeField] private TurnPhase currentTurnPhase = TurnPhase.StartTurn;
        [SerializeField] private int currentPlayerIndex = 0;
        [SerializeField] private int turnNumber = 1;

        [Header("Victory Conditions")]
        [SerializeField] private int victoryPoints = 0;
        [SerializeField] private int victoryPointsRequired = 100;
        [SerializeField] private bool cooperativeVictory = true;

        [Header("Game Objects")]
        [SerializeField] private List<GameObject> players = new List<GameObject>();
        [SerializeField] private GameObject sharedGameArea;

        // Managers and Systems
        private TurnManager turnManager;
        private UIManager uiManager;
        private AudioManager audioManager;

        // Timer for turn limits
        private float currentTurnTimer = 0f;
        private Coroutine turnTimerCoroutine;

        // Events for game state changes
        public static event Action<GameState> OnGameStateChanged;
        public static event Action<TurnPhase> OnTurnPhaseChanged;
        public static event Action<int> OnPlayerTurnChanged;
        public static event Action<int> OnTurnNumberChanged;
        public static event Action OnGameStarted;
        public static event Action OnGameEnded;
        public static event Action OnVictoryAchieved;
        public static event Action OnDefeatOccurred;

        // Properties
        public GameState CurrentGameState => currentGameState;
        public TurnPhase CurrentTurnPhase => currentTurnPhase;
        public int CurrentPlayerIndex => currentPlayerIndex;
        public int TurnNumber => turnNumber;
        public int VictoryPoints => victoryPoints;
        public int PlayerCount => currentPlayerCount;
        public bool IsGameActive => currentGameState == GameState.PlayerTurn || currentGameState == GameState.Processing;

        // Singleton pattern for easy access
        public static GameManager Instance { get; private set; }

        /// <summary>
        /// Initialize the GameManager singleton and set up initial state.
        /// </summary>
        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeGameManager();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Initialize game components and systems.
        /// </summary>
        private void InitializeGameManager()
        {
            // Initialize sub-managers
            turnManager = GetComponent<TurnManager>() ?? gameObject.AddComponent<TurnManager>();
            
            // Find or create UI and Audio managers
            uiManager = FindObjectOfType<UIManager>();
            audioManager = FindObjectOfType<AudioManager>();

            // Subscribe to card and deck events
            SubscribeToGameEvents();

            Debug.Log("GameManager initialized");
        }

        /// <summary>
        /// Subscribe to various game events for coordination.
        /// </summary>
        private void SubscribeToGameEvents()
        {
            // Card events
            Card.OnCardPlayed += HandleCardPlayed;
            Card.OnCardDrawn += HandleCardDrawn;

            // Deck events  
            Deck.OnDeckEmpty += HandleDeckEmpty;
            Deck.OnDeckShuffled += HandleDeckShuffled;
        }

        /// <summary>
        /// Start a new game with the specified number of players.
        /// </summary>
        /// <param name="playerCount">Number of players (2-4)</param>
        public void StartNewGame(int playerCount = 2)
        {
            if (playerCount < 2 || playerCount > maxPlayers)
            {
                Debug.LogError($"Invalid player count: {playerCount}. Must be between 2 and {maxPlayers}");
                return;
            }

            currentPlayerCount = playerCount;
            InitializeGameSession();
            ChangeGameState(GameState.GameSetup);
            
            OnGameStarted?.Invoke();
            Debug.Log($"Starting new game with {playerCount} players");
        }

        /// <summary>
        /// Initialize a new game session.
        /// </summary>
        private void InitializeGameSession()
        {
            // Reset game variables
            turnNumber = 1;
            currentPlayerIndex = 0;
            victoryPoints = 0;

            // Reset timer
            currentTurnTimer = 0f;
            if (turnTimerCoroutine != null)
            {
                StopCoroutine(turnTimerCoroutine);
            }

            // Initialize players
            SetupPlayers();

            // Setup shared game area
            SetupSharedGameArea();
        }

        /// <summary>
        /// Set up player objects and their decks.
        /// </summary>
        private void SetupPlayers()
        {
            // Ensure we have the right number of player objects
            while (players.Count < currentPlayerCount)
            {
                GameObject newPlayer = new GameObject($"Player{players.Count + 1}");
                players.Add(newPlayer);
            }

            // Disable extra players
            for (int i = currentPlayerCount; i < players.Count; i++)
            {
                if (players[i] != null)
                {
                    players[i].SetActive(false);
                }
            }

            // Initialize active players
            for (int i = 0; i < currentPlayerCount; i++)
            {
                if (players[i] != null)
                {
                    players[i].SetActive(true);
                    InitializePlayer(players[i], i);
                }
            }
        }

        /// <summary>
        /// Initialize a single player with starting deck and hand.
        /// </summary>
        /// <param name="player">Player GameObject</param>
        /// <param name="playerIndex">Player's index</param>
        private void InitializePlayer(GameObject player, int playerIndex)
        {
            // Add or get Deck component
            Deck playerDeck = player.GetComponent<Deck>();
            if (playerDeck == null)
            {
                playerDeck = player.AddComponent<Deck>();
            }

            // TODO: Add starting cards to player deck
            // This would typically load from a predefined starting deck configuration

            Debug.Log($"Initialized Player {playerIndex + 1}");
        }

        /// <summary>
        /// Set up the shared game area with common decks and objectives.
        /// </summary>
        private void SetupSharedGameArea()
        {
            if (sharedGameArea == null)
            {
                sharedGameArea = new GameObject("SharedGameArea");
            }

            // TODO: Initialize shared decks, objectives, and other shared game elements
            Debug.Log("Shared game area initialized");
        }

        /// <summary>
        /// Change the current game state and notify listeners.
        /// </summary>
        /// <param name="newState">New game state</param>
        public void ChangeGameState(GameState newState)
        {
            if (currentGameState == newState)
            {
                return; // No change needed
            }

            GameState previousState = currentGameState;
            currentGameState = newState;

            // Handle state-specific logic
            HandleGameStateChange(previousState, newState);

            OnGameStateChanged?.Invoke(newState);
            Debug.Log($"Game state changed: {previousState} -> {newState}");
        }

        /// <summary>
        /// Handle logic for game state transitions.
        /// </summary>
        /// <param name="previousState">Previous game state</param>
        /// <param name="newState">New game state</param>
        private void HandleGameStateChange(GameState previousState, GameState newState)
        {
            switch (newState)
            {
                case GameState.GameSetup:
                    // Setup complete, start first turn
                    StartCoroutine(BeginGameplayAfterSetup());
                    break;

                case GameState.PlayerTurn:
                    BeginPlayerTurn();
                    break;

                case GameState.Processing:
                    // Handle game processing state
                    break;

                case GameState.GameOver:
                    EndGame();
                    break;

                case GameState.Paused:
                    PauseGame();
                    break;
            }
        }

        /// <summary>
        /// Coroutine to begin gameplay after setup is complete.
        /// </summary>
        private IEnumerator BeginGameplayAfterSetup()
        {
            yield return new WaitForSeconds(1f); // Brief pause for setup completion
            ChangeGameState(GameState.PlayerTurn);
        }

        /// <summary>
        /// Begin a player's turn.
        /// </summary>
        private void BeginPlayerTurn()
        {
            ChangeTurnPhase(TurnPhase.StartTurn);

            // Start turn timer if enabled
            if (enableTurnTimer && turnTimeLimit > 0)
            {
                StartTurnTimer();
            }
        }

        /// <summary>
        /// Change the current turn phase.
        /// </summary>
        /// <param name="newPhase">New turn phase</param>
        public void ChangeTurnPhase(TurnPhase newPhase)
        {
            if (currentTurnPhase == newPhase)
            {
                return;
            }

            currentTurnPhase = newPhase;
            OnTurnPhaseChanged?.Invoke(newPhase);

            // Handle phase-specific logic
            HandleTurnPhase(newPhase);

            Debug.Log($"Turn phase changed to: {newPhase}");
        }

        /// <summary>
        /// Handle logic for turn phase changes.
        /// </summary>
        /// <param name="phase">Current turn phase</param>
        private void HandleTurnPhase(TurnPhase phase)
        {
            switch (phase)
            {
                case TurnPhase.StartTurn:
                    // Trigger start of turn effects
                    break;

                case TurnPhase.DrawPhase:
                    // Handle card drawing
                    DrawCards();
                    break;

                case TurnPhase.ActionPhase:
                    // Player can now take actions
                    break;

                case TurnPhase.EndTurn:
                    // End turn processing
                    EndCurrentPlayerTurn();
                    break;
            }
        }

        /// <summary>
        /// Handle card drawing for the current player.
        /// </summary>
        private void DrawCards()
        {
            GameObject currentPlayer = GetCurrentPlayer();
            if (currentPlayer != null)
            {
                Deck playerDeck = currentPlayer.GetComponent<Deck>();
                if (playerDeck != null)
                {
                    // Draw starting hand or additional cards
                    List<Card> drawnCards = playerDeck.DrawCards(1); // Draw 1 card per turn
                    Debug.Log($"Player {currentPlayerIndex + 1} drew {drawnCards.Count} cards");
                }
            }

            // Move to action phase
            ChangeTurnPhase(TurnPhase.ActionPhase);
        }

        /// <summary>
        /// End the current player's turn and advance to next player.
        /// </summary>
        public void EndCurrentPlayerTurn()
        {
            // Stop turn timer
            if (turnTimerCoroutine != null)
            {
                StopCoroutine(turnTimerCoroutine);
                turnTimerCoroutine = null;
            }

            // Advance to next player
            AdvanceToNextPlayer();
        }

        /// <summary>
        /// Advance to the next player's turn.
        /// </summary>
        private void AdvanceToNextPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % currentPlayerCount;

            // If we've completed a full round, increment turn number
            if (currentPlayerIndex == 0)
            {
                turnNumber++;
                OnTurnNumberChanged?.Invoke(turnNumber);
            }

            OnPlayerTurnChanged?.Invoke(currentPlayerIndex);

            // Check victory conditions
            if (CheckVictoryConditions())
            {
                ChangeGameState(GameState.GameOver);
                return;
            }

            // Start next player's turn
            BeginPlayerTurn();
        }

        /// <summary>
        /// Get the currently active player GameObject.
        /// </summary>
        /// <returns>Current player GameObject</returns>
        public GameObject GetCurrentPlayer()
        {
            if (currentPlayerIndex >= 0 && currentPlayerIndex < players.Count)
            {
                return players[currentPlayerIndex];
            }
            return null;
        }

        /// <summary>
        /// Start the turn timer coroutine.
        /// </summary>
        private void StartTurnTimer()
        {
            currentTurnTimer = turnTimeLimit;
            turnTimerCoroutine = StartCoroutine(TurnTimerCoroutine());
        }

        /// <summary>
        /// Coroutine for managing turn time limits.
        /// </summary>
        private IEnumerator TurnTimerCoroutine()
        {
            while (currentTurnTimer > 0 && IsGameActive)
            {
                currentTurnTimer -= Time.deltaTime;
                yield return null;
            }

            // Time limit reached - force end turn
            if (IsGameActive)
            {
                Debug.Log("Turn time limit reached - forcing end turn");
                ChangeTurnPhase(TurnPhase.EndTurn);
            }
        }

        /// <summary>
        /// Check if victory conditions have been met.
        /// </summary>
        /// <returns>True if victory conditions are met</returns>
        private bool CheckVictoryConditions()
        {
            // Basic victory condition - reaching required victory points
            if (victoryPoints >= victoryPointsRequired)
            {
                OnVictoryAchieved?.Invoke();
                return true;
            }

            // TODO: Add other victory/defeat conditions
            return false;
        }

        /// <summary>
        /// Add victory points to the team total.
        /// </summary>
        /// <param name="points">Points to add</param>
        public void AddVictoryPoints(int points)
        {
            victoryPoints += points;
            Debug.Log($"Victory points: {victoryPoints}/{victoryPointsRequired}");

            // Check for victory
            if (CheckVictoryConditions())
            {
                ChangeGameState(GameState.GameOver);
            }
        }

        /// <summary>
        /// End the current game.
        /// </summary>
        private void EndGame()
        {
            // Stop any running timers
            if (turnTimerCoroutine != null)
            {
                StopCoroutine(turnTimerCoroutine);
                turnTimerCoroutine = null;
            }

            OnGameEnded?.Invoke();
            Debug.Log("Game ended");
        }

        /// <summary>
        /// Pause the current game.
        /// </summary>
        public void PauseGame()
        {
            Time.timeScale = 0f;
            Debug.Log("Game paused");
        }

        /// <summary>
        /// Resume the current game.
        /// </summary>
        public void ResumeGame()
        {
            Time.timeScale = 1f;
            ChangeGameState(GameState.PlayerTurn);
            Debug.Log("Game resumed");
        }

        /// <summary>
        /// Event handler for when a card is played.
        /// </summary>
        /// <param name="card">Card that was played</param>
        private void HandleCardPlayed(Card card)
        {
            Debug.Log($"GameManager: Card played - {card.CardName}");
            // Add any game-wide card play effects here
        }

        /// <summary>
        /// Event handler for when a card is drawn.
        /// </summary>
        /// <param name="card">Card that was drawn</param>
        private void HandleCardDrawn(Card card)
        {
            Debug.Log($"GameManager: Card drawn - {card.CardName}");
        }

        /// <summary>
        /// Event handler for when a deck becomes empty.
        /// </summary>
        /// <param name="deck">Deck that became empty</param>
        private void HandleDeckEmpty(Deck deck)
        {
            Debug.Log($"GameManager: Deck empty - {deck.DeckName}");
        }

        /// <summary>
        /// Event handler for when a deck is shuffled.
        /// </summary>
        /// <param name="deck">Deck that was shuffled</param>
        private void HandleDeckShuffled(Deck deck)
        {
            Debug.Log($"GameManager: Deck shuffled - {deck.DeckName}");
        }

        /// <summary>
        /// Clean up subscriptions when destroyed.
        /// </summary>
        private void OnDestroy()
        {
            // Unsubscribe from events
            Card.OnCardPlayed -= HandleCardPlayed;
            Card.OnCardDrawn -= HandleCardDrawn;
            Deck.OnDeckEmpty -= HandleDeckEmpty;
            Deck.OnDeckShuffled -= HandleDeckShuffled;
        }
    }

    /// <summary>
    /// Simple placeholder classes for managers that would be implemented separately.
    /// </summary>
    public class TurnManager : MonoBehaviour
    {
        // Placeholder for turn-specific logic
    }

    public class UIManager : MonoBehaviour
    {
        // Placeholder for UI management
    }

    public class AudioManager : MonoBehaviour
    {
        // Placeholder for audio management
    }
}