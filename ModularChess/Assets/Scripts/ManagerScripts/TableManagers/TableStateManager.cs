using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TableStateManager : MonoBehaviour //TODO check if State managers need to derive from monobehaviour
{
    private static TableStateManager instance;
    public static TableStateManager Instance { get { return instance; } }

    [SerializeField] private List<Card> _startingPlayerDeck;
    [SerializeField] private List<Card> _startingEnemyDeck;

    private TableState currentTableState => GameStateManager.Instance.GameStates.Last().TableGameState;
    private BoardState currentBoardState => GameStateManager.Instance.GameStates.Last().BoardGameState;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    private void UpdateTableState(TableState updatedTableState)
    {
        
    }

    public TableState InitializeTable()
    {
        return new TableState
        {
            PlayerSide = GetInitialTableSide(_startingPlayerDeck),
            EnemySide = GetInitialTableSide(_startingEnemyDeck),

            DiscardPile = new List<Card>()
        };
    }

    private TableSide GetInitialTableSide(List<Card> startingDeck)
    {
        return new TableSide
        {
            Hand = new List<Card>(),
            Field = new List<Card>(),

            Deck = startingDeck
        };
    }



    public void DrawCardFromDeck(Sides deckSide)
    {
        TableSide tableSide = (deckSide == Sides.Player) ? currentTableState.PlayerSide : currentTableState.EnemySide;

        Card drawnCard = tableSide.Deck[0];

        List<Card> updatedDeck = new List<Card>(tableSide.Deck);
        updatedDeck.RemoveAt(0);
        
        List<Card> updatedHand = new List<Card>(tableSide.Hand);
        updatedHand.Add(drawnCard);


        TableSide updatedTableSide = new TableSide
        {
            Hand = updatedHand,
            Field = tableSide.Field,

            Deck = updatedDeck
        };


        TableState updatedTableState = new TableState
        {
            PlayerSide = (deckSide == Sides.Player) ? updatedTableSide : currentTableState.PlayerSide,
            EnemySide = (deckSide == Sides.Player) ? currentTableState.EnemySide : updatedTableSide,

            DiscardPile = currentTableState.DiscardPile
        };

        GameStateManager.Instance.UpdateTableGameState(updatedTableState);
    }


    public void PlayCharacterCardToField(Card characterCard, Sides cardSide)
    {
        if (IsCardInHand(characterCard, cardSide) == false) return;

        Vector2 tempTestingSpawnPosition = (cardSide == Sides.Player) ? new Vector2(0,0) : new Vector2(0,7);
        TableSide initialTableSide = (cardSide == Sides.Player) ? currentTableState.PlayerSide : currentTableState.EnemySide;

        Piece characterCardPiece = Instantiate(characterCard.CharacterPiece);
        characterCardPiece.Team = cardSide;

        
        List<Card> updatedHand = new List<Card>(initialTableSide.Hand);
        updatedHand.Remove(characterCard);
        
        List<Card> updatedField = new List<Card>(initialTableSide.Field);
        updatedField.Add(characterCard);


        TableSide updatedTableSide = new TableSide
        {
            Hand = updatedHand,
            Field = updatedField,

            Deck = initialTableSide.Deck
        };

        TableState updatedTableState = new TableState
        {
            PlayerSide = (cardSide == Sides.Player) ? updatedTableSide : currentTableState.PlayerSide,
            EnemySide = (cardSide == Sides.Player) ? currentTableState.EnemySide : updatedTableSide,

            DiscardPile = currentTableState.DiscardPile
        };



        GameState updatedGameState = new GameState
        {
            TableGameState = updatedTableState,
            BoardGameState = BoardStateManager.Instance.GetBoardWithAddedPiece(tempTestingSpawnPosition, characterCardPiece)
        };

        
       GameStateManager.Instance.UpdateGameState(updatedGameState);
    }

    private bool IsCardInHand(Card card, Sides handSide)
    {
        List<Card> handToCheck = (handSide == Sides.Player) ? currentTableState.PlayerSide.Hand : currentTableState.EnemySide.Hand;
        if (handToCheck.Contains(card)) return true;
        return false;
    }
}
