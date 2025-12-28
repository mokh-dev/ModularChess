using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class BishopController : MonoBehaviour, IMovement, IAttack
{
    public int MovementRange;
    public int AttackRange;

    [HideInInspector] public List<Vector2> Directions = new List<Vector2>{new Vector2(1,1), new Vector2(-1,1), new Vector2(1,-1), new Vector2(-1,-1)};


    [SerializeField] private Pieces _pieceType = Pieces.Bishop;
    [SerializeField] private float _baseValue = 3;


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



