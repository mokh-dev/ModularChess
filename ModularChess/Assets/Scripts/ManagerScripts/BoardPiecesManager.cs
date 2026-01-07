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
            if (attackPos == kingPiece.CurrentPiecePosition)
            {
                return true;
            } 
        }

        return false;
    }

    private List<Vector2> GetAllAttacksFromTeam(BoardState boardState, Players team)
    {
        List<Vector2> totalAttacks = new List<Vector2>();

        foreach (KeyValuePair<Vector2, Piece> piece in boardState.BoardPieces)
        {
            if (piece.Value.PieceTeam != team) continue;

            List<Vector2> pieceAttacks = piece.Value.GetCurrentAttacks();
            totalAttacks.AddRange(pieceAttacks);
        } 

        return totalAttacks;
    }

    private bool IsValidCheckDefenseMove()
    {
        //simulates move 
        //checks if still in check
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



    public void AddTestPiece()
    {
        AddNewPieceObj(_testPieceType, _testPiecePos, _testPieceTeam);
    }


    public void AddNewPieceObj(PieceTypes type, Vector2 pos, Players team) 
    {
        GameObject newPieceObj = Instantiate(BoardDataManager.Instance.BasePiecePre, pos, Quaternion.identity);
        newPieceObj.transform.SetParent(BoardPiecesParent.transform);

        PieceController newPieceController = newPieceObj.GetComponent<PieceController>();

        newPieceController.PieceObjType = type;
        newPieceController.PieceObjTeam = team;

        newPieceController.InitializePieceObj();

        BoardStateManager.Instance.CurrentBoardState.BoardPieces.Add(pos, newPieceController.piece);
    }

    
    public void MoveBoardPieceObj(PieceController pieceControllerToMove, BoardMove move)
    {
        Vector2 endPostion = move.PieceMove.Item2;

        BoardPieceObjects.Remove(pieceControllerToMove.piece.CurrentPiecePosition);

        pieceControllerToMove.MovePieceObj(endPostion);

        BoardPieceObjects.Add(endPostion, pieceControllerToMove);  
    }

    public void DestroyPieceObjAtPos(Vector2 pos)
    {
        Destroy(BoardPieceObjects[pos].gameObject);
        BoardPieceObjects.Remove(pos);
    }



    // public bool CheckLegalMove(Piece piece, Vector2 endPos)
    // {
    //     if (CheckLegalMovement(piece, endPos) == true) return true;
    //     if (CheckLegalAttack(piece, endPos) == true) return true;

    //     return false;
    // }


    // private bool CheckLegalMovement(Piece piece, Vector2 endPos)
    // {
    //     if (piece.GetCurrentMovements().Contains(endPos)) return true;

    //     return false;
    // }

    // private bool CheckLegalAttack(Piece piece, Vector2 endPos)
    // {
    //     if (piece.GetCurrentAttacks().Contains(endPos)) return true;

    //     return false;
    // }


    public bool CheckForPawnEnPasant(Piece pieceToCheck, Vector2 endPos, out Vector2 attackPosition)
    {
        attackPosition = default;

        if (pieceToCheck.PieceType != PieceTypes.Pawn) return false;

        PawnController pawnController = (PawnController)pieceToCheck.logic;
        if (pawnController.EnPasantEnemyMovementAttackPositions.Count == 0) return false;
        
        if (pawnController.EnPasantEnemyMovementAttackPositions.TryGetValue(endPos, out Vector2 possibleAttackPosition) == false) return false;

        attackPosition = possibleAttackPosition;
        pawnController.ClearEnPasantDict();

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
            outputPieces+= ", {" + piece.Value.CurrentPiecePosition.ToString() + ": " + piece.Value.PieceType.ToString() + "}";
        }

        foreach (KeyValuePair<Vector2, PieceController> piece in BoardPieceObjects)
        {
            outputControllers+= ", {" + piece.Key.ToString() + ": " + piece.Value.gameObject.name.ToString() + "}";
        }

        Debug.Log("Dictionary = [" + outputPieces + "]");
        Debug.Log("Dictionary = [" + outputControllers + "]");
    }
}
