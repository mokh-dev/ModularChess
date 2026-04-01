using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class BoardPiecesManager : MonoBehaviour
{
    private static BoardPiecesManager instance;
    public static BoardPiecesManager Instance { get { return instance; } }


    public Dictionary<Vector2, PieceBuilder> BoardPieceObjects {get; private set;} = new Dictionary<Vector2, PieceBuilder>();

    [SerializeField] private GameObject _boardPiecesParent;
    [SerializeField] private GameObject _basePiecePre;
    [SerializeField] private GameObject _movementMarkerPre;
    [SerializeField] private GameObject _attackMarkerPre;

    

    public List<GameObject> Markers = new List<GameObject>();

    private BoardState currentBoardState => GameStateManager.Instance.GameStates.Last().BoardGameState;
    private GameState currentGameState => GameStateManager.Instance.GameStates.Last();

    


    [Header("---Test---")]
    [SerializeField] private Vector2 _testPiecePos;
    [SerializeField] private Piece _testPieceSO;




    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }

    }



    public void DisplayBoardState(BoardState updatedBoardState)
    {
        ClearBoard();

        foreach (KeyValuePair<Vector2, Piece> pieceInPos in updatedBoardState.BoardPieces)
        {
            AddNewPieceObj(pieceInPos.Key, pieceInPos.Value); 
        }
    }



    private void ClearBoard()
    {
        foreach (KeyValuePair<Vector2, PieceBuilder> pieceInPos in BoardPieceObjects)
        {
            Destroy(pieceInPos.Value.gameObject); 
        }

        BoardPieceObjects = new Dictionary<Vector2, PieceBuilder>();
    }


    public void AddNewPieceObj(Vector2 spawnPosition, Piece piece) 
    {
        GameObject newPieceObj = Instantiate(_basePiecePre, spawnPosition, Quaternion.identity, _boardPiecesParent.transform);

        PieceBuilder pieceBuilder = newPieceObj.GetComponent<PieceBuilder>();

        pieceBuilder.InitializePieceObj(spawnPosition, piece);
        BoardPieceObjects.Add(spawnPosition, pieceBuilder);
    }




    public void SpawnMarkersForPieceObj(PieceBuilder selectedPieceBuilder)
    {
        ClearAllMarkers();
        SpawnAttackMarkers(selectedPieceBuilder);
        SpawnMovementMarkers(selectedPieceBuilder);
    }

    private void SpawnMovementMarkers(PieceBuilder selectedPieceBuilder)
    {
        List<Vector2> possibleMovements = selectedPieceBuilder.ControlledPiece.GetMovements(selectedPieceBuilder.PiecePosition, currentGameState);
        foreach (Vector2 possibleMovementPosition in possibleMovements)
        {
            if (IsValidCurrentMove(selectedPieceBuilder.PiecePosition, possibleMovementPosition) == false) continue;

            GameObject newMarker = Instantiate(_movementMarkerPre, possibleMovementPosition, Quaternion.identity);
            Markers.Add(newMarker);
        }
    }

    private void SpawnAttackMarkers(PieceBuilder selectedPieceBuilder)
    {
        List<Vector2> possibleAttacks = selectedPieceBuilder.ControlledPiece.GetAttacks(selectedPieceBuilder.PiecePosition, currentGameState);

        foreach (Vector2 possibleAttackPosition in possibleAttacks)
        {
            if (IsValidCurrentAttack(selectedPieceBuilder.PiecePosition, possibleAttackPosition) == false) continue;

            GameObject newMarker = Instantiate(_attackMarkerPre, possibleAttackPosition, Quaternion.identity);
            Markers.Add(newMarker);
        }
    }

    private bool IsValidCurrentMove(Vector2 initialMovePos, Vector2 endMovePos)
    {
        (Vector2, Vector2) boardMove = (initialMovePos, endMovePos);

        if (BoardStateManager.Instance.IsValidBoardMove(boardMove) == false) return false;

        return true;
    }

    private bool IsValidCurrentAttack(Vector2 initialMovePos, Vector2 endMovePos)
    {
        (Vector2, Vector2) boardAttack = (initialMovePos, endMovePos);

        if (BoardStateManager.Instance.IsValidBoardAttack(boardAttack) == false) return false;

        return true;
    }


    

    public void ClearAllMarkers()
    {
        foreach (var marker in Markers)
        {
            Destroy(marker);
        }

        Markers.Clear();
    }

    public void AddTestPiece() // Editor Button
    {
        AddNewPieceObj(_testPiecePos, _testPieceSO);
    }

    public void PrintDictionary() // Editor Button
    {
        string outputPieces = "";
        string outputControllers = "";

        

        foreach (KeyValuePair<Vector2, Piece> piece in currentBoardState.BoardPieces)
        {
            outputPieces+= ", {" + piece.Key.ToString() + ": " + piece.Value.Type.ToString() + "}";
        }

        foreach (KeyValuePair<Vector2, PieceBuilder> piece in BoardPieceObjects)
        {
            outputControllers+= ", {" + piece.Key.ToString() + ": " + piece.Value.gameObject.name.ToString() + "}";
        }

        Debug.Log("Dictionary = [" + outputPieces + "]");
        Debug.Log("Dictionary = [" + outputControllers + "]");
    }
}
