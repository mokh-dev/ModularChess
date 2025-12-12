using System.Collections.Generic;

using UnityEngine;


public class PawnMovementPattern : MonoBehaviour, IMovementPattern
{
    public List<Vector2> FindPossibleMoves(Dictionary<Vector2, GameObject> boardGameObjects, GameObject currentPiece)
    {
        List<Vector2> possibleMoves = new List<Vector2>();

        Vector2 oneStep = new Vector2(currentPiece.transform.position.x, currentPiece.transform.position.y+1);

        

        possibleMoves.Add(oneStep);

        // BoardStateManager.Instance


        return possibleMoves;
    }
}


