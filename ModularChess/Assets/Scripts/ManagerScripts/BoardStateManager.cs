using System;
using System.Collections.Generic;
using UnityEngine;


public class BoardStateManager : MonoBehaviour
{
    private static BoardStateManager _instance;

    public static BoardStateManager Instance { get { return _instance; } }

    [field: SerializeField] public GameObject[] BoardGameObjects { get; private set;} = new GameObject[63];
    public GameState gameState; 





    [Header("---Test---")]
    [SerializeField] private PieceScriptableObject testpieceSO;
    [SerializeField] private Vector2 testPiecePos;
    [SerializeField] private int testpieceTeam;


    public List<GameObject> PossibleMoveMarkers = new List<GameObject>();



    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        // gameState = GameState.Team1MoveTurn; 
    }


    public void AddTestPiece()
    {
        AddNewPiece(testPiecePos, testpieceTeam, testpieceSO);
    }


    public void AddNewPiece(Vector2 pos, int team, PieceScriptableObject pieceData)
    {
        GameObject newPiece = Instantiate(BoardDataManager.Instance.basePiecePre, pos, Quaternion.identity);

        newPiece.GetComponent<PieceController>().LoadPieceData(pieceData, team);


        BoardGameObjects[BoardHelper.ConvertVector2PosToIntPos(pos)] = newPiece;
    }




    public void ClearPossibleMoveMarkers()
    {
        foreach (var possibleMoveMarker in PossibleMoveMarkers)
        {
            Destroy(possibleMoveMarker);
        }

        PossibleMoveMarkers.Clear();
    }


    public enum GameState
    {
        Team1MoveTurn,
        Team1CardTurn,

        Team2MoveTurn,
        Team2CardTurn
    }
}
