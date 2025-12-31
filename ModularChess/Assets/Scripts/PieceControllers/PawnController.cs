using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class PawnController : PieceMoveLogic
{
    public int MovementStep = 1;
    public int HomeRowStep = 2;

    public Dictionary<Vector2, Vector2> EnPasantEnemyMovementAttackPositions{get; private set;} = new Dictionary<Vector2, Vector2>();


    private int whiteHomeRow = 1;
    private int blackHomeRow = 6;



    public void ClearEnPasantDict()
    {
        EnPasantEnemyMovementAttackPositions = new Dictionary<Vector2, Vector2>();
    }

    public bool CanBeEnPasanted()
    {
        int homeRow = (pieceController.PieceTeam == Players.White) ? whiteHomeRow : blackHomeRow; 

        if (pieceController.movedLastTurn == false) return false;
        if (pieceController.PreviousPiecePosition.y != homeRow) return false;
        
        Vector2 homeRowStepPos = new Vector2(pieceController.PreviousPiecePosition.x, pieceController.PreviousPiecePosition.y + (HomeRowStep * pieceController.MoveDir));

        if (homeRowStepPos != (Vector2)pieceController.CurrentPiecePosition) return false;

        return true;
    }

    private bool CheckCanEnPasant(Vector2 positionToCheck)
    {
        if (BoardPiecesManager.Instance.BoardPieces.TryGetValue(positionToCheck, out PieceController pieceInPosition) == false) return false;
        if (pieceInPosition.PieceTeam == pieceController.PieceTeam) return false;
        if (pieceInPosition.PieceType != Pieces.Pawn) return false; 
        if (((PawnController)pieceInPosition.logic).CanBeEnPasanted() == false) return false;

        return true;
    }


    public override List<Vector2> FindMovements()
    {
        int homeRow = (pieceController.PieceTeam == Players.White) ? whiteHomeRow : blackHomeRow;
        
        Vector2 currentPos = (Vector2)pieceController.CurrentPiecePosition;
        List<Vector2> possibleMoves = new List<Vector2>();


        Vector2 oneStepPos = new Vector2(currentPos.x, currentPos.y + (MovementStep * pieceController.MoveDir));
        if (IsValidMovement(oneStepPos)) {possibleMoves.Add(oneStepPos);}

        
        if (currentPos.y == homeRow)
        {
            Vector2 homeRowStepPos = new Vector2(currentPos.x, currentPos.y + (HomeRowStep * pieceController.MoveDir));

            if (IsPathEmpty((Vector2)pieceController.CurrentPiecePosition, homeRowStepPos) && IsEmptyAtPos(homeRowStepPos))
            {
                possibleMoves.Add(homeRowStepPos);
            }            
        }

        return possibleMoves;
    }

    public override List<Vector2> FindAttacks()
    {
        Vector2 currentPos = (Vector2)pieceController.CurrentPiecePosition;
        List<Vector2> possibleAttackPositions = new List<Vector2>();

        Vector2 rightPos = new Vector2(currentPos.x+1, currentPos.y + (1*pieceController.MoveDir));
        Vector2 leftPos = new Vector2(currentPos.x-1, currentPos.y + (1*pieceController.MoveDir));

        possibleAttackPositions.Add(rightPos);
        possibleAttackPositions.Add(leftPos);

        Vector2 rightEnPasantPos = new Vector2(currentPos.x+1, currentPos.y);
        Vector2 leftEnPasantPos = new Vector2(currentPos.x-1, currentPos.y);

        List<Vector2> validNormalAttacks = ValidateAttacks(possibleAttackPositions);

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


