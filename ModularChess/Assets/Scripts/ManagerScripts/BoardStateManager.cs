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
        // _inBetweenBoardState = initialBoardState;
    }

    private BoardState InitializeBoard(BoardState boardState)
    {
        foreach (KeyValuePair<Vector2, PieceController> pieceControllerInPos in BoardPiecesManager.Instance.BoardPieceObjects)
        {
            boardState.BoardPieces.Add(pieceControllerInPos.Key, pieceControllerInPos.Value.GetInitialPiece());
        }
        return boardState;
    }
    

    private void BoardUpdated()
    {
        // WhiteInCheck = BoardPiecesManager.Instance.IsInCheck(Players.White);
        // BlackInCheck = BoardPiecesManager.Instance.IsInCheck(Players.Black);
    }


    //TODO check for king checks and add to board state
    public void PlayMove(PieceController pieceControllerToMove, Move move)
    {       
        switch (move)
        {
            case BoardMove boardMove:
                BoardStates.Add(MoveBoardPiece(CurrentBoardState, boardMove, false));
                BoardPiecesManager.Instance.MoveBoardPieceObj(pieceControllerToMove, boardMove);
                break;
            
            case CardMove cardMove:
                break;

            default:
                break;
        }

        //EndPlayerTurn();
    }

    public BoardState MoveBoardPiece(BoardState initialBoardState, BoardMove move, bool isSimulated)
    {
        Vector2 initialPostion = move.PieceMove.Item1;
        Vector2 endPostion = move.PieceMove.Item2;

        Piece movingPiece = initialBoardState.BoardPieces[initialPostion];

        Vector2 attackedPosition = new Vector2(-1,-1); // -1,-1 means no attack

        if (IsValidBoardAttack(initialBoardState, move) == true) 
        {
            bool isNonLandingAttack = CheckForNonLandingAttack(movingPiece, endPostion, out Vector2 attackPosition);
            attackedPosition = isNonLandingAttack ? attackPosition : endPostion;

            if (isSimulated == false) BoardPiecesManager.Instance.DestroyPieceObjAtPos(attackedPosition);
        }

        BoardState updatedBoardState = GenerateNewMovedBoardState(initialBoardState, move, attackedPosition);

        return updatedBoardState;
    }

    private BoardState GenerateNewMovedBoardState(BoardState initialBoardState, BoardMove move, Vector2 attackedPosition)
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

            updatedBoardPiece.PreviousPiecePositions.Add(updatedBoardState.TurnCount - 1, boardPiece.Key);

            if (boardPiece.Key == initialPosition)
            {
                updatedBoardPiece.PiecePosition = endPostion;
                updatedPosition = endPostion;
            }
            
            updatedBoardPiece.Attacks = null;
            updatedBoardPiece.Movements = null;

            updatedBoardPiece.TurnCount = updatedBoardState.TurnCount;

            updatedBoardPiece.logic.LogicPiece = updatedBoardPiece;

            updatedBoardState.BoardPieces.Add(updatedPosition, updatedBoardPiece);
        }

        return updatedBoardState;
    }

    private bool CheckForNonLandingAttack(Piece pieceToCheck, Vector2 endPos, out Vector2 attackPosition)
    {
        attackPosition = default;

        if (BoardPiecesManager.Instance.CheckForPawnEnPasant(pieceToCheck, endPos, out Vector2 pawnAttackPosition) == true)
        {
            attackPosition = pawnAttackPosition;
            return true;
        }

        return false;
    }



    public bool IsValidBoardMove(BoardState boardState, BoardMove boardMove)
    {
        if (boardState.BoardPieces.TryGetValue(boardMove.PieceMove.Item1, out Piece movingPiece) == false) return false;


        bool validAttack = movingPiece.GetAttacks().Contains(boardMove.PieceMove.Item2);
        bool validMove = movingPiece.GetMovements().Contains(boardMove.PieceMove.Item2);

        if ((validAttack || validMove) == false) return false;

        return true;
    }

    public bool IsValidBoardAttack(BoardState boardState, BoardMove boardMove)
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
   

    public bool SimulateBoardMoves(BoardState initialBoardState, List<Move> movesToSimulate, out BoardState finalBoardState)
    {
        finalBoardState = new BoardState();
        BoardState simulatedBoardState = initialBoardState;


        foreach (Move move in movesToSimulate)
        {
            switch (move)
            {
                case BoardMove boardMove:
                    if (IsValidBoardMove(simulatedBoardState, boardMove) == false) return false;

                    simulatedBoardState = PlaySimulatedBoardMove(simulatedBoardState, boardMove.PieceMove);
                    break;
                
                case CardMove cardMove:
                    //no cards yet
                    break;
                
                default:
                    continue;
            }
        }

        finalBoardState = simulatedBoardState;
        return true;
    }


    private BoardState PlaySimulatedBoardMove(BoardState boardState, (Vector2, Vector2) boardMove)
    {
        boardState.BoardPieces.TryGetValue(boardMove.Item1, out Piece movingPiece);

        if (boardState.BoardPieces.TryGetValue(boardMove.Item2, out Piece attackedPiece) == true)
        {
            boardState.BoardPieces.Remove(boardMove.Item2);
        }   

        Piece updatedPiece = ReInitializeMovedPiece(movingPiece, boardMove.Item1, boardMove.Item2);

        boardState.BoardPieces.Add(boardMove.Item2, updatedPiece);
        boardState.BoardPieces.Remove(boardMove.Item1);

        boardState.PlayerTurn = (boardState.PlayerTurn == Players.White) ? Players.Black : Players.White;
        boardState.TurnCount++;

        return boardState;     
    }

    private Piece ReInitializeMovedPiece(Piece initialPiece, Vector2 previousPos, Vector2 currentPos)
    {
        initialPiece.Attacks = null;
        initialPiece.Movements = null;

        //initialPiece.PreviousPiecePosition = previousPos;
        initialPiece.PiecePosition = currentPos;

        return initialPiece;
    }

    public void RebuildBoardState(int boardStateIndex)
    {
        //TODO rebuild board state from index
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

