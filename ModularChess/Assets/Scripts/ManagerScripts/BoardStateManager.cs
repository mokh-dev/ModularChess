using System.Collections.Generic;
using UnityEngine;


public class BoardStateManager : MonoBehaviour
{
    private static BoardStateManager instance;

    public static BoardStateManager Instance { get { return instance; } }


    public Dictionary<Vector2, GameObject> BoardGameObjects { get; private set;} = new Dictionary<Vector2, GameObject>();
    public Players CurrentTurn {get; private set;} 


    [SerializeField] private GameObject _boardPiecesParent;


    [Header("---Test---")]
    [SerializeField] private GameObject testPiecePre;
    [SerializeField] private Vector2 _testPiecePos;
    [SerializeField] private Players _testPieceTeam;


    public List<GameObject> Markers = new List<GameObject>();


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }

        LoadBoardToDict(); 

        StartGame();
    }

    private void LoadBoardToDict()
    {
        foreach (var gameObjectTransform in _boardPiecesParent.GetComponentsInChildren<Transform>())
        {
            if (gameObjectTransform == _boardPiecesParent.transform) continue;
            BoardGameObjects.Add((Vector2)gameObjectTransform.position, gameObjectTransform.gameObject);
        }
    }

    private void StartGame()
    {
        CurrentTurn = Players.White;
    }
    
    public void EndPlayerTurn()
    {
        CurrentTurn = (CurrentTurn == Players.White) ? Players.Black : Players.White;
    }

    public void AddTestPiece()
    {
        AddNewPiece(testPiecePre, _testPiecePos, _testPieceTeam);
    }


    public void AddNewPiece(GameObject piecePre, Vector2 pos, Players team)
    {
        GameObject newPiece = Instantiate(piecePre, pos, Quaternion.identity);
        newPiece.transform.SetParent(_boardPiecesParent.transform);

        newPiece.GetComponent<PieceController>().PieceTeam = team;
        newPiece.GetComponent<PieceController>().RefreshTeam();

        BoardGameObjects.Add(pos, newPiece);
    }

    public void AttackPieceAtPos(Vector2 pos)
    {
        Destroy(BoardGameObjects[pos]);
        BoardGameObjects.Remove(pos);
    }

    public void MovePiece(GameObject pieceToMove, Vector2 endPos)
    {
        if (CheckLegalAttack(pieceToMove, endPos) == true) 
        {
            AttackPieceAtPos(endPos);
        }
        else
        {
            if (CheckLegalMovement(pieceToMove, endPos) == false) return;
        }


        BoardGameObjects.Remove((Vector2)pieceToMove.transform.position);

        pieceToMove.transform.position = endPos;
        BoardGameObjects.Add(endPos, pieceToMove);
    }

     

    public bool CheckLegalMove(GameObject piece, Vector2 endPos)
    {
        if (CheckLegalMovement(piece, endPos) == true) return true;
        if (CheckLegalAttack(piece, endPos) == true) return true;

        return false;
    }


    private bool CheckLegalMovement(GameObject piece, Vector2 endPos)
    {
        PieceController currentPieceController = piece.GetComponent<PieceController>();

        List<Vector2> possibleMovements = currentPieceController.FindCurrentPossibleMovements();

        if (possibleMovements.Contains(endPos)) return true;

        return false;
    }

    private bool CheckLegalAttack(GameObject piece, Vector2 endPos)
    {
        PieceController currentPieceController = piece.GetComponent<PieceController>();

        List<Vector2> possibleAttacks = currentPieceController.FindCurrentPossibleAttacks();

        if (possibleAttacks.Contains(endPos)) return true;

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
}

public enum Players
{
    White,
    Black,
}