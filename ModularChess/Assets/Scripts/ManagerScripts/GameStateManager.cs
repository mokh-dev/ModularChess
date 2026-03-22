using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager instance;
    public static GameStateManager Instance { get { return instance; } }

    public List<GameState> GameStates = new List<GameState>();
    public GameState CurrentGameState => GameStates.Last();


    public Players CurrentPlayerTurn;

    [SerializeField] private RawImage _turnIndicatior;

    [SerializeField] private int currentTurnCount; //serialized for viewing not changing 


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
        InitializeGame();
    }

    private void InitializeGame()
    {
        GameState initialGameState = new GameState
        {
            TableGameState = TableStateManager.Instance.InitializeTable(),
            BoardGameState = BoardStateManager.Instance.InitializeBoard(),

            TurnCount = 0
        };

        GameStates.Add(initialGameState);
    }

    //TODO these updates should probably be unity events
    public void UpdateTableGameState(TableState updatedTableState)
    {
        GameState updatedGameState = new GameState
        {
            TableGameState = updatedTableState,
            BoardGameState = CurrentGameState.BoardGameState
        };

        GameStates.Add(updatedGameState);
        TableCardManager.Instance.DisplayTableState(updatedGameState.TableGameState);
    }

    public void UpdateBoardGameState(BoardState updatedBoardState)
    {
        GameState updatedGameState = new GameState
        {
            TableGameState = CurrentGameState.TableGameState,
            BoardGameState = updatedBoardState
        };

        GameStates.Add(updatedGameState);
        BoardPiecesManager.Instance.DisplayBoardState(updatedBoardState);
    }

    public void UpdateGameState(GameState updatedGameState)
    {
        GameStates.Add(updatedGameState);
        BoardPiecesManager.Instance.DisplayBoardState(updatedGameState.BoardGameState);
        TableCardManager.Instance.DisplayTableState(updatedGameState.TableGameState);
    }


    public void EndPlayerTurn() //UI Button
    {
        CurrentPlayerTurn = (CurrentPlayerTurn == Players.White) ? Players.Black : Players.White;
        _turnIndicatior.color = (CurrentPlayerTurn == Players.White) ? Color.white : Color.black;
    }

    
    public void PrintTableStates() //Editor Inspector Button
    {
        int i = 0;
        foreach (GameState gameState in GameStates)
        {
            Debug.Log("Table State Num: " + i);
            Debug.Log("special test Num: " + gameState.BoardGameState.testInt);
            Debug.Log("Player Side:");


            string deckOutput = "";
            gameState.TableGameState.PlayerSide.Deck.ForEach(card => deckOutput += $"{card.Title}, ");

            string handOutput = "";
            gameState.TableGameState.PlayerSide.Hand.ForEach(card => handOutput += $"{card.Title}, ");

            string fieldOutput = "";
            gameState.TableGameState.PlayerSide.Field.ForEach(card => fieldOutput += $"{card.Title}, ");


            Debug.Log("Deck: " + deckOutput);
            Debug.Log("Hand: " + handOutput);
            Debug.Log("Field: " + fieldOutput);
            Debug.Log("-------------------");

            i++;
        }
    }

    public void PrintBoardStates() //Editor Inspector Button
    {
        int i = 0;
        foreach (GameState gameState in GameStates)
        {
            string boardPiecesOutput = "";

            foreach (KeyValuePair<Vector2, Piece> boardPiece in gameState.BoardGameState.BoardPieces)
            {
                boardPiecesOutput += boardPiece.Value.Team.ToString()+ " " + boardPiece.Value.Type + " at: " + boardPiece.Key.ToString();
            }
            Debug.Log("board State Num: " + i);
            Debug.Log("Turn: " + gameState.TurnCount.ToString() + ", ");
            Debug.Log("Board Pieces: " + boardPiecesOutput);
            Debug.Log("-------------------");

            i++;
        }
    }
}

public struct GameState
{
    public TableState TableGameState;
    public BoardState BoardGameState;

    public int TurnCount;
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


public struct BoardState
{
    public Dictionary<Vector2, Piece> BoardPieces;
    public int testInt;
    // a list of positions that store the ground info
    // (to tell that at position [3,4] theres a wall or if its non existant)
}

public enum Players
{
    //TODO change all occurrences to Player and Enemy
    White,
    Black,
}

public enum PieceLogicType
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King,
}