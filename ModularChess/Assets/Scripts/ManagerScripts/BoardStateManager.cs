using System;
using System.Collections.Generic;
using UnityEngine;


public class BoardStateManager : MonoBehaviour
{
    private static BoardStateManager _instance;

    public static BoardStateManager Instance { get { return _instance; } }

    public Dictionary<Vector2, GameObject> BoardGameObjects { get; private set;} = new Dictionary<Vector2, GameObject>();
    public GameState gameState; 





    [Header("---Test---")]
    [SerializeField] private GameObject testPiecePre;
    [SerializeField] private Vector2 testPiecePos;
    [SerializeField] private int testpieceTeam;


    public List<GameObject> Markers = new List<GameObject>();



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
        AddNewPiece(testPiecePre, testPiecePos, testpieceTeam);
    }


    public void AddNewPiece(GameObject piecePre, Vector2 pos, int team)
    {
        GameObject newPiece = Instantiate(piecePre, pos, Quaternion.identity);
        newPiece.GetComponent<PieceController>().Team = team;

        BoardGameObjects.Add(pos, newPiece);
    }

    public void MovePiece(GameObject pieceToMove, Vector2 endPos)
    {
        if (CheckLegalMove(pieceToMove, endPos) == false) return;

        BoardGameObjects.Remove(new Vector2(pieceToMove.transform.position.x, pieceToMove.transform.position.y));

        pieceToMove.transform.position = endPos;

        BoardGameObjects.Add(endPos, pieceToMove);
    }

    public bool CheckLegalMove(GameObject piece, Vector2 endPos)
    {
        List<Vector2> possibleMoves = piece.GetComponent<PieceController>().FindCurrentPossibleMoves(BoardGameObjects);

        if (possibleMoves.Contains(endPos)) return true;

        return false;
    }


    public void ClearAllMarkers()
    {
        foreach (var marker in Markers)
        {
            Destroy(marker);
        }

        Markers.Clear();
    }

    public void PrintDictionary()
    {
        string output = "";

        foreach (KeyValuePair<Vector2, GameObject> item in BoardGameObjects)
        {
            output+= ", {" + item.Key.ToString() + ": " + item.Value.name.ToString() + "}";
        }

        Debug.Log("Dictionary = [" + output + "]");
    }


    public enum GameState
    {
        Team1MoveTurn,
        Team1CardTurn,

        Team2MoveTurn,
        Team2CardTurn
    }
}
