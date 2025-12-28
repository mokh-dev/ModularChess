using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class QueenController : MonoBehaviour, IMovement, IAttack
{
    public int MovementRange;
    public int AttackRange;

    [HideInInspector] public List<Vector2> Directions = new List<Vector2>{Vector2.right, Vector2.left, Vector2.up, Vector2.down,
                                                    new Vector2(1,1), new Vector2(-1,1), new Vector2(1,-1), new Vector2(-1,-1)};


    [SerializeField] private Pieces _pieceType = Pieces.Queen;
    [SerializeField] private float _baseValue = 9;


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

        return pieceController.FindLaneMovementsInDirections(Directions, currentPos, MovementRange);
    }

    public List<Vector2> FindAttacks()
    {
        Vector2 currentPos = (Vector2)transform.position;

        return pieceController.FindLaneAttacksInDirections(Directions, currentPos, AttackRange);
    }
}

