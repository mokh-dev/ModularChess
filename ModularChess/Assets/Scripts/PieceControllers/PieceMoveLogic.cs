using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PieceMoveLogic
{
    public abstract List<Vector2> FindMovements();
    public abstract List<Vector2> FindAttacks();

    public Piece LogicPiece;
    
    protected BoardState currentBoardState => BoardStateManager.Instance.BoardStates[LogicPiece.TurnCount];

   protected bool IsInBounds(Vector2 currentPos)
    {
        if (currentPos.x > 7) return false;
        if (currentPos.x < 0) return false;

        if (currentPos.y > 7) return false;
        if (currentPos.y < 0) return false;

        return true;
    }
    protected bool IsEmptyAtPos(Vector2 endPos)
    {  
        if (IsInBounds(endPos) == false) return false;
        if (currentBoardState.BoardPieces.TryGetValue(endPos, out Piece unused) == true) return false;
       
        return true;
    }
    protected bool IsPathEmpty(Vector2 currentPos, Vector2 endPos)
    {
        Vector2 direction = DirectionalizeVector2(endPos - currentPos);
        Vector2 iteratedPos = currentPos + direction;

        while (iteratedPos != endPos)
        {
            if (IsValidMovement(iteratedPos) == false) return false;
            iteratedPos += direction;
        }

        return true;
    }
    private Vector2 DirectionalizeVector2(Vector2 vector)
    {
        return new Vector2(
            Math.Sign(vector.x),
            Math.Sign(vector.y)
        );
    }

    protected bool IsValidAttack(Vector2 attackPos)
    {
        if (IsInBounds(attackPos) == false) return false;
        if (currentBoardState.BoardPieces.TryGetValue(attackPos, out Piece pieceAtAttackPos) == false) return false;
        if (pieceAtAttackPos.PieceTeam == LogicPiece.PieceTeam) return false;

        return true;
    }
    protected List<Vector2> ValidateAttacks(List<Vector2> possibleAttackPositions)
    {
        List<Vector2> validAttackPositions = new List<Vector2>();

        foreach (var possibleAttackPos in possibleAttackPositions)
        {
            if (IsValidAttack(possibleAttackPos) == false) continue;

            validAttackPositions.Add(possibleAttackPos);
        }

        return validAttackPositions;
    }
    protected bool IsValidMovement(Vector2 possibleMovementPos)
    {
        if (IsInBounds(possibleMovementPos) == false) return false;
        if (IsEmptyAtPos(possibleMovementPos) == false) return false;

        return true;
    }
    protected List<Vector2> ValidateMovements(List<Vector2> possibleMovementPositions)
    {
        List<Vector2> validMovementPositions = new List<Vector2>();

        foreach (var possibleMovementPos in possibleMovementPositions)
        {
            if (IsValidMovement(possibleMovementPos) == false) continue;

            validMovementPositions.Add(possibleMovementPos);
        }

        return validMovementPositions;
    }






    private int[] FindDiameterValuesOnAxis(int valueCount, int startingValue, int radius)
    {
        int[] values = new int[valueCount];

        for (int i = 0; i < valueCount; i++)
        {
            values[i] = startingValue - radius + i;
        }

        return values;
    }

    protected List<Vector2> FindSquarePositionsAtRange(Vector2 currentPos, int distanceToCorner)
    {
        if (distanceToCorner < 1) return new List<Vector2>();

        List<Vector2> squarePositions = new List<Vector2>();

        int diameterValueCount = 1 + 2*distanceToCorner;

        int[] xDiameterValues;
        int[] yDiameterValues;

        xDiameterValues = FindDiameterValuesOnAxis(diameterValueCount, (int)currentPos.x, distanceToCorner);
        yDiameterValues = FindDiameterValuesOnAxis(diameterValueCount, (int)currentPos.y, distanceToCorner);


        foreach (int xValue in xDiameterValues)
        {
            foreach (int yValue in yDiameterValues)
            {
                squarePositions.Add(new Vector2(xValue, yValue));
            }
        }

        return squarePositions;
    }

    protected List<Vector2> FindSlidingMovements(Vector2 currentPos, Vector2 direction, int slideDistance)
    {
        List<Vector2> possibleMoves = new List<Vector2>();

        Vector2 slidingPos = currentPos + direction;
        while (IsInBounds(slidingPos) && slideDistance > 0)
        {
            if (IsEmptyAtPos(slidingPos) == false) break;
            possibleMoves.Add(slidingPos);
            slidingPos += direction;

            slideDistance--;
        }

        return possibleMoves;
    }

    protected bool TryFindSlidingAttack(out Vector2 validAttack, Vector2 currentPos, Vector2 direction, int slideDistance)
    {
        Vector2 attackPos;
        Vector2 lastPosition;

        List<Vector2> slidingMovements = FindSlidingMovements(currentPos, direction, slideDistance);

        if (slidingMovements.Count == 0)
        {
            lastPosition = currentPos;
        }
        else
        {
            lastPosition = slidingMovements.LastOrDefault();
        }


        attackPos = lastPosition + direction;
        
        if (IsValidAttack(attackPos) == true)
        {
            validAttack = attackPos;
            return true;  
        } 

        validAttack = default;
        return false;
    }

    protected List<Vector2> FindLaneMovementsInDirections(List<Vector2> directions, Vector2 currentPos, int slideDistance = 8)
    {
        List<Vector2> laneMovements = new List<Vector2>();

        for (int i = 0; i < directions.Count; i++)
        {
            laneMovements.AddRange(FindSlidingMovements(currentPos, directions[i], slideDistance));
        }

        return laneMovements;
    }

    protected List<Vector2> FindLaneAttacksInDirections(List<Vector2> directions, Vector2 currentPos, int slideDistance = 8)
    {
        List<Vector2> laneAttacks = new List<Vector2>();

        for (int i = 0; i < directions.Count; i++)
        {
            if (TryFindSlidingAttack(out Vector2 possibleAttack, currentPos, directions[i], slideDistance) == false) continue;
            if (IsInBounds(possibleAttack) == false) continue;
            laneAttacks.Add(possibleAttack);
        }

        return laneAttacks;
    }


}

