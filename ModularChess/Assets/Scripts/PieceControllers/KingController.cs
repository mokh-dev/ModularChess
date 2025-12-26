using System.Collections.Generic;
using Unity.VisualScripting;
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

    private Vector2 FindCornerAtRange(Vector2 currentPos, int distanceToCorner, Vector2 direction)
    {
        return currentPos + (direction * distanceToCorner);
    }

    private List<Vector2> FindCornersAtRange(Vector2 currentPos, int distanceToCorner)
    {
        List<Vector2> cornersAtRange = new List<Vector2>();
        Vector2[] directions = {new Vector2(1,1), new Vector2(-1,1), new Vector2(1,-1), new Vector2(-1,-1)};


        for (int i = 1; i <= distanceToCorner; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector2 cornerPos = FindCornerAtRange(currentPos, i, directions[j]);
                if (pieceController.IsValidMovement(cornerPos) == false) continue;

                cornersAtRange.Add(cornerPos);
            }
        }


        return cornersAtRange;
    }

    public List<Vector2> FindMovements()
    {
        Vector2 currentPos = (Vector2)transform.position;



        return FindCornersAtRange(currentPos, MovementRange);
    }

    public List<Vector2> FindAttacks()
    {
        Vector2 currentPos = (Vector2)transform.position;



        return new List<Vector2>();
    }
}