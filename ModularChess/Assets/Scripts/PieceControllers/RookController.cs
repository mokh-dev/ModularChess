using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class RookController : PieceMoveLogic
{
    public int MovementRange = 8;
    public int AttackRange = 8;

    public List<Vector2> Directions = new List<Vector2>{Vector2.right, Vector2.left, Vector2.up, Vector2.down};


    public override List<Vector2> FindMovements()
    {
        return FindLaneMovementsInDirections(Directions, piece.CurrentPiecePosition, MovementRange);
    }

    public override List<Vector2> FindAttacks()
    {
        return FindLaneAttacksInDirections(Directions, piece.CurrentPiecePosition, AttackRange);
    }
}



