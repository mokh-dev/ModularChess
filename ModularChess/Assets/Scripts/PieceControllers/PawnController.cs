using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceController))]
public class PawnController : PieceMoveLogic
{
    public int MovementStep = 1;
    public int AttackStep = 1;
    public int HomeRowStep = 2;

    private int whiteHomeRow = 1;
    private int blackHomeRow = 6;



    public override List<Vector2> FindMovements()
    {
        int homeRow = (LogicPiece.PieceTeam == Players.White) ? whiteHomeRow : blackHomeRow;
        int moveDir = (LogicPiece.PieceTeam == Players.White) ? 1 : -1; 
        
        Vector2 currentPos = LogicPiece.PiecePosition;
        List<Vector2> possibleMoves = new List<Vector2>();


        Vector2 oneStepPos = new Vector2(currentPos.x, currentPos.y + (MovementStep * moveDir));
        if (IsValidMovement(oneStepPos)) {possibleMoves.Add(oneStepPos);}


        if (currentPos.y == homeRow)
        {
            Vector2 homeRowStepPos = new Vector2(currentPos.x, currentPos.y + (HomeRowStep * moveDir));

            if (IsPathEmpty(LogicPiece.PiecePosition, homeRowStepPos) && IsEmptyAtPos(homeRowStepPos))
            {
                possibleMoves.Add(homeRowStepPos);
            }            
        }

        return possibleMoves;
    }

    public override List<Vector2> FindAttacks()
    {
        int moveDir = (LogicPiece.PieceTeam == Players.White) ? 1 : -1; 

        Vector2 currentPos = LogicPiece.PiecePosition;
        List<Vector2> possibleAttackPositions = new List<Vector2>();

        Vector2 rightPos = new Vector2(currentPos.x+1, currentPos.y + (AttackStep * moveDir));
        Vector2 leftPos = new Vector2(currentPos.x-1, currentPos.y + (AttackStep * moveDir));

        possibleAttackPositions.Add(rightPos);
        possibleAttackPositions.Add(leftPos);

        return ValidateAttacks(possibleAttackPositions);
    }
}


