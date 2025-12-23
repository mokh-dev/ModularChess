using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class PawnController : MonoBehaviour, IMovement, IAttack
{
    public int MovementStep;
    public int HomeRowStep;

    [SerializeField] private int _homeRow = 1;

    private PieceController pieceController;

    void Awake()
    {
        pieceController = gameObject.GetComponent<PieceController>();
    }


    public List<Vector2> FindMovements()
    {
        Vector2 currentPos = (Vector2)transform.position;
        List<Vector2> possibleMoves = new List<Vector2>();


        Vector2 oneStepPos = new Vector2(currentPos.x, currentPos.y + (MovementStep * pieceController.moveDir));
        if (pieceController.IsEmptyAtPos(oneStepPos)) {possibleMoves.Add(oneStepPos);}

        int currentHomeRow = (pieceController.PieceTeam == Players.White) ? _homeRow : (int)pieceController.boardShape.y - _homeRow;
        if (currentPos.y == currentHomeRow)
        {
            Vector2 homeRowStepPos = new Vector2(currentPos.x, currentPos.y + (HomeRowStep * pieceController.moveDir));
            if (pieceController.IsPathEmpty((Vector2)transform.position, homeRowStepPos)) {possibleMoves.Add(homeRowStepPos);}
        }

        return possibleMoves;
    }

    public List<Vector2> FindAttacks()
    {
        List<Vector2> possibleAttackPositions = new List<Vector2>();

        Vector2 rightPos = new Vector2(transform.position.x+1, transform.position.y + (1*pieceController.moveDir));
        Vector2 leftPos = new Vector2(transform.position.x-1, transform.position.y + (1*pieceController.moveDir));

        possibleAttackPositions.Add(rightPos);
        possibleAttackPositions.Add(leftPos);

        return pieceController.ValidateAttacks(possibleAttackPositions);
    }
}


