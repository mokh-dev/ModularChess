using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class BishopController : PieceMoveLogic
{
    public int MovementRange = 8;
    public int AttackRange = 8;

    public List<Vector2> Directions = new List<Vector2>{new Vector2(1,1), new Vector2(-1,1), new Vector2(1,-1), new Vector2(-1,-1)};



    public override List<Vector2> FindMovements()
    {
        return FindLaneMovementsInDirections(Directions, pieceController.CurrentPiecePosition, MovementRange);
    }

    public override List<Vector2> FindAttacks()
    {
        return FindLaneAttacksInDirections(Directions, pieceController.CurrentPiecePosition, AttackRange);
    }
}



