using System.Collections.Generic;

using UnityEngine;


public class PawnMovementPattern : MovementPattern
{
    public override List<Vector2> FindPossibleMoves(int boardSize, Vector2 currentPos, int team)
    {
        List<Vector2> possibleMoves = new List<Vector2>();

        Vector2 oneStep = new Vector2(currentPos.x, currentPos.y+1);
        possibleMoves.Add(oneStep);

        // BoardStateManager.Instance


        return possibleMoves;
    }
}


