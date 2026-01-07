using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PieceController : MonoBehaviour
{
    private SpriteRenderer sr;
    public PieceTypes PieceObjType;
    public Players PieceObjTeam;
    public Piece piece;


    public void InitializePieceObj()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = BoardDataManager.Instance.GetSpriteFromPieceType(PieceObjType);

        InitializePiece((Vector2)transform.position, (Vector2)transform.position, PieceObjType, PieceObjTeam, 0);

        RefreshPieceIdentity();
    }

    public void InitializePiece(Vector2 currentPos, Vector2 previousPos, PieceTypes pieceType, Players team, int currentTurnCount)
    {
        piece = new Piece();

        piece.CurrentTurnCount = currentTurnCount;

        piece.PieceType = pieceType;
        piece.PieceTeam = team;

        piece.logic = (piece.logic == null) ? BoardDataManager.Instance.GetLogicFromPieceType(PieceObjType) : piece.logic;   
        
        piece.CurrentPiecePosition = currentPos;
        piece.PreviousPiecePosition = previousPos;

        piece.logic.piece = piece; 
    }


    // private void BoardUpdated()
    // {
    //     ClearMoves();
    // }

    // private void ClearMoves()
    // {
    //     piece.CurrentMovements = new List<Vector2>();
    //     piece.CurrentAttacks = new List<Vector2>();
    // }

    public void MovePieceObj(Vector2 endPos)
    {    
        Vector2 previousPos = (Vector2)transform.position;
        transform.position = endPos;

        int currentTurnCount = BoardStateManager.Instance.CurrentBoardState.TurnCount;
        Debug.Log(piece.CurrentPiecePosition);
        InitializePiece(endPos, previousPos, PieceObjType, PieceObjTeam, currentTurnCount);
        Debug.Log("Initialize new piece current pos:" + piece.CurrentPiecePosition);
        RefreshPieceIdentity();
    }

    public void RefreshPieceIdentity()
    {
        if (sr == null) {sr = gameObject.GetComponent<SpriteRenderer>();}
        sr.color = (PieceObjTeam == Players.White) ? BoardDataManager.Instance.WhitePieceTeamColor : BoardDataManager.Instance.BlackPieceTeamColor;
    
        gameObject.name = piece.PieceTeam.ToString()+ " " + piece.PieceType + " at: (" + ((int)transform.position.x).ToString() + "," + ((int)transform.position.y).ToString() + ")";
    }

    //TODO move to board manager
    public void SpawnMarkers() 
    {
        BoardPiecesManager.Instance.ClearAllMarkers();

        List<Vector2> possibleMoves = piece.GetCurrentMovements();
        foreach (var location in possibleMoves)
        {
            GameObject newMarker = Instantiate(BoardDataManager.Instance.PossibleMovementMarkerPre, location, Quaternion.identity);
            BoardPiecesManager.Instance.Markers.Add(newMarker);
        }

        List<Vector2> possibleAttack = piece.GetCurrentAttacks();
        foreach (var location in possibleAttack)
        {
            GameObject newMarker = Instantiate(BoardDataManager.Instance.PossibleAttackMarkerPre, location, Quaternion.identity);
            BoardPiecesManager.Instance.Markers.Add(newMarker);
        }
    }
}
