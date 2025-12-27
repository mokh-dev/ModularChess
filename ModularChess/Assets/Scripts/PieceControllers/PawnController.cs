using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class PawnController : MonoBehaviour, IMovement, IAttack
{
    public int MovementStep;
    public int HomeRowStep;

    public Dictionary<Vector2, Vector2> EnPasantEnemyMovementAttackPositions{get; private set;} = new Dictionary<Vector2, Vector2>();

    [SerializeField] private int _homeRow = 1;

    private PieceController pieceController;

    void Awake()
    {
        pieceController = gameObject.GetComponent<PieceController>();

        _homeRow = (pieceController.PieceTeam == Players.White) ? _homeRow : (int)pieceController.boardShape.y - _homeRow;
    }

    public void ClearEnPasantDict()
    {
        EnPasantEnemyMovementAttackPositions = new Dictionary<Vector2, Vector2>();
    }

    public bool CanBeEnPasanted()
    {
        if (pieceController.movedLastTurn == false) return false;
        if (pieceController.previousPiecePosition.y != _homeRow) return false;
        
        Vector2 homeRowStepPos = new Vector2(pieceController.previousPiecePosition.x, pieceController.previousPiecePosition.y + (HomeRowStep * pieceController.moveDir));

        if (homeRowStepPos != (Vector2)transform.position) return false;

        return true;
    }

    private bool CheckCanEnPasant(Vector2 positionToCheck)
    {
        if (BoardStateManager.Instance.BoardGameObjects.TryGetValue(positionToCheck, out GameObject pieceInPosition) == false) return false;
        if (pieceInPosition.GetComponent<PieceController>().PieceTeam == pieceController.PieceTeam) return false;
        if (pieceInPosition.TryGetComponent<PawnController>(out PawnController pawnInPosition) == false) return false;
        if (pawnInPosition.CanBeEnPasanted() == false) return false;

        return true;
    }


    public List<Vector2> FindMovements()
    {
        Vector2 currentPos = (Vector2)transform.position;
        List<Vector2> possibleMoves = new List<Vector2>();


        Vector2 oneStepPos = new Vector2(currentPos.x, currentPos.y + (MovementStep * pieceController.moveDir));
        if (pieceController.IsValidMovement(oneStepPos)) {possibleMoves.Add(oneStepPos);}

        
        if (currentPos.y == _homeRow)
        {
            Vector2 homeRowStepPos = new Vector2(currentPos.x, currentPos.y + (HomeRowStep * pieceController.moveDir));

            if (pieceController.IsPathEmpty((Vector2)transform.position, homeRowStepPos) && pieceController.IsEmptyAtPos(homeRowStepPos))
            {
                possibleMoves.Add(homeRowStepPos);
            }            
        }

        return possibleMoves;
    }

    public List<Vector2> FindAttacks()
    {
        Vector2 currentPos = (Vector2)transform.position;
        List<Vector2> possibleAttackPositions = new List<Vector2>();

        Vector2 rightPos = new Vector2(currentPos.x+1, currentPos.y + (1*pieceController.moveDir));
        Vector2 leftPos = new Vector2(currentPos.x-1, currentPos.y + (1*pieceController.moveDir));

        possibleAttackPositions.Add(rightPos);
        possibleAttackPositions.Add(leftPos);

        Vector2 rightEnPasantPos = new Vector2(currentPos.x+1, currentPos.y);
        Vector2 leftEnPasantPos = new Vector2(currentPos.x-1, currentPos.y);

        List<Vector2> validNormalAttacks = pieceController.ValidateAttacks(possibleAttackPositions);

        ClearEnPasantDict();

        if (CheckCanEnPasant(rightEnPasantPos) == true)
        {
            EnPasantEnemyMovementAttackPositions.Add(rightPos, rightEnPasantPos); 
            validNormalAttacks.Add(rightPos);
        }
        if (CheckCanEnPasant(leftEnPasantPos) == true)
        {
            EnPasantEnemyMovementAttackPositions.Add(leftPos, leftEnPasantPos); 
            validNormalAttacks.Add(leftPos);
        }

        return validNormalAttacks;
    }
}


