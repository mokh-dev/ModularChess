using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct Piece
{

    
    public PieceTypes PieceType;

    public Players PieceTeam; // change to just team
    public PieceMoveLogic logic;
    public float PieceBaseValue;
    public float PieceOverallValue;

    public List<Vector2> Movements;
    public List<Vector2> Attacks;

    public Dictionary<int, Vector2> PreviousPiecePositions; 
    
    //TODO change to list of previous positions
    // or some list of previous piece structs
    public Vector2 PiecePosition;

    public int TurnCount;

    public List<Vector2> GetMovements()
    {
        if ((Movements == null) || (Movements.Count == 0))
        {
            logic.LogicPiece = this;
            Movements = logic.FindMovements();
        }
        return Movements;
    }

    public List<Vector2> GetAttacks()
    {
        if ((Attacks == null) || (Attacks.Count == 0))
        {
            logic.LogicPiece = this;
            Attacks = logic.FindAttacks();
        }
        return Attacks;
    }
}
