using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class RookController : MonoBehaviour, IMovement, IAttack
{
   private PieceController pieceController;

    void Awake()
    {
        pieceController = gameObject.GetComponent<PieceController>();
    }


    public List<Vector2> FindMovements()
    {
        Vector2 currentPos = (Vector2)transform.position;
        List<Vector2> possibleMoves = new List<Vector2>();

        List<Vector2> rightLane = pieceController.FindSlidingMovements(currentPos, Vector2.right);
        List<Vector2> leftLane = pieceController.FindSlidingMovements(currentPos, Vector2.left);
        List<Vector2> upLane = pieceController.FindSlidingMovements(currentPos, Vector2.up);
        List<Vector2> downLane = pieceController.FindSlidingMovements(currentPos, Vector2.down);

        possibleMoves.AddRange(rightLane);
        possibleMoves.AddRange(leftLane);
        possibleMoves.AddRange(upLane);
        possibleMoves.AddRange(downLane);

        return possibleMoves;
    }

    public List<Vector2> FindAttacks()
    {
        Vector2 currentPos = (Vector2)transform.position;
        List<Vector2> validAttacks = new List<Vector2>();

        Vector2[] directions = {Vector2.right, Vector2.left, Vector2.up, Vector2.down};


        for (int i = 0; i < directions.Length; i++)
        {
            if (pieceController.TryFindSlidingAttack(out Vector2 possibleAttack, currentPos, directions[i]) == false) continue;
            if (pieceController.IsInBounds(possibleAttack) == false) continue;
            validAttacks.Add(possibleAttack);
        }

        return validAttacks;
    }
}



