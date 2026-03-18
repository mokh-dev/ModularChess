using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TableStateManager : MonoBehaviour
{
    private static TableStateManager instance;
    public static TableStateManager Instance { get { return instance; } }

    public List<TableState> TableStates = new List<TableState>();
    public TableState CurrentTableState => TableStates.Last();

    [SerializeField] private List<Card> _startingPlayerDeck;
    [SerializeField] private List<Card> _startingEnemyDeck;



    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    void Start()
    {
        InitializeTable();
    }


    private void InitializeTable()
    {
        TableState initialTableState = new TableState
        {
            PlayerSide = GetInitialTableSide(_startingPlayerDeck),
            EnemySide = GetInitialTableSide(_startingEnemyDeck),

            DiscardPile = new List<Card>()
        };

        TableStates.Add(initialTableState);
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

    private void UpdateTableState(TableState updatedTableState)
    {
        TableStates.Add(updatedTableState);
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

        UpdateTableState(updatedTableState);
    }


    public void PrintTableStates() //Editor Inspector Button
    {
        int i = 0;
        foreach (TableState state in TableStates)
        {
            Debug.Log("Table State Num: " + i);
            Debug.Log("Player Side:");


            string deckOutput = "";
            state.PlayerSide.Deck.ForEach(card => deckOutput += $"{card.Title}, ");

            string handOutput = "";
            state.PlayerSide.Hand.ForEach(card => handOutput += $"{card.Title}, ");

            string fieldOutput = "";
            state.PlayerSide.Field.ForEach(card => fieldOutput += $"{card.Title}, ");


            Debug.Log("Deck: " + deckOutput);
            Debug.Log("Hand: " + handOutput);
            Debug.Log("Field: " + fieldOutput);
            Debug.Log("-------------------");

            i++;
        }
    }

    public struct TableState
    {
        public TableSide PlayerSide;
        public TableSide EnemySide;

        public List<Card> DiscardPile;
    }



    public struct TableSide
    {
        public List<Card> Hand;
        public List<Card> Field;
        public List<Card> Deck;
    }

}
