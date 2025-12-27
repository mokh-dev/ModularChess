using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class KingController : MonoBehaviour, IMovement, IAttack
{
   private PieceController pieceController;
   public int MovementRange;
   public int AttackRange;


    void Awake()
    {
        pieceController = gameObject.GetComponent<PieceController>();
    }

    public List<Vector2> FindMovements()
    {
        Vector2 currentPos = (Vector2)transform.position;

        List<Vector2> possibleSquareMovements = pieceController.FindSquarePositionsAtRange(currentPos, MovementRange);

        return pieceController.ValidateMovements(possibleSquareMovements);
    }

    public List<Vector2> FindAttacks()
    {
        Vector2 currentPos = (Vector2)transform.position;

        List<Vector2> possibleSquareAttacks = pieceController.FindSquarePositionsAtRange(currentPos, MovementRange);

        return pieceController.ValidateAttacks(possibleSquareAttacks);
    }
}