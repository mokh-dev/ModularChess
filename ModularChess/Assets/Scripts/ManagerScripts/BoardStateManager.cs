using System;
using System.Collections.Generic;
using UnityEngine;


public class BoardStateManager : MonoBehaviour
{
    private static BoardStateManager _instance;

    public static BoardStateManager Instance { get { return _instance; } }

    [field: SerializeField] public GameObject[] BoardGameObjects { get; private set;} = new GameObject[63];





    [Header("---Test---")]
    [SerializeField] private PieceScriptableObject testpieceSO;
    [SerializeField] private Vector2 testPiecePos;
    [SerializeField] private int testpieceTeam;


    private List<GameObject> _possibleMoveMarkers = new List<GameObject>();



    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
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


    public void SpawnPossibleMoveMarkers(List<Vector2> possibleMovesToMark)
    {
        ClearPossibleMoveMarkers();

        foreach (var possibleMove in possibleMovesToMark)
        {
            GameObject newMarker = Instantiate(BoardDataManager.Instance.possibleMoveMarkerPre, possibleMove, Quaternion.identity);
            _possibleMoveMarkers.Add(newMarker);
        }
    }

    private void ClearPossibleMoveMarkers()
    {
        foreach (var possibleMoveMarker in _possibleMoveMarkers)
        {
            Destroy(possibleMoveMarker);
        }

        _possibleMoveMarkers.Clear();
    }


}
