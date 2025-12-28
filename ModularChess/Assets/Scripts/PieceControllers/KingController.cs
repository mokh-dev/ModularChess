using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class KingController : MonoBehaviour, IMovement, IAttack
{
    public int MovementRange;
    public int AttackRange;

    [SerializeField] private Pieces _pieceType = Pieces.Rook;
    [SerializeField] private float _baseValue = Mathf.Infinity;


    private PieceController pieceController;


    void Awake()
    {
        pieceController = gameObject.GetComponent<PieceController>();
        pieceController.PieceType = _pieceType;
        pieceController.PieceBaseValue = _baseValue;
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