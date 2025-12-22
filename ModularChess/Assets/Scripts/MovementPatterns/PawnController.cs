using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;


public class PawnController : BasePieceController, IMovement, IAttack
{
    public int MovementStep;
    public int HomeRowStep;

    [SerializeField] private int _homeRow = 1;

    
    public List<Vector2> FindMovements()
    {
        Vector2 currentPos = (Vector2)transform.position;
        List<Vector2> possibleMoves = new List<Vector2>();


        Vector2 oneStepPos = new Vector2(currentPos.x, currentPos.y + (MovementStep * moveDir));
        if (IsEmptyAtPos(oneStepPos)) {possibleMoves.Add(oneStepPos);}

        int currentHomeRow = (PieceTeam == Players.White) ? _homeRow : (int)boardShape.y - _homeRow;
        if (currentPos.y == currentHomeRow)
        {
            Vector2 homeRowStepPos = new Vector2(currentPos.x, currentPos.y + (HomeRowStep * moveDir));
            if (IsPathEmpty((Vector2)transform.position, homeRowStepPos)) {possibleMoves.Add(homeRowStepPos);}
        }

        return possibleMoves;
    }

    public List<Vector2> FindAttacks()
    {
        List<Vector2> possibleAttackPositions = new List<Vector2>();
        List<Vector2> validAttackPositions = new List<Vector2>();


        Vector2 rightPos = new Vector2(transform.position.x+1, transform.position.y + (1*moveDir));
        Vector2 leftPos = new Vector2(transform.position.x-1, transform.position.y + (1*moveDir));

        possibleAttackPositions.Add(rightPos);
        possibleAttackPositions.Add(leftPos);


        foreach (var possibleAttackPos in possibleAttackPositions)
        {
            if (IsInBounds(possibleAttackPos) == false) continue;

            if (BoardStateManager.Instance.BoardGameObjects.TryGetValue(possibleAttackPos, out GameObject pieceAtAttackPos) == false) continue;
            
            if (pieceAtAttackPos.GetComponent<BasePieceController>().PieceTeam == PieceTeam) continue;

            validAttackPositions.Add(possibleAttackPos);
        }

        return validAttackPositions;
    }
}


