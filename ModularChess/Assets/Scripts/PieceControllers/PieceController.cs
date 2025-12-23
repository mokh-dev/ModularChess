using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceController : MonoBehaviour
{

    public Players PieceTeam;


    public SpriteRenderer sr {get; private set;}
    public int moveDir {get; private set;}
    public Vector2 boardShape {get; private set;} = new Vector2(7,7);


    public IMovement movementPattern {get; private set;}
    public IAttack attackPattern {get; private set;}

    

    void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        RefreshTeam();

        movementPattern = gameObject.GetComponent<IMovement>();
        attackPattern = gameObject.GetComponent<IAttack>();

        moveDir = (PieceTeam == Players.White) ? 1 : -1;
    }

    public void RefreshTeam()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = (PieceTeam == Players.White) ? BoardDataManager.Instance.WhitePieceTeamColor : BoardDataManager.Instance.BlackPieceTeamColor;
    
        Vector2 position = (Vector2)transform.position;
        gameObject.name = PieceTeam.ToString() + " Piece at: (" + ((int)position.x).ToString() + "," + ((int)position.y).ToString() + ")";
    }


    public List<Vector2> FindCurrentPossibleMovements()
    {
        return movementPattern?.FindMovements();
    }

    public List<Vector2> FindCurrentPossibleAttacks()
    {
        return attackPattern?.FindAttacks();
    }

    public void SpawnMarkers()
    {
        BoardStateManager.Instance.ClearAllMarkers();

        List<Vector2> possibleMoves = FindCurrentPossibleMovements();
        foreach (var location in possibleMoves)
        {
            GameObject newMarker = Instantiate(BoardDataManager.Instance.PossibleMovementMarkerPre, location, Quaternion.identity);
            BoardStateManager.Instance.Markers.Add(newMarker);
        }

        List<Vector2> possibleAttack = FindCurrentPossibleAttacks();
        foreach (var location in possibleAttack)
        {
            GameObject newMarker = Instantiate(BoardDataManager.Instance.PossibleAttackMarkerPre, location, Quaternion.identity);
            BoardStateManager.Instance.Markers.Add(newMarker);
        }
    }



    public Vector2 DirectionalizeVector2(Vector2 vector)
    {
        return new Vector2(
            Math.Sign(vector.x),
            Math.Sign(vector.y)
        );
    }

    public bool IsPathEmpty(Vector2 currentPos, Vector2 endPos)
    {
        Vector2 direction = DirectionalizeVector2(endPos - currentPos);
        Vector2 iteratedPos = currentPos + direction;

        while (iteratedPos != endPos)
        {
            if (IsEmptyAtPos(iteratedPos) == false) return false;
            iteratedPos += direction;
        }

        return true;
    }


   public bool IsInBounds(Vector2 currentPos)
    {
        if (currentPos.x > boardShape.x) return false;
        if (currentPos.x < 0) return false;

        if (currentPos.y > boardShape.y) return false;
        if (currentPos.y < 0) return false;

        return true;
    }
    public bool IsEmptyAtPos(Vector2 endPos)
    {  
        if (IsInBounds(endPos) == false) return false;
        if (BoardStateManager.Instance.BoardGameObjects.TryGetValue(endPos, out GameObject obj) == true) return false;

        return true;
    }

    public List<Vector2> FindSlidingMovements(Vector2 currentPos, Vector2 direction, int slideDistance = 8)
    {
        List<Vector2> possibleMoves = new List<Vector2>();

        Vector2 slidingPos = currentPos + direction;
        while (IsInBounds(slidingPos) || slideDistance <= 0)
        {
            if (IsEmptyAtPos(slidingPos) == false) break;
            possibleMoves.Add(slidingPos);
            slidingPos += direction;

            slideDistance--;
        }

        return possibleMoves;
    }

    public bool TryFindSlidingAttack(out Vector2 validAttack, Vector2 currentPos, Vector2 direction, int slideDistance = 8)
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

    public bool IsValidAttack(Vector2 attackPos)
    {
        if (IsInBounds(attackPos) == false) return false;

        if (BoardStateManager.Instance.BoardGameObjects.TryGetValue(attackPos, out GameObject pieceAtAttackPos) == false) return false;
            
        if (pieceAtAttackPos.GetComponent<PieceController>().PieceTeam == PieceTeam) return false;

        return true;
    }

    public List<Vector2> ValidateAttacks(List<Vector2> possibleAttackPositions)
    {
        List<Vector2> validAttackPositions = new List<Vector2>();

        foreach (var possibleAttackPos in possibleAttackPositions)
        {
            if (IsValidAttack(possibleAttackPos) == false) continue;

            validAttackPositions.Add(possibleAttackPos);
        }

        return validAttackPositions;
    }


}

public interface IMovement
{
    List<Vector2> FindMovements();
}

public interface IAttack
{
    List<Vector2> FindAttacks();
}

