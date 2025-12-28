using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.Events;


public class BoardStateManager : MonoBehaviour
{
    private static BoardStateManager instance;

    public static BoardStateManager Instance { get { return instance; } }


    public Dictionary<Vector2, GameObject> BoardGameObjects { get; private set;} = new Dictionary<Vector2, GameObject>();
    public Players CurrentTurn {get; private set;} 
    public bool WhiteInCheck {get; private set;} 
    public bool BlackInCheck {get; private set;} 


    public UnityEvent BoardUpdate;
    public UnityEvent ResetLastMove;

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
        newPiece.GetComponent<PieceController>().RefreshPieceIdentity();

        BoardGameObjects.Add(pos, newPiece);
    }

    public void AttackPieceAtPos(Vector2 pos)
    {
        Destroy(BoardGameObjects[pos]);
        BoardGameObjects.Remove(pos);
    }

    public void MoveBoardPiece(GameObject pieceToMove, Vector2 endPos)
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

        BoardGameObjects.Remove((Vector2)pieceToMove.transform.position);

        pieceToMove.GetComponent<PieceController>().MovePiece(endPos);

        BoardGameObjects.Add(endPos, pieceToMove);

        EndPlayerTurn();
        BoardUpdate.Invoke();
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

        List<Vector2> possibleMovements = currentPieceController.GetCurrentMovements();

        if (possibleMovements.Contains(endPos)) return true;

        return false;
    }

    private bool CheckLegalAttack(GameObject piece, Vector2 endPos)
    {
        PieceController currentPieceController = piece.GetComponent<PieceController>();

        List<Vector2> possibleAttacks = currentPieceController.GetCurrentAttacks();

        if (possibleAttacks.Contains(endPos)) return true;

        return false;
    }

    private bool CheckForNonCaptureAttack(GameObject pieceToCheck, Vector2 endPos, out Vector2 attackPosition)
    {
        attackPosition = default;

        if (HandlePawnEnPasant(pieceToCheck, endPos, out Vector2 pawnAttackPosition) == true)
        {
            attackPosition = pawnAttackPosition;
            return true;
        }

        return false;
    }
    private bool HandlePawnEnPasant(GameObject pieceToCheck, Vector2 endPos, out Vector2 attackPosition)
    {
        attackPosition = default;

        if (pieceToCheck.TryGetComponent(out IAttack component) == false) return false;
        if (component is not PawnController) return false;

        PawnController pawnController = pieceToCheck.GetComponent<PawnController>();
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

public enum Pieces
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King,
}
