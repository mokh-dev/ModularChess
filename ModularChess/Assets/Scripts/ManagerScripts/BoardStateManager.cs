using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;


public class BoardStateManager : MonoBehaviour
{
    private static BoardStateManager instance;

    public static BoardStateManager Instance { get { return instance; } }

        
    public List<BoardState> BoardStates = new List<BoardState>();
    public BoardState CurrentBoardState => BoardStates.Last();

    public Dictionary<int, BoardState> SimulatedBoardStates = new Dictionary<int, BoardState>();


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
        StartGame();
    }

    private void StartGame()
    {
        BoardState initialBoardState = new BoardState();
        initialBoardState.BoardPieces = new Dictionary<Vector2, Piece>();
        initialBoardState = InitializeBoard(initialBoardState);

        initialBoardState.PlayerTurn = Players.White;
        initialBoardState.TurnCount = 0;

        BoardStates.Add(initialBoardState);
    }

    private BoardState InitializeBoard(BoardState boardState)
    {
        foreach (KeyValuePair<Vector2, PieceController> pieceControllerInPos in BoardPiecesManager.Instance.BoardPieceObjects)
        {
            boardState.BoardPieces.Add(pieceControllerInPos.Key, pieceControllerInPos.Value.GetInitialPiece());
        }
        return boardState;
    }
    

    private void PlayBoardMove(BoardState playedBoardState, BoardMove boardMove, Vector2? attackedPosition)
    {
        BoardStates.Add(playedBoardState);

        BoardPiecesManager.Instance.MoveBoardPieceObj(boardMove);
        if (attackedPosition != null)
        {
            BoardPiecesManager.Instance.DestroyPieceObjAtPos((Vector2)attackedPosition);
        }
    }

    private BoardState GetMovedBoardState(BoardState initialBoardState, BoardMove move, out Vector2? attackedPosition)
    {
        Vector2 initialPostion = move.PieceMove.Item1;
        Vector2 endPostion = move.PieceMove.Item2;

        Piece movingPiece = initialBoardState.BoardPieces[initialPostion];

        attackedPosition = null;

        if (IsValidBoardAttack(initialBoardState, move) == true) 
        {
            bool isNonLandingAttack = CheckForNonLandingAttack(movingPiece, endPostion, out Vector2 attackPosition);
            attackedPosition = isNonLandingAttack ? attackPosition : endPostion;
        }

        BoardState updatedBoardState = GenerateNewMovedBoardState(initialBoardState, move, attackedPosition);

        return updatedBoardState;
    }

    private BoardState GenerateNewMovedBoardState(BoardState initialBoardState, BoardMove move, Vector2? attackedPosition)
    {
        Vector2 initialPosition = move.PieceMove.Item1;
        Vector2 endPostion = move.PieceMove.Item2;

        BoardState updatedBoardState = new BoardState();
        
        updatedBoardState.TurnCount = initialBoardState.TurnCount + 1;
        updatedBoardState.PlayerTurn = (initialBoardState.PlayerTurn == Players.White) ? Players.Black : Players.White;
        updatedBoardState.BoardPieces = new Dictionary<Vector2, Piece>();

        foreach (KeyValuePair<Vector2, Piece> boardPiece in initialBoardState.BoardPieces)
        {   
            if (boardPiece.Key == attackedPosition) continue;

            Vector2 updatedPosition = boardPiece.Key;
            Piece updatedBoardPiece = boardPiece.Value;

            updatedBoardPiece.PreviousPiecePositions = new Dictionary<int, Vector2>();
            updatedBoardPiece.PreviousPiecePositions.Add(updatedBoardState.TurnCount - 1, boardPiece.Key);

            if (boardPiece.Key == initialPosition)
            {
                updatedBoardPiece.PiecePosition = endPostion;
                updatedPosition = endPostion;
            }
            
            updatedBoardPiece.Attacks = null;
            updatedBoardPiece.Movements = null;

            updatedBoardPiece.TurnCount = updatedBoardState.TurnCount;

            updatedBoardPiece.Logic.LogicPiece = updatedBoardPiece;

            updatedBoardState.BoardPieces.Add(updatedPosition, updatedBoardPiece);
        }

        return updatedBoardState;
    }

    public bool TryPlayBoardMove(BoardState initialBoardState, BoardMove boardMove)
    {
        if (IsValidBoardMove(initialBoardState, boardMove, out BoardState simulatedBoardState, out Vector2? attackedPosition) == false) return false;
        PlayBoardMove(simulatedBoardState, boardMove, attackedPosition);
        
        return true;
    }

    public bool SimulatedMoveHasCheck(BoardState initialBoardState, BoardMove boardMove, out BoardState simulatedBoardState, out Vector2? attackedPosition)
    {
        BoardState movedBoardState = GetMovedBoardState(initialBoardState, boardMove, out attackedPosition);
        SimulatedBoardStates[movedBoardState.TurnCount] = movedBoardState;

        bool initialPlayerInCheck = BoardPiecesManager.Instance.IsInCheck(movedBoardState, initialBoardState.PlayerTurn);
        bool oppositePlayerInCheck = BoardPiecesManager.Instance.IsInCheck(movedBoardState, movedBoardState.PlayerTurn);



        if (initialPlayerInCheck == true)
        {
            //illegal move
            attackedPosition = null;
            simulatedBoardState = default;
            return false;
        }

        if (oppositePlayerInCheck == true)
        {
            if (movedBoardState.PlayerTurn == Players.White)
            {
                movedBoardState.WhiteInCheck = true;
            }
            else if (movedBoardState.PlayerTurn == Players.Black)
            {
                movedBoardState.BlackInCheck = true;
            }
        }

        simulatedBoardState = movedBoardState;
        return true;
    }

    
    private bool CheckForNonLandingAttack(Piece pieceToCheck, Vector2 endPos, out Vector2 attackPosition)
    {
        if (BoardPiecesManager.Instance.CheckForPawnEnPasant(pieceToCheck, endPos, out attackPosition) == true)
        {
            return true;
        }

        attackPosition = default;
        return false;
    }

    public bool IsValidBoardMove(BoardState initialBoardState, BoardMove boardMove, out BoardState simulatedBoardState, out Vector2? attackedPosition)
    {
        attackedPosition = null;
        simulatedBoardState = default;

        if (initialBoardState.BoardPieces.TryGetValue(boardMove.PieceMove.Item1, out Piece movingPiece) == false) return false;

        bool validAttack = movingPiece.GetAttacks().Contains(boardMove.PieceMove.Item2);
        bool validMove = movingPiece.GetMovements().Contains(boardMove.PieceMove.Item2);

        if ((validAttack || validMove) == false) return false;


        if (SimulatedMoveHasCheck(initialBoardState, boardMove, out simulatedBoardState, out attackedPosition) == false) return false;

        return true;
    }

    private bool IsValidBoardAttack(BoardState boardState, BoardMove boardMove)
    {
        if (boardState.BoardPieces.TryGetValue(boardMove.PieceMove.Item1, out Piece attackingPiece) == false) return false;
        if (attackingPiece.GetAttacks().Contains(boardMove.PieceMove.Item2) == false) return false;

        return true;
    }

    public bool IsValidBoardMovement(BoardState boardState, BoardMove boardMove)
    {
        if (boardState.BoardPieces.TryGetValue(boardMove.PieceMove.Item1, out Piece movingPiece) == false) return false;
        if (movingPiece.GetMovements().Contains(boardMove.PieceMove.Item2) == false) return false;

        return true;
    }

    public void PrintBoardStates()
    {
        int i = 0;
        foreach (BoardState state in BoardStates)
        {
            string boardPiecesOutput = "";

            foreach (KeyValuePair<Vector2, Piece> boardPiece in state.BoardPieces)
            {
                boardPiecesOutput += boardPiece.Value.PieceTeam.ToString()+ " " + boardPiece.Value.PieceType + " at: " + boardPiece.Value.PiecePosition.ToString();
            }
            Debug.Log("board State Num: " + i);
            Debug.Log("Turn: " + state.TurnCount.ToString() + ", ");
            Debug.Log("White In Check: " + state.WhiteInCheck.ToString());
            Debug.Log("Black In Check: " + state.BlackInCheck.ToString());
            Debug.Log("Board Pieces: " + boardPiecesOutput);
            Debug.Log("-------------------");

            i++;
        }
    }

}

public struct BoardState
{
    public Dictionary<Vector2, Piece> BoardPieces;
    // cards or effects in play
    public Players PlayerTurn;
    public int TurnCount;


    public bool WhiteInCheck;
    public bool BlackInCheck;
}

public enum Players
{
    White,
    Black,
}

public enum PieceTypes
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King,
}


public abstract class Move{} 

public class BoardMove : Move
{
    public (Vector2, Vector2) PieceMove;
}

public class CardMove : Move
{
    public int CardIdToPlay;
}

