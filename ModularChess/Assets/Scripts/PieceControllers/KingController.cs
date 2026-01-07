using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class KingController : PieceMoveLogic
{
    public int MovementRange = 1;
    public int AttackRange = 1;



    public override List<Vector2> FindMovements()
    {
        List<Vector2> possibleSquareMovements = FindSquarePositionsAtRange(piece.CurrentPiecePosition, MovementRange);

        return ValidateMovements(possibleSquareMovements);
    }

    public override List<Vector2> FindAttacks()
    {
        List<Vector2> possibleSquareAttacks = FindSquarePositionsAtRange(piece.CurrentPiecePosition, MovementRange);

        return ValidateAttacks(possibleSquareAttacks);
    }
}