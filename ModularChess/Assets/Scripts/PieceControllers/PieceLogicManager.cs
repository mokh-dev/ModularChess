using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PieceLogicManager
{
    public static List<Vector2> FindMovements(Vector2 piecePosition, Piece logicPiece, GameState logicGameState)
    {
        switch (logicPiece.MovementType)
        {
            case(PieceMovementType.Rectangle):
                return RectangleMovements(piecePosition, logicPiece, logicGameState);
            default:
                Debug.LogError("Movement Type Not Found");
                return null;
        }
    }
    public static List<Vector2> FindAttacks(Vector2 piecePosition, Piece logicPiece, GameState logicGameState)
    {
        switch (logicPiece.AttackType)
        {
            case(PieceAttackType.Rectangle):
                return RectangleAttacks(piecePosition, logicPiece, logicGameState);
            default:
                Debug.LogError("Attack Type Not Found");
                return null;
        }
    }



    private static List<Vector2> RectangleMovements(Vector2 piecePosition, Piece logicPiece, GameState logicGameState)
    {
        List<Vector2> possibleSquareMovements = FindSquarePositionsAtRange(piecePosition, logicPiece.MovementRange);

        return ValidateMovements(possibleSquareMovements, logicGameState);
    }

    private static List<Vector2> RectangleAttacks(Vector2 piecePosition, Piece logicPiece, GameState logicGameState)
    {
        List<Vector2> possibleSquareAttacks = FindSquarePositionsAtRange(piecePosition, logicPiece.AttackingRange);

        return ValidateAttacks(possibleSquareAttacks, logicPiece, logicGameState);
    }



    private static bool IsInBounds(Vector2 position)
    {
        if (position.x > 7) return false;
        if (position.x < 0) return false;

        if (position.y > 7) return false;
        if (position.y < 0) return false;

        return true;
    }
    private static bool IsEmptyAtPos(Vector2 endPos, GameState gameState)
    {  
        if (IsInBounds(endPos) == false) return false;
        if (gameState.BoardGameState.BoardPieces.TryGetValue(endPos, out Piece unused) == true) return false;
       
        return true;
    }
    private static bool IsPathEmpty(Vector2 currentPos, Vector2 endPos, GameState gameState)
    {
        Vector2 direction = DirectionalizeVector2(endPos - currentPos);
        Vector2 iteratedPos = currentPos + direction;

        while (iteratedPos != endPos)
        {
            if (IsValidMovement(iteratedPos, gameState) == false) return false;
            iteratedPos += direction;
        }

        return true;
    }
    private static Vector2 DirectionalizeVector2(Vector2 vector)
    {
        return new Vector2(
            Math.Sign(vector.x),
            Math.Sign(vector.y)
        );
    }

    private static bool IsValidAttack(Vector2 possibleAttackPos, Piece piece, GameState logicGameState)
    {
        if (IsInBounds(possibleAttackPos) == false) return false;
        if (logicGameState.BoardGameState.BoardPieces.TryGetValue(possibleAttackPos, out Piece pieceAtAttackPos) == false) return false;
        if (pieceAtAttackPos.Team == piece.Team) return false;

        return true;
    }
    private static List<Vector2> ValidateAttacks(List<Vector2> possibleAttackPositions, Piece piece, GameState logicGameState)
    {
        List<Vector2> validAttackPositions = new List<Vector2>();

        foreach (var possibleAttackPos in possibleAttackPositions)
        {
            if (IsValidAttack(possibleAttackPos, piece, logicGameState) == false) continue;

            validAttackPositions.Add(possibleAttackPos);
        }

        return validAttackPositions;
    }
    private static bool IsValidMovement(Vector2 possibleMovementPos, GameState gameState)
    {
        if (IsInBounds(possibleMovementPos) == false) return false;
        if (IsEmptyAtPos(possibleMovementPos, gameState) == false) return false;
        
        return true;
    }
    private static List<Vector2> ValidateMovements(List<Vector2> possibleMovementPositions, GameState gameState)
    {
        List<Vector2> validMovementPositions = new List<Vector2>();

        foreach (var possibleMovementPos in possibleMovementPositions)
        {
            if (IsValidMovement(possibleMovementPos, gameState) == false) continue;

            validMovementPositions.Add(possibleMovementPos);
        }

        return validMovementPositions;
    }






    private static int[] FindDiameterValuesOnAxis(int valueCount, int startingValue, int radius)
    {
        int[] values = new int[valueCount];

        for (int i = 0; i < valueCount; i++)
        {
            values[i] = startingValue - radius + i;
        }

        return values;
    }

    private static List<Vector2> FindSquarePositionsAtRange(Vector2 currentPos, int distanceToCorner)
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

    private static List<Vector2> FindSlidingMovements(Vector2 currentPos, Vector2 direction, int slideDistance, GameState gameState)
    {
        List<Vector2> possibleMoves = new List<Vector2>();

        Vector2 slidingPos = currentPos + direction;
        while (IsInBounds(slidingPos) && slideDistance > 0)
        {
            if (IsEmptyAtPos(slidingPos, gameState) == false) break;
            possibleMoves.Add(slidingPos);
            slidingPos += direction;

            slideDistance--;
        }

        return possibleMoves;
    }

    private static bool TryFindSlidingAttack(out Vector2 validAttack, Vector2 currentPos, Vector2 direction, int slideDistance, Piece piece, GameState gameState)
    {
        Vector2 attackPos;
        Vector2 lastPosition;

        List<Vector2> slidingMovements = FindSlidingMovements(currentPos, direction, slideDistance, gameState);

        if (slidingMovements.Count == 0)
        {
            lastPosition = currentPos;
        }
        else
        {
            lastPosition = slidingMovements.LastOrDefault();
        }


        attackPos = lastPosition + direction;
        
        if (IsValidAttack(attackPos, piece, gameState) == true)
        {
            validAttack = attackPos;
            return true;  
        } 

        validAttack = default;
        return false;
    }

    private static List<Vector2> FindLaneMovementsInDirections(List<Vector2> directions, Vector2 currentPos, GameState gameState, int slideDistance = 8)
    {
        List<Vector2> laneMovements = new List<Vector2>();

        for (int i = 0; i < directions.Count; i++)
        {
            laneMovements.AddRange(FindSlidingMovements(currentPos, directions[i], slideDistance, gameState));
        }

        return laneMovements;
    }

    private static List<Vector2> FindLaneAttacksInDirections(List<Vector2> directions, Vector2 currentPos, Piece piece, GameState gameState, int slideDistance = 8)
    {
        List<Vector2> laneAttacks = new List<Vector2>();

        for (int i = 0; i < directions.Count; i++)
        {
            if (TryFindSlidingAttack(out Vector2 possibleAttack, currentPos, directions[i], slideDistance, piece, gameState) == false) continue;
            if (IsInBounds(possibleAttack) == false) continue;
            laneAttacks.Add(possibleAttack);
        }

        return laneAttacks;
    }


}

