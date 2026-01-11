using System.Collections.Generic;
using UnityEngine;


public class PieceController : MonoBehaviour
{
    private SpriteRenderer sr;
    public PieceTypes PieceObjType;
    public Players PieceObjTeam;
    public Piece ControlledPiece => BoardStateManager.Instance.CurrentBoardState.BoardPieces[(Vector2)transform.position];


    public void InitializePieceObj()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = BoardDataManager.Instance.GetSpriteFromPieceType(PieceObjType);
    }

    public Piece GetInitialPiece()
    {
        Piece piece = new Piece();

        piece.TurnCount = 0;

        piece.PieceType = PieceObjType;
        piece.PieceTeam = PieceObjTeam;

        piece.logic = (piece.logic == null) ? BoardDataManager.Instance.GetLogicFromPieceType(PieceObjType) : piece.logic;   
        
        piece.PiecePosition = (Vector2)transform.position;
        piece.PreviousPiecePositions = new Dictionary<int, Vector2>();

        return piece;
    }


    public void MovePieceObj(Vector2 endPos)
    {    
        Vector2 previousPos = (Vector2)transform.position;
        transform.position = endPos;

        RefreshPieceIdentity();
    }

    public void RefreshPieceIdentity()
    {
        if (sr == null) {sr = gameObject.GetComponent<SpriteRenderer>();}
        sr.color = (PieceObjTeam == Players.White) ? BoardDataManager.Instance.WhitePieceTeamColor : BoardDataManager.Instance.BlackPieceTeamColor;
    
        gameObject.name = ControlledPiece.PieceTeam.ToString()+ " " + ControlledPiece.PieceType + " at: (" + ((int)transform.position.x).ToString() + "," + ((int)transform.position.y).ToString() + ")";
    }

    //TODO move to board manager
    public void SpawnMarkers() 
    {
        BoardPiecesManager.Instance.ClearAllMarkers();

        List<Vector2> possibleMoves = ControlledPiece.GetMovements();
        foreach (var location in possibleMoves)
        {
            GameObject newMarker = Instantiate(BoardDataManager.Instance.PossibleMovementMarkerPre, location, Quaternion.identity);
            BoardPiecesManager.Instance.Markers.Add(newMarker);
        }

        List<Vector2> possibleAttack = ControlledPiece.GetAttacks();
        foreach (var location in possibleAttack)
        {
            GameObject newMarker = Instantiate(BoardDataManager.Instance.PossibleAttackMarkerPre, location, Quaternion.identity);
            BoardPiecesManager.Instance.Markers.Add(newMarker);
        }
    }
}
