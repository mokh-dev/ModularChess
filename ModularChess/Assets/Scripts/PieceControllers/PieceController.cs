using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceController : MonoBehaviour
{

    
    public Pieces PieceType;

    [HideInInspector] public Players PieceTeam;
    [HideInInspector] public PieceMoveLogic logic;
    [HideInInspector] public float PieceBaseValue;
    [HideInInspector] public float PieceOverallValue;
    [HideInInspector] public bool movedLastTurn;

    [HideInInspector] public List<Vector2> CurrentMovements;
    [HideInInspector] public List<Vector2> CurrentAttacks;

    public int MoveDir {get; private set;}
    public Vector2 CurrentPiecePosition {get; private set;}
    public Vector2 PreviousPiecePosition {get; private set;}

    private SpriteRenderer sr;




    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        RefreshPieceIdentity();

        logic = (logic == null) ? GetLogicFromPieceType(PieceType) : logic;   
        logic.pieceController = this;

        MoveDir = (PieceTeam == Players.White) ? 1 : -1;

        PreviousPiecePosition = (Vector2)transform.position;
        CurrentPiecePosition = (Vector2)transform.position;

        BoardPiecesManager.Instance.ResetLastMove.AddListener(ResetLastMove);
        BoardStateManager.Instance.BoardUpdate.AddListener(BoardUpdated);
    }
    private PieceMoveLogic GetLogicFromPieceType(Pieces pieceType)
    {
        switch (pieceType)
        {
            case Pieces.Pawn:
                PieceBaseValue = 1;
                return new PawnController();
            
            case Pieces.Knight:
                PieceBaseValue = 3;
                return new KnightController();    

            case Pieces.Bishop:
                PieceBaseValue = 3;
                return new BishopController();     

            case Pieces.Rook:
                PieceBaseValue = 5;
                return new RookController(); 

            case Pieces.Queen:
                PieceBaseValue = 9;
                return new QueenController();  

            case Pieces.King:
                PieceBaseValue = Mathf.Infinity;
                return new KingController();        
            
            default:
                return null;
        }
    }

    private void BoardUpdated()
    {
        ClearMoves();
    }

    private void ClearMoves()
    {
        CurrentMovements = new List<Vector2>();
        CurrentAttacks = new List<Vector2>();
    }

    public void ResetLastMove()
    {
        movedLastTurn = false;
    }

    public void MovePiece(Vector2 endPos)
    {
        PreviousPiecePosition = (Vector2)transform.position;
        movedLastTurn = true;

        transform.position = endPos;
        CurrentPiecePosition = (Vector2)transform.position;

        RefreshPieceIdentity();
    }

    public void RefreshPieceIdentity()
    {
        if (sr == null) {sr = gameObject.GetComponent<SpriteRenderer>();}
        sr.color = (PieceTeam == Players.White) ? BoardDataManager.Instance.WhitePieceTeamColor : BoardDataManager.Instance.BlackPieceTeamColor;
    
        gameObject.name = PieceTeam.ToString()+ " " + PieceType + " at: (" + ((int)transform.position.x).ToString() + "," + ((int)transform.position.y).ToString() + ")";
    }


    public List<Vector2> GetCurrentMovements()
    {
        if ((CurrentMovements == null) || (CurrentMovements.Count == 0)) {CurrentMovements = logic.FindMovements();}
        return CurrentMovements;
    }

    public List<Vector2> GetCurrentAttacks()
    {
        if ((CurrentAttacks == null) || (CurrentAttacks.Count == 0)) {CurrentAttacks = logic.FindAttacks();}
        return CurrentAttacks;
    }

    //TODO move to board manager
    public void SpawnMarkers() 
    {
        BoardPiecesManager.Instance.ClearAllMarkers();

        List<Vector2> possibleMoves = GetCurrentMovements();
        foreach (var location in possibleMoves)
        {
            GameObject newMarker = Instantiate(BoardDataManager.Instance.PossibleMovementMarkerPre, location, Quaternion.identity);
            BoardPiecesManager.Instance.Markers.Add(newMarker);
        }

        List<Vector2> possibleAttack = GetCurrentAttacks();
        foreach (var location in possibleAttack)
        {
            GameObject newMarker = Instantiate(BoardDataManager.Instance.PossibleAttackMarkerPre, location, Quaternion.identity);
            BoardPiecesManager.Instance.Markers.Add(newMarker);
        }
    }




}
