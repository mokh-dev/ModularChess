using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class BoardPiecesManager : MonoBehaviour
{
    private static BoardPiecesManager instance;
    public static BoardPiecesManager Instance { get { return instance; } }


    public Dictionary<Vector2, PieceController> BoardPieceObjects = new Dictionary<Vector2, PieceController>();

    public GameObject BoardPiecesParent;

    public List<GameObject> Markers = new List<GameObject>();

    public UnityEvent ResetLastMove;

    


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
        foreach (var gameObjectTransform in BoardPiecesParent.GetComponentsInChildren<Transform>())
        {
            if (gameObjectTransform == BoardPiecesParent.transform) continue;

            gameObjectTransform.gameObject.GetComponent<PieceController>().InitializePieceObj();
            BoardPieceObjects.Add((Vector2)gameObjectTransform.position, gameObjectTransform.gameObject.GetComponent<PieceController>());
        }  
    }

    public bool IsInCheck(BoardState boardState, Players teamToCheck)
    {
        if (TryFindKingOfTeam(boardState, teamToCheck, out Piece kingPiece) == false) return false;

        Players enemyTeam = (teamToCheck == Players.White) ? Players.Black : Players.White;
        List<Vector2> enemyAttackPositions = GetAllAttacksFromTeam(boardState, enemyTeam);

        foreach (Vector2 attackPos in enemyAttackPositions)
        {
            if (attackPos == kingPiece.PiecePosition)
            {
                return true;
            } 
        }

        return false;
    }
    
    private bool TryFindKingOfTeam(BoardState boardState, Players kingTeam, out Piece kingPiece)
    {
        kingPiece = default;

        foreach (KeyValuePair<Vector2, Piece> boardPosPiece in boardState.BoardPieces)
        {
            if (boardPosPiece.Value.PieceType != PieceTypes.King) continue;
            if (boardPosPiece.Value.PieceTeam != kingTeam) continue;

            kingPiece = boardPosPiece.Value;
            return true;
        }

        return false;
    }

    private List<Vector2> GetAllAttacksFromTeam(BoardState boardState, Players team)
    {
        List<Vector2> totalAttacks = new List<Vector2>();

        foreach (KeyValuePair<Vector2, Piece> piece in boardState.BoardPieces)
        {
            if (piece.Value.PieceTeam != team) continue;

            List<Vector2> pieceAttacks = piece.Value.GetAttacks();
            totalAttacks.AddRange(pieceAttacks);
        } 

        return totalAttacks;
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

        BoardStateManager.Instance.CurrentBoardState.BoardPieces.Add(pos, newPieceController.GetInitialPiece());
    }

    
    public void MoveBoardPieceObj(BoardMove move)
    {
        Vector2 initialPosition = move.PieceMove.Item1;
        Vector2 endPostion = move.PieceMove.Item2;

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

    
    public void RebuildBoardState(BoardState boardState)
    {
        //TODO rebuild board state
        //pieceController MovePieceObj becomes unneccesary 
        //and is simulated goes away
        //along with the connected BoardPieceManager functions
    }



    public bool CheckForPawnEnPasant(Piece pieceToCheck, Vector2 endPos, out Vector2 attackPosition)
    {
        attackPosition = default;

        if (pieceToCheck.PieceType != PieceTypes.Pawn) return false;

        PawnController pawnController = (PawnController)pieceToCheck.Logic;
        if (pawnController.EnPasantEnemyMovementAttackPositions.Count == 0) return false;
        
        if (pawnController.EnPasantEnemyMovementAttackPositions.TryGetValue(endPos, out Vector2 possibleAttackPosition) == false) return false;

        attackPosition = possibleAttackPosition;
        pawnController.ClearEnPasantDict();

        return true;
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
        Debug.Log(selectedPieceController.ControlledPiece.PieceTeam);
        Debug.Log(selectedPieceController.ControlledPiece.PieceType);
        Debug.Log(selectedPieceController.ControlledPiece.PiecePosition);
        Debug.Log("selectedPieceController.ControlledPiece.TurnCount: " + selectedPieceController.ControlledPiece.TurnCount.ToString());
        List<Vector2> possibleAttacks = selectedPieceController.ControlledPiece.GetAttacks();
        Debug.Log("possibleAttacks.Count: " + possibleAttacks.Count.ToString());

        foreach (Vector2 possibleAttackPosition in possibleAttacks)
        {
            Debug.Log("checking attack on " + possibleAttackPosition.ToString());
            if (IsValidCurrentMove(selectedPieceController.ControlledPiece.PiecePosition, possibleAttackPosition) == false) continue;

            GameObject newMarker = Instantiate(BoardDataManager.Instance.PossibleAttackMarkerPre, possibleAttackPosition, Quaternion.identity);
            Markers.Add(newMarker);
        }
    }

    private bool IsValidCurrentMove(Vector2 initialMovePos, Vector2 endMovePos)
    {
        BoardMove boardMove = new BoardMove();
        boardMove.PieceMove = (initialMovePos, endMovePos);

        Debug.Log("Valid turn count: " + BoardStateManager.Instance.CurrentBoardState.TurnCount.ToString());
        if (BoardStateManager.Instance.IsValidBoardMove(BoardStateManager.Instance.CurrentBoardState, boardMove, out BoardState _, out Vector2? __) == false) return false;

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

        

        foreach (KeyValuePair<Vector2, Piece> piece in BoardStateManager.Instance.CurrentBoardState.BoardPieces)
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
