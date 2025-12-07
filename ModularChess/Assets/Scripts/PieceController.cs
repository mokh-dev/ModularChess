using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{

    [SerializeField] private int team;
    [SerializeField] private PieceScriptableObject pieceData;

    private SpriteRenderer sr;

    private Vector2 _boardPos;

    private List<Vector2> _possibleMoves;


    public void LoadPieceData(PieceScriptableObject loadPieceData, int loadTeam)
    {
        pieceData = loadPieceData;
        team = loadTeam;

        UpdatePieceData();
    }

    private void UpdatePieceData()
    {
        gameObject.name = pieceData.PieceName;

        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = pieceData.PieceSprite;

        BoardStateManager boardStateManager = GameObject.FindAnyObjectByType<BoardStateManager>();
        sr.color = (team == 1) ? BoardDataManager.Instance.LightPieceTeamColor : BoardDataManager.Instance.DarkPieceTeamColor;
    }


    public List<Vector2> FindCurrentPossibleMoves()
    {
        return pieceData.PieceMovementPattern.FindPossibleMoves(BoardStateManager.Instance.BoardGameObjects, new Vector2(transform.position.x, transform.position.y), team);
    }

    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }


    public void SpawnPossibleMoveMarkers()
    {
        BoardStateManager.Instance.ClearPossibleMoveMarkers();

        _possibleMoves = FindCurrentPossibleMoves();
        foreach (var possibleMove in _possibleMoves)
        {
            GameObject newMarker = Instantiate(BoardDataManager.Instance.possibleMoveMarkerPre, possibleMove, Quaternion.identity);
            BoardStateManager.Instance.PossibleMoveMarkers.Add(newMarker);
        }
    }

    


}
