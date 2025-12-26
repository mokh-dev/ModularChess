using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class BishopController : MonoBehaviour, IMovement, IAttack
{
   private PieceController pieceController;
   public List<Vector2> Directions = new List<Vector2>{new Vector2(1,1), new Vector2(-1,1), new Vector2(1,-1), new Vector2(-1,-1)};
   public int MovementRange;
   public int AttackRange;


    void Awake()
    {
        pieceController = gameObject.GetComponent<PieceController>();
    }


    public List<Vector2> FindMovements()
    {
        Vector2 currentPos = (Vector2)transform.position;

        return pieceController.FindLaneMovementsInDirections(Directions, currentPos, MovementRange);
    }

    public List<Vector2> FindAttacks()
    {
        Vector2 currentPos = (Vector2)transform.position;

        return pieceController.FindLaneAttacksInDirections(Directions, currentPos, AttackRange);
    }
}



