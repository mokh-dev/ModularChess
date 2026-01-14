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

    public int TurnCount;

    public BoardState UsedBoardState
    {
        get
        {
            if (IsRealState()) return BoardStateManager.Instance.BoardStates[TurnCount];
            
            if (BoardStateManager.Instance.SimulatedBoardStates.TryGetValue(TurnCount, out BoardState simulatedState) == true) return simulatedState;
            
            throw new Exception("Piece: " + this.ToString() + " tried using non-existant Board State");
        }

    }

    private bool IsRealState()
    {
        return (TurnCount >= 0) && (TurnCount < BoardStateManager.Instance.BoardStates.Count);
    }

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
