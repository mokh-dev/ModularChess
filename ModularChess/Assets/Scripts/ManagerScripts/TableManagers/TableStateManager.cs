using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TableStateManager : Singleton<TableStateManager>
{
    [SerializeField] private List<CardData> _startingPlayerDeck;

    private TableState currentTableState => GameStateManager.Instance.GameStates.Last().TableGameState;
    private BoardState currentBoardState => GameStateManager.Instance.GameStates.Last().BoardGameState;


    private void UpdateTableState(TableState updatedTableState)
    {
        
    }

    public TableState InitializeTable()
    {
        return new TableState
        {
            PlayerSide = GetInitialTableSide(_startingPlayerDeck),

            DiscardPile = new List<Card>()
        };
    }

    private TableSide GetInitialTableSide(List<CardData> startingDeckData)
    {
        List<Card> startingDeck = new List<Card>();

        foreach (CardData cardData in startingDeckData)
        {
            startingDeck.Add(new Card(cardData));
        }

        return new TableSide
        {
            Hand = new List<Card>(),
            Field = new List<Card>(),

            Deck = startingDeck
        };
    }



    public void DrawCardFromDeck()
    {
        Card drawnCard = currentTableState.PlayerSide.Deck[0];

        List<Card> updatedDeck = new List<Card>(currentTableState.PlayerSide.Deck);
        updatedDeck.RemoveAt(0);
        
        List<Card> updatedHand = new List<Card>(currentTableState.PlayerSide.Hand);
        updatedHand.Add(drawnCard);


        TableSide updatedTableSide = new TableSide
        {
            Hand = updatedHand,
            Field = currentTableState.PlayerSide.Field,

            Deck = updatedDeck
        };


        TableState updatedTableState = new TableState
        {
            PlayerSide = updatedTableSide,

            DiscardPile = currentTableState.DiscardPile
        };

        GameStateManager.Instance.UpdateTableGameState(updatedTableState);
    }


    public void PlayCharacterCardToField(Card characterCard, Teams cardTeam)
    {
        Vector2 tempTestingSpawnPosition = (cardTeam == Teams.Player) ? new Vector2(0,0) : new Vector2(0,7);
        
        List<Card> updatedHand = new List<Card>(currentTableState.PlayerSide.Hand);
        updatedHand.Remove(characterCard);
        
        List<Card> updatedField = new List<Card>(currentTableState.PlayerSide.Field);
        updatedField.Add(characterCard);


        TableSide updatedTableSide = new TableSide
        {
            Hand = updatedHand,
            Field = updatedField,

            Deck = currentTableState.PlayerSide.Deck
        };

        TableState updatedTableState = new TableState
        {
            PlayerSide = updatedTableSide,

            DiscardPile = currentTableState.DiscardPile
        };



        GameState updatedGameState = new GameState
        {
            TableGameState = updatedTableState,
            BoardGameState = BoardStateManager.Instance.GetBoardWithAddedPiece(tempTestingSpawnPosition, new Piece(characterCard.CharacterPieceData))
        };

        
       GameStateManager.Instance.UpdateGameState(updatedGameState);
    }

    private bool IsCardInHand(Card card)
    {
        List<Card> handToCheck = currentTableState.PlayerSide.Hand;
        if (handToCheck.Contains(card)) return true;
        return false;
    }
}
