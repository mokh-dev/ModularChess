using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class PawnController : PieceMoveLogic
{
    public int MovementStep = 1;
    public int AttackStep = 1;
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
        int homeRow = (piece.PieceTeam == Players.White) ? whiteHomeRow : blackHomeRow; 
        int moveDir = (piece.PieceTeam == Players.White) ? 1 : -1; 

        //FIXME en pasant
        // if (piece.movedLastTurn == false) return false;
        if (piece.PreviousPiecePosition.y != homeRow) return false;
        
        Vector2 homeRowStepPos = new Vector2(piece.PreviousPiecePosition.x, piece.PreviousPiecePosition.y + (HomeRowStep * moveDir));

        if (homeRowStepPos != piece.CurrentPiecePosition) return false;

        return true;
    }

    private bool CheckCanEnPasant(Vector2 positionToCheck)
    {
        if (currentBoardState.BoardPieces.TryGetValue(positionToCheck, out Piece pieceInPosition) == false) return false;
        if (pieceInPosition.PieceTeam == piece.PieceTeam) return false;
        if (pieceInPosition.PieceType != PieceTypes.Pawn) return false; 
        if (((PawnController)pieceInPosition.logic).CanBeEnPasanted() == false) return false;

        return true;
    }


    public override List<Vector2> FindMovements()
    {
        int homeRow = (piece.PieceTeam == Players.White) ? whiteHomeRow : blackHomeRow;
        int moveDir = (piece.PieceTeam == Players.White) ? 1 : -1; 
        
        Vector2 currentPos = piece.CurrentPiecePosition;
        List<Vector2> possibleMoves = new List<Vector2>();


        Vector2 oneStepPos = new Vector2(currentPos.x, currentPos.y + (MovementStep * moveDir));
        if (IsValidMovement(oneStepPos)) {possibleMoves.Add(oneStepPos);}


        if (currentPos.y == homeRow)
        {
            Vector2 homeRowStepPos = new Vector2(currentPos.x, currentPos.y + (HomeRowStep * moveDir));

            if (IsPathEmpty(piece.CurrentPiecePosition, homeRowStepPos) && IsEmptyAtPos(homeRowStepPos))
            {
                possibleMoves.Add(homeRowStepPos);
            }            
        }

        return possibleMoves;
    }

    public override List<Vector2> FindAttacks()
    {
        int moveDir = (piece.PieceTeam == Players.White) ? 1 : -1; 

        Vector2 currentPos = piece.CurrentPiecePosition;
        List<Vector2> possibleAttackPositions = new List<Vector2>();

        Vector2 rightPos = new Vector2(currentPos.x+1, currentPos.y + (AttackStep * moveDir));
        Vector2 leftPos = new Vector2(currentPos.x-1, currentPos.y + (AttackStep * moveDir));

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


