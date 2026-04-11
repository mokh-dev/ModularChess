using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BoardStateManager : Singleton<BoardStateManager>
{
    private BoardState currentBoardState => GameStateManager.Instance.GameStates.Last().BoardGameState;
    private GameState currentGameState => GameStateManager.Instance.GameStates.Last();


    private void OnEnable() 
    {
        ActionSystem.AttachPerformer<MovePieceGA>(BoardMovePerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<MovePieceGA>();
    }


    public BoardState InitializeBoard()
    {
        return new BoardState
        {
            BoardPieces = new Dictionary<Vector2, Piece>(),
        };
    }


    private void UpdateBoardState(BoardState updatedBoardState)
    {
        GameStateManager.Instance.UpdateBoardGameState(updatedBoardState);
    }


    public void TryPlayBoardMove((Vector2, Vector2) boardMove)
    {
        if (IsValidBoardMove(boardMove) == false) return;

        if (ActionSystem.Instance.IsPerforming) return;
        MovePieceGA movePieceGA = new()
        {
            BoardMove = boardMove,
            MovingPiece = currentBoardState.BoardPieces[boardMove.Item1]
        };

        ActionSystem.Instance.Perform(movePieceGA);
    }

    private IEnumerator BoardMovePerformer(MovePieceGA movePieceGA)
    {
        BoardState movedBoardState = GetMovedBoardState(currentBoardState, movePieceGA.BoardMove);
        UpdateBoardState(movedBoardState);
        yield return null;
    }


    private BoardState GetMovedBoardState(BoardState initialBoardState, (Vector2, Vector2) boardMove)
    {
        Vector2 initialPosition = boardMove.Item1;
        Vector2 endPostion = boardMove.Item2;

        BoardState updatedBoardState = new BoardState
        {
            BoardPieces = new Dictionary<Vector2, Piece>()
        };

        foreach (KeyValuePair<Vector2, Piece> boardPiece in initialBoardState.BoardPieces)
        {   
            Vector2 updatedPosition = boardPiece.Key;
            Piece updatedBoardPiece = boardPiece.Value;

            if (boardPiece.Key == initialPosition)
            {
                updatedBoardState.BoardPieces.Add(endPostion, updatedBoardPiece);
                continue;
            }

            updatedBoardState.BoardPieces.Add(updatedPosition, updatedBoardPiece);
        }

        return updatedBoardState;
    }

    public BoardState GetBoardWithAddedPiece(Vector2 piecePosition, Piece pieceToAdd)
    {
        //TODO add validity Checks
        Dictionary<Vector2, Piece> updatedBoardPieces = new Dictionary<Vector2, Piece>(currentBoardState.BoardPieces);

        updatedBoardPieces.Add(piecePosition, pieceToAdd);

        return new BoardState
        {
            BoardPieces = updatedBoardPieces
        };
    }

    public bool IsValidBoardAttack((Vector2, Vector2) boardAttack)
    {
        if (currentBoardState.BoardPieces.TryGetValue(boardAttack.Item1, out Piece attackingPiece) == false) return false;
        if (PieceLogicManager.FindAttacks(boardAttack.Item1, attackingPiece, currentGameState).Contains(boardAttack.Item2) == false) return false;

        return true;
    }

    public bool IsValidBoardMove((Vector2, Vector2) boardMove)
    {
        if (currentBoardState.BoardPieces.TryGetValue(boardMove.Item1, out Piece movingPiece) == false) return false;
        if (PieceLogicManager.FindMovements(boardMove.Item1, movingPiece, currentGameState).Contains(boardMove.Item2) == false) return false;

        return true;
    }
}



