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
        int homeRow = (LogicPiece.PieceTeam == Players.White) ? whiteHomeRow : blackHomeRow; 
        int moveDir = (LogicPiece.PieceTeam == Players.White) ? 1 : -1; 
        
        Vector2 previousPosition = LogicPiece.PreviousPiecePositions[LogicPiece.TurnCount - 1];

        // Debug.Log("checking piece:");
        // Debug.Log(LogicPiece.PieceTeam);
        // Debug.Log(LogicPiece.PieceType);
        // Debug.Log(LogicPiece.PiecePosition);
        // Debug.Log("LogicPiece.TurnCount: " + LogicPiece.TurnCount.ToString());
        // foreach (KeyValuePair<int, Vector2> pos in LogicPiece.PreviousPiecePositions)
        // {
        //     Debug.Log(pos.Key.ToString() + ": " + pos.Value.ToString());
        
        // }
        if (previousPosition.y != homeRow) return false;

        Vector2 homeRowStepPos = new Vector2(previousPosition.x, previousPosition.y + (HomeRowStep * moveDir));
        if (homeRowStepPos != LogicPiece.PiecePosition) return false;

        return true;
    }

    private bool CheckCanEnPasant(Vector2 positionToCheck)
    {
        if (LogicPiece.UsedBoardState.BoardPieces.TryGetValue(positionToCheck, out Piece pieceInPosition) == false) return false;

        if (pieceInPosition.PieceTeam == LogicPiece.PieceTeam) return false;

        if (pieceInPosition.PieceType != PieceTypes.Pawn) return false; 
        // Debug.Log("check 4");
        // Debug.Log(LogicPiece.PieceTeam);
        // Debug.Log(LogicPiece.PieceType);
        // Debug.Log(LogicPiece.PiecePosition);
        // Debug.Log("LogicPiece.TurnCount: " + LogicPiece.TurnCount.ToString());


        if (((PawnController)pieceInPosition.Logic).CanBeEnPasanted() == false) return false;
        Debug.Log("check true");

        return true;
    }


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

        Vector2 rightEnPasantPos = new Vector2(currentPos.x+1, currentPos.y);
        Vector2 leftEnPasantPos = new Vector2(currentPos.x-1, currentPos.y);

        List<Vector2> validNormalAttacks = ValidateAttacks(possibleAttackPositions);

        ClearEnPasantDict();

        if (CheckCanEnPasant(rightEnPasantPos) == true)
        {
            Debug.Log("can en pasant to the right");
            EnPasantEnemyMovementAttackPositions.Add(rightPos, rightEnPasantPos); 
            validNormalAttacks.Add(rightPos);
        }
        if (CheckCanEnPasant(leftEnPasantPos) == true)
        {
            Debug.Log("can en pasant to the left");
            EnPasantEnemyMovementAttackPositions.Add(leftPos, leftEnPasantPos); 
            validNormalAttacks.Add(leftPos);
        }

        return validNormalAttacks;
    }
}


