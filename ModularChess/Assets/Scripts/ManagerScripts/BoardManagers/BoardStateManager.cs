using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class BoardStateManager : MonoBehaviour
{
    private static BoardStateManager instance;
    public static BoardStateManager Instance { get { return instance; } }

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



    public BoardState InitializeBoard()
    {
        return new BoardState
        {
            BoardPieces = new Dictionary<Vector2, Piece>()
        };
    }


    private void UpdateBoardState(BoardState updatedBoardState)
    {
        GameStateManager.Instance.UpdateBoardGameState(updatedBoardState);
        BoardPiecesManager.Instance.DisplayBoardState(updatedBoardState);
    }


    public bool TryPlayBoardMove((Vector2, Vector2) boardMove)
    {
        if (IsValidBoardMove(boardMove) == false) return false;

        BoardState movedBoardState = GetMovedBoardState(currentBoardState, boardMove);
        UpdateBoardState(movedBoardState);
        
        return true;
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
                updatedBoardPiece.PiecePosition = endPostion;
                updatedPosition = endPostion;
            }
            
            updatedBoardPiece.Attacks = null;
            updatedBoardPiece.Movements = null;

            updatedBoardPiece.PreviousPiecePositions.Add(boardPiece.Key);


            updatedBoardPiece.Logic.LogicPiece = updatedBoardPiece;

            updatedBoardState.BoardPieces.Add(updatedPosition, updatedBoardPiece);
        }

        return updatedBoardState;
    }

    public bool IsValidBoardAttack((Vector2, Vector2) boardAttack)
    {
        if (currentBoardState.BoardPieces.TryGetValue(boardAttack.Item1, out Piece attackingPiece) == false) return false;
        if (attackingPiece.GetAttacks().Contains(boardAttack.Item2) == false) return false;

        return true;
    }

    public bool IsValidBoardMove((Vector2, Vector2) boardMove)
    {
        if (currentBoardState.BoardPieces.TryGetValue(boardMove.Item1, out Piece movingPiece) == false) return false;
        if (movingPiece.GetMovements().Contains(boardMove.Item2) == false) return false;

        return true;
    }
}



