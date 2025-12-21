using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{

    public int Team;

    private SpriteRenderer sr;

    private Vector2 _boardPos;

    private IMovement movementPattern;
    private IAttack attackPattern;


    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        movementPattern = gameObject.GetComponent<IMovement>();
        attackPattern = gameObject.GetComponent<IAttack>();
    }


    public List<Vector2> FindCurrentPossibleMoves(Dictionary<Vector2, GameObject> boardGameObjects)
    {
        return movementPattern?.FindMoves(boardGameObjects, gameObject);
    }

    public List<Vector2> FindCurrentPossibleAttacks(Dictionary<Vector2, GameObject> boardGameObjects)
    {
        return attackPattern?.FindAttacks(boardGameObjects, gameObject);
    }



    public void SpawnMarkers()
    {
        BoardStateManager.Instance.ClearAllMarkers();

        List<Vector2> possibleMoves = FindCurrentPossibleMoves(BoardStateManager.Instance.BoardGameObjects);
        List<Vector2> possibleAttacks = FindCurrentPossibleAttacks(BoardStateManager.Instance.BoardGameObjects);


        movementPattern?.SpawnMoveMarkers(possibleMoves);
        attackPattern?.SpawnAttackMarkers(possibleAttacks);
    }

    public void SpawnPossibleAttackMarkers()
    {
        BoardStateManager.Instance.ClearAllMarkers();
        List<Vector2> possibleAttacks = FindCurrentPossibleAttacks(BoardStateManager.Instance.BoardGameObjects);
        attackPattern?.SpawnAttackMarkers(possibleAttacks);
    }
}

// have a base piece class and have pawn derive from it and have custom logic for pawns

