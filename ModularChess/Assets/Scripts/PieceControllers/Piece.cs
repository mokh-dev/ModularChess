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

    public List<Vector2> CurrentMovements;
    public List<Vector2> CurrentAttacks;

    public Vector2 PreviousPiecePosition; 
    
    //TODO change to list of previous positions
    // or some list of previous piece structs
    public Vector2 CurrentPiecePosition;

    public int CurrentTurnCount;

    public List<Vector2> GetCurrentMovements()
    {
        if ((CurrentMovements == null) || (CurrentMovements.Count == 0))
        {
            logic.piece = this;
            CurrentMovements = logic.FindMovements();
        }
        return CurrentMovements;
    }

    public List<Vector2> GetCurrentAttacks()
    {
        if ((CurrentAttacks == null) || (CurrentAttacks.Count == 0))
        {
            logic.piece = this;
            CurrentAttacks = logic.FindAttacks();
        }
        return CurrentAttacks;
    }
}
