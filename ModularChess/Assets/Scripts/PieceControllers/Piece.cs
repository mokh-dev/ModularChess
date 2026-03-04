using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct Piece
{

    
    public PieceTypes PieceType;

    public Players PieceTeam;
    public PieceMoveLogic Logic;
    public float PieceBaseValue;
    public float PieceOverallValue;

    public List<Vector2> Movements;
    public List<Vector2> Attacks;

    public Dictionary<int, Vector2> PreviousPiecePositions; 
    public Vector2 PiecePosition;

    public int BoardTurnCount;

    public BoardState UsedBoardState => BoardStateManager.Instance.BoardStates[BoardTurnCount];



    public List<Vector2> GetMovements()
    {
        if ((Movements == null) || (Movements.Count == 0))
        {
            Logic.LogicPiece = this;
            Movements = Logic.FindMovements();
        }
        return Movements;
    }

    public List<Vector2> GetAttacks()
    {
        if ((Attacks == null) || (Attacks.Count == 0))
        {
            Logic.LogicPiece = this;
            Attacks = Logic.FindAttacks();
        }
        return Attacks;
    }
}
