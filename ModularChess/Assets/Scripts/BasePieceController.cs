using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BasePieceController : MonoBehaviour
{

    public Players PieceTeam;


    protected SpriteRenderer sr;
    protected int moveDir;
    protected Vector2 boardShape = new Vector2(7,7);


    private IMovement movementPattern;
    private IAttack attackPattern;

    

    void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        UpdateTeamColor();

        movementPattern = gameObject.GetComponent<IMovement>();
        attackPattern = gameObject.GetComponent<IAttack>();

        moveDir = (PieceTeam == Players.White) ? 1 : -1;
    }

    public void UpdateTeamColor()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = (PieceTeam == Players.White) ? BoardDataManager.Instance.WhitePieceTeamColor : BoardDataManager.Instance.BlackPieceTeamColor;
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

    protected Vector2 DirectionalizeVector2(Vector2 vector)
    {
        return new Vector2(
            Math.Sign(vector.x),
            Math.Sign(vector.y)
        );
    }

    protected bool IsPathEmpty(Vector2 currentPos, Vector2 endPos)
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


   protected bool IsInBounds(Vector2 currentPos)
    {
        if (currentPos.x > boardShape.x) return false;
        if (currentPos.x < 0) return false;

        if (currentPos.y > boardShape.y) return false;
        if (currentPos.y < 0) return false;

        return true;
    }
    protected bool IsEmptyAtPos(Vector2 endPos)
    {  
        if (IsInBounds(endPos) == false) return false;
        if (BoardStateManager.Instance.BoardGameObjects.TryGetValue(endPos, out GameObject obj) == true) return false;

        return true;
    }

    protected List<Vector2> FindSlidingMovementsFromPosInDirection(Vector2 currentPos, Vector2 direction, int slideDistance = 8)
    {
        List<Vector2> possibleMoves = new List<Vector2>();

        Vector2 slidingPos = currentPos + direction;
        while (IsInBounds(slidingPos) || slideDistance <= 0)
        {
            possibleMoves.Add(slidingPos);
            slidingPos += direction;

            slideDistance--;
        }

        return possibleMoves;
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

