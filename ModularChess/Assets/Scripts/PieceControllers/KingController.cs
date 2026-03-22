using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceBuilder))]
public class KingController : PieceMoveLogic
{
    public int MovementRange = 1;
    public int AttackRange = 1;



    public override List<Vector2> FindMovements(Vector2 piecePosition, Piece logicPiece, GameState logicGameState)
    {
        List<Vector2> possibleSquareMovements = FindSquarePositionsAtRange(piecePosition, MovementRange);

        return ValidateMovements(possibleSquareMovements, logicGameState);
    }

    public override List<Vector2> FindAttacks(Vector2 piecePosition, Piece logicPiece, GameState logicGameState)
    {
        List<Vector2> possibleSquareAttacks = FindSquarePositionsAtRange(piecePosition, MovementRange);

        return ValidateAttacks(possibleSquareAttacks, logicPiece, logicGameState);
    }
}