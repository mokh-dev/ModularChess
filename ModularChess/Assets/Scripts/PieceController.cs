using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{

    [SerializeField] private int team;
    [SerializeField] private PieceScriptableObject pieceData;

    private SpriteRenderer sr;


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




    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();


        List<Vector2> possibleMoves = pieceData.PieceMovementPattern.FindPossibleMoves(BoardStateManager.Instance.BoardGameObjects, new Vector2(transform.position.x, transform.position.y), team);
        foreach (var move in possibleMoves)
        {
            Debug.Log(move);
        }

        BoardStateManager.Instance.SpawnPossibleMoveMarkers(possibleMoves);
    }

    void Update()
    {
        
        
    }
}
