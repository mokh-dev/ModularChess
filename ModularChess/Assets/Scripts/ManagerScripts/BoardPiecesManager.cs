using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class BoardPiecesManager : MonoBehaviour
{
    private static BoardPiecesManager instance;
    public static BoardPiecesManager Instance { get { return instance; } }


    public Dictionary<Vector2, PieceController> BoardPieces { get; private set;} = new Dictionary<Vector2, PieceController>();
    public bool WhiteInCheck {get; private set;} 
    public bool BlackInCheck {get; private set;} 
    public List<GameObject> Markers = new List<GameObject>();

    public UnityEvent ResetLastMove;


    [SerializeField] private GameObject _boardPiecesParent;

    [Header("---Test---")]
    [SerializeField] private GameObject testPiecePre;
    [SerializeField] private Vector2 _testPiecePos;
    [SerializeField] private Players _testPieceTeam;



    private PieceController whiteKing;
    private PieceController blackKing;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }


        LoadBoardToDict(); 

        whiteKing = FindKingOfTeam(Players.White);
        blackKing = FindKingOfTeam(Players.Black);
    }

    void Start()
    {
        BoardStateManager.Instance.BoardUpdate.AddListener(BoardUpdated);
    }

    private void BoardUpdated()
    {
        WhiteInCheck = IsInCheck(Players.White);
        BlackInCheck = IsInCheck(Players.Black);
    }

    private bool IsInCheck(Players teamToCheck)
    {
        PieceController kingPiece = (teamToCheck == Players.White) ? whiteKing : blackKing;
        Vector2 kingPosition = kingPiece.CurrentPiecePosition;

        Players enemyTeam = (teamToCheck == Players.White) ? Players.Black : Players.White;
        List<Vector2> enemyAttackPositions = GetAllAttacksFromTeam(enemyTeam);

        foreach (Vector2 attackPos in enemyAttackPositions)
        {
            if (attackPos == kingPosition)
            {
                Debug.Log(teamToCheck.ToString() + " is in check");
                return true;
            } 
        }

        return false;
    }

    private List<Vector2> GetAllAttacksFromTeam(Players team)
    {
        List<Vector2> totalAttacks = new List<Vector2>();

        foreach (KeyValuePair<Vector2, PieceController> boardObj in BoardPieces)
        {
            PieceController objPieceController = boardObj.Value;
            if (objPieceController.PieceTeam != team) continue;

            List<Vector2> pieceAttacks = objPieceController.GetCurrentAttacks();
            totalAttacks.AddRange(pieceAttacks);
        } 

        return totalAttacks;
    }

    private PieceController FindKingOfTeam(Players kingTeam)
    {
        PieceController king = null;

        foreach (KeyValuePair<Vector2, PieceController> boardObj in BoardPieces)
        {
            if (boardObj.Value.PieceType != Pieces.King) continue;
            if (boardObj.Value.PieceTeam != kingTeam) continue;

            king = boardObj.Value;
            Debug.Log("Found king" + kingTeam);
        }

        return king;
    }

    private void LoadBoardToDict()
    {
        foreach (var gameObjectTransform in _boardPiecesParent.GetComponentsInChildren<Transform>())
        {
            if (gameObjectTransform == _boardPiecesParent.transform) continue;
            BoardPieces.Add((Vector2)gameObjectTransform.position, gameObjectTransform.gameObject.GetComponent<PieceController>());
        }
    }


    public void AddTestPiece()
    {
        AddNewPiece(testPiecePre, _testPiecePos, _testPieceTeam);
    }


    public void AddNewPiece(GameObject piecePre, Vector2 pos, Players team)
    {
        GameObject newPieceObj = Instantiate(piecePre, pos, Quaternion.identity);
        newPieceObj.transform.SetParent(_boardPiecesParent.transform);

        PieceController newPieceController = newPieceObj.GetComponent<PieceController>();

        newPieceController.PieceTeam = team;
        newPieceController.RefreshPieceIdentity();

        BoardPieces.Add(pos, newPieceController);
    }

    public void AttackPieceAtPos(Vector2 pos)
    {
        Destroy(BoardPieces[pos].gameObject);
        BoardPieces.Remove(pos);
    }

    public void MoveBoardPiece(PieceController pieceToMove, Vector2 endPos)
    {
        if (CheckLegalAttack(pieceToMove, endPos) == true) 
        {
            if (CheckForNonCaptureAttack(pieceToMove, endPos, out Vector2 attackPosition) == true)
            {
                AttackPieceAtPos(attackPosition);
            }
            else
            {
                AttackPieceAtPos(endPos);
            }
        }
        else
        {
            if (CheckLegalMovement(pieceToMove, endPos) == false) return;
        }

        ResetLastMove.Invoke();

        BoardPieces.Remove((Vector2)pieceToMove.transform.position);

        pieceToMove.MovePiece(endPos);

        BoardPieces.Add(endPos, pieceToMove);

        BoardStateManager.Instance.EndPlayerTurn();
        
    }

     

    public bool CheckLegalMove(PieceController piece, Vector2 endPos)
    {
        if (CheckLegalMovement(piece, endPos) == true) return true;
        if (CheckLegalAttack(piece, endPos) == true) return true;

        return false;
    }


    private bool CheckLegalMovement(PieceController piece, Vector2 endPos)
    {
        if (piece.GetCurrentMovements().Contains(endPos)) return true;

        return false;
    }

    private bool CheckLegalAttack(PieceController piece, Vector2 endPos)
    {
        if (piece.GetCurrentAttacks().Contains(endPos)) return true;

        return false;
    }

    private bool CheckForNonCaptureAttack(PieceController pieceToCheck, Vector2 endPos, out Vector2 attackPosition)
    {
        attackPosition = default;

        if (HandlePawnEnPasant(pieceToCheck, endPos, out Vector2 pawnAttackPosition) == true)
        {
            attackPosition = pawnAttackPosition;
            return true;
        }

        return false;
    }
    private bool HandlePawnEnPasant(PieceController pieceToCheck, Vector2 endPos, out Vector2 attackPosition)
    {
        attackPosition = default;

        if (pieceToCheck.PieceType != Pieces.Pawn) return false;

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
        string output = "";

        foreach (KeyValuePair<Vector2, PieceController> item in BoardPieces)
        {
            output+= ", {" + item.Key.ToString() + ": " + item.Value.name.ToString() + "}";
        }

        Debug.Log("Dictionary = [" + output + "]");
    }
}
