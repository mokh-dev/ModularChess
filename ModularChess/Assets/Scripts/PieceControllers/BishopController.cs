using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceBuilder))]
public class BishopController : PieceMoveLogic
{
    public int MovementRange = 8;
    public int AttackRange = 8;

    public List<Vector2> Directions = new List<Vector2>{new Vector2(1,1), new Vector2(-1,1), new Vector2(1,-1), new Vector2(-1,-1)};



    public override List<Vector2> FindMovements(Vector2 piecePosition, Piece logicPiece, GameState logicGameState)
    {
        return FindLaneMovementsInDirections(Directions, piecePosition, logicGameState, MovementRange);
    }

    public override List<Vector2> FindAttacks(Vector2 piecePosition, Piece logicPiece, GameState logicGameState)
    {
        return FindLaneAttacksInDirections(Directions, piecePosition, logicPiece, logicGameState, AttackRange);
    }
}



