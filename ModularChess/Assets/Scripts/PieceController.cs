using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PieceController : MonoBehaviour
{

    public int Team;

    private SpriteRenderer sr;

    private Vector2 _boardPos;

    private List<Vector2> _possibleMoves;

    private IMovementPattern movementPattern;


    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        movementPattern = gameObject.GetComponent<IMovementPattern>();
    }

    public List<Vector2> FindCurrentPossibleMoves(Dictionary<Vector2, GameObject> boardGameObjects)
    {
        return movementPattern?.FindPossibleMoves(boardGameObjects, gameObject);
    }


    public void SpawnPossibleMoveMarkers()
    {
        BoardStateManager.Instance.ClearPossibleMoveMarkers();
        _possibleMoves = FindCurrentPossibleMoves(BoardStateManager.Instance.BoardGameObjects);

        foreach (var possibleMove in _possibleMoves)
        {
            GameObject newMarker = Instantiate(BoardDataManager.Instance.possibleMoveMarkerPre, possibleMove, Quaternion.identity);
            BoardStateManager.Instance.PossibleMoveMarkers.Add(newMarker);
        }
    }
}

public interface IMovementPattern
{
    List<Vector2> FindPossibleMoves(Dictionary<Vector2, GameObject> boardGameObjects, GameObject currentPiece);
}