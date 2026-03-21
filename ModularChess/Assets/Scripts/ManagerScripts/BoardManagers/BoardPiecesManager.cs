using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class BoardPiecesManager : MonoBehaviour
{
    private static BoardPiecesManager instance;
    public static BoardPiecesManager Instance { get { return instance; } }


    public Dictionary<Vector2, PieceController> BoardPieceObjects = new Dictionary<Vector2, PieceController>();

    public GameObject BoardPiecesParent;

    public List<GameObject> Markers = new List<GameObject>();

    public UnityEvent ResetLastMove;

    
    private BoardState currentBoardState => GameStateManager.Instance.GameStates.Last().BoardGameState;


    [Header("---Test---")]
    [SerializeField] private Players _testPieceTeam;
    [SerializeField] private PieceTypes _testPieceType;
    [SerializeField] private Vector2 _testPiecePos;




    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }


        LoadBoardObjectsToDict();
    }

    private void LoadBoardObjectsToDict()
    {
        foreach (Transform gameObjectTransform in BoardPiecesParent.transform)
        {
            if (gameObjectTransform == BoardPiecesParent.transform) continue; //TODO check if this is needed

            gameObjectTransform.gameObject.GetComponent<PieceController>().InitializePieceObj();
            BoardPieceObjects.Add((Vector2)gameObjectTransform.position, gameObjectTransform.gameObject.GetComponent<PieceController>());
        }  
    }


    public void DisplayBoardState(BoardState updatedBoardState)
    {
        
    }




    public void AddTestPiece()
    {
        AddNewPieceObj(_testPieceType, _testPiecePos, _testPieceTeam);
    }


    // FIXME adds to CurrentBoardState which doesnt work
    public void AddNewPieceObj(PieceTypes type, Vector2 pos, Players team) 
    {
        GameObject newPieceObj = Instantiate(BoardDataManager.Instance.BasePiecePre, pos, Quaternion.identity);
        newPieceObj.transform.SetParent(BoardPiecesParent.transform);

        PieceController newPieceController = newPieceObj.GetComponent<PieceController>();

        newPieceController.PieceObjType = type;
        newPieceController.PieceObjTeam = team;

        newPieceController.InitializePieceObj();

        //currentBoardState.BoardPieces.Add(pos, newPieceController.GetInitialPiece());
    }

    
    public void MoveBoardPieceObj((Vector2, Vector2) boardMove)
    {
        Vector2 initialPosition = boardMove.Item1;
        Vector2 endPostion = boardMove.Item2;

        PieceController pieceControllerToMove = BoardPieceObjects[initialPosition];

        BoardPieceObjects.Remove(initialPosition);

        pieceControllerToMove.MovePieceObj(endPostion);

        BoardPieceObjects.Add(endPostion, pieceControllerToMove);  
    }

    public void DestroyPieceObjAtPos(Vector2 pos)
    {
        Destroy(BoardPieceObjects[pos].gameObject);
        BoardPieceObjects.Remove(pos);
    }


    public void SpawnMarkersForPieceObj(PieceController selectedPieceController)
    {
        ClearAllMarkers();
        SpawnAttackMarkers(selectedPieceController);
        SpawnMovementMarkers(selectedPieceController);
    }

    private void SpawnMovementMarkers(PieceController selectedPieceController)
    {
        List<Vector2> possibleMovements = selectedPieceController.ControlledPiece.GetMovements();
        foreach (Vector2 possibleMovementPosition in possibleMovements)
        {
            if (IsValidCurrentMove(selectedPieceController.ControlledPiece.PiecePosition, possibleMovementPosition) == false) continue;

            GameObject newMarker = Instantiate(BoardDataManager.Instance.PossibleMovementMarkerPre, possibleMovementPosition, Quaternion.identity);
            Markers.Add(newMarker);
        }
    }

    private void SpawnAttackMarkers(PieceController selectedPieceController)
    {
        List<Vector2> possibleAttacks = selectedPieceController.ControlledPiece.GetAttacks();

        foreach (Vector2 possibleAttackPosition in possibleAttacks)
        {
            if (IsValidCurrentAttack(selectedPieceController.ControlledPiece.PiecePosition, possibleAttackPosition) == false) continue;

            GameObject newMarker = Instantiate(BoardDataManager.Instance.PossibleAttackMarkerPre, possibleAttackPosition, Quaternion.identity);
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

    public void PrintDictionary()
    {
        string outputPieces = "";
        string outputControllers = "";

        

        foreach (KeyValuePair<Vector2, Piece> piece in currentBoardState.BoardPieces)
        {
            outputPieces+= ", {" + piece.Value.PiecePosition.ToString() + ": " + piece.Value.PieceType.ToString() + "}";
        }

        foreach (KeyValuePair<Vector2, PieceController> piece in BoardPieceObjects)
        {
            outputControllers+= ", {" + piece.Key.ToString() + ": " + piece.Value.gameObject.name.ToString() + "}";
        }

        Debug.Log("Dictionary = [" + outputPieces + "]");
        Debug.Log("Dictionary = [" + outputControllers + "]");
    }
}
