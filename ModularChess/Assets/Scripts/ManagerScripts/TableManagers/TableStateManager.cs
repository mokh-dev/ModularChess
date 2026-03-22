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

    private TableState CurrentTableState => GameStateManager.Instance.GameStates.Last().TableGameState;

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



    public void DrawCardFromDeck()
    {
        Card drawnCard = CurrentTableState.PlayerSide.Deck[0];

        List<Card> updatedDeck = new List<Card>(CurrentTableState.PlayerSide.Deck);
        updatedDeck.RemoveAt(0);
        
        List<Card> updatedHand = new List<Card>(CurrentTableState.PlayerSide.Hand);
        updatedHand.Add(drawnCard);


        TableSide updatedPlayerSide = new TableSide
        {
            Hand = updatedHand,
            Field = CurrentTableState.PlayerSide.Field,

            Deck = updatedDeck
        };

        TableState updatedTableState = new TableState
        {
            PlayerSide = updatedPlayerSide,
            EnemySide = CurrentTableState.EnemySide,

            DiscardPile = CurrentTableState.DiscardPile
        };

        GameStateManager.Instance.UpdateTableGameState(updatedTableState);
    }


    public void PlayCharacterCardToField(Card characterCard)
    {
        Vector2 tempTestingSpawnPosition = new Vector2(0,0);

        if (IsCardInHand(characterCard) == false) return;

        List<Card> updatedHand = new List<Card>(CurrentTableState.PlayerSide.Hand);
        updatedHand.Remove(characterCard);
        
        List<Card> updatedField = new List<Card>(CurrentTableState.PlayerSide.Field);
        updatedField.Add(characterCard);


        TableSide updatedPlayerSide = new TableSide
        {
            Hand = updatedHand,
            Field = updatedField,

            Deck = CurrentTableState.PlayerSide.Deck
        };

        TableState updatedTableState = new TableState
        {
            PlayerSide = updatedPlayerSide,
            EnemySide = CurrentTableState.EnemySide,

            DiscardPile = CurrentTableState.DiscardPile
        };

        GameState updatedGameState = new GameState
        {
            TableGameState = updatedTableState,
            BoardGameState = BoardStateManager.Instance.GetBoardWithAddedPiece(tempTestingSpawnPosition, characterCard.CharacterPiece)
        };
        

       GameStateManager.Instance.UpdateGameState(updatedGameState);
    }

    private bool IsCardInHand(Card card)
    {
        if (CurrentTableState.PlayerSide.Hand.Contains(card)) return true;
        return false;
    }
}
