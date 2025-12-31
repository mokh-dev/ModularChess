using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class QueenController : PieceMoveLogic
{
    public int MovementRange = 8;
    public int AttackRange = 8;


    [HideInInspector] public List<Vector2> Directions = new List<Vector2>{Vector2.right, Vector2.left, Vector2.up, Vector2.down,
                                                    new Vector2(1,1), new Vector2(-1,1), new Vector2(1,-1), new Vector2(-1,-1)};



    public override List<Vector2> FindMovements()
    {
        return FindLaneMovementsInDirections(Directions, pieceController.CurrentPiecePosition, MovementRange);
    }

    public override List<Vector2> FindAttacks()
    {
        return FindLaneAttacksInDirections(Directions, pieceController.CurrentPiecePosition, AttackRange);
    }
}

