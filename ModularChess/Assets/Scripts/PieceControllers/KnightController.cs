using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class KnightController : MonoBehaviour, IMovement, IAttack
{
   private PieceController pieceController;
   

    [SerializeField] private Pieces _pieceType = Pieces.Bishop;
    [SerializeField] private float _baseValue = 3;

    void Awake()
    {
        pieceController = gameObject.GetComponent<PieceController>();
        pieceController.PieceType = _pieceType;
        pieceController.PieceBaseValue = _baseValue;
    }

    private List<Vector2> FindCornersAtRange(Vector2 currentPos, int distanceToCorner)
    {
        List<Vector2> cornersAtRange = new List<Vector2>();
        Vector2[] directions = {new Vector2(1,1), new Vector2(-1,1), new Vector2(-1,-1), new Vector2(1,-1)};


        for (int i = 0; i < 4; i++)
        {
            Vector2 cornerPos = currentPos + (directions[i] * distanceToCorner);

            cornersAtRange.Add(cornerPos);
        }
        
        return cornersAtRange;
    }

    private List<Vector2> KnightMovePositions(Vector2 currentPos)
    {
        List<Vector2> possiblePositions = new List<Vector2>();

        List<Vector2> corners = FindCornersAtRange(currentPos, 2);

        (Vector2, Vector2)[] offsets = {(Vector2.left, Vector2.down), 
                                        (Vector2.right, Vector2.down),
                                        (Vector2.right, Vector2.up),
                                        (Vector2.left, Vector2.up)};

        for (int i = 0; i < corners.Count; i++)
        {
            Vector2 firstOffsetPosition = corners[i] + offsets[i].Item1;
            Vector2 secondOffsetPosition = corners[i] + offsets[i].Item2;

            possiblePositions.Add(firstOffsetPosition);
            possiblePositions.Add(secondOffsetPosition);
        }

        return possiblePositions;
    }

    public List<Vector2> FindMovements()
    {
        Vector2 currentPos = (Vector2)transform.position;

        List<Vector2> possibleMovements = KnightMovePositions(currentPos);

        return pieceController.ValidateMovements(possibleMovements);
    }

    public List<Vector2> FindAttacks()
    {
        Vector2 currentPos = (Vector2)transform.position;

        List<Vector2> possibleAttacks = KnightMovePositions(currentPos);

        return pieceController.ValidateAttacks(possibleAttacks);
    }
}