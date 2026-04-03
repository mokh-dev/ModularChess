using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PieceBuilder : MonoBehaviour
{
    public Piece ControlledPiece {get; private set;}
    public Vector2 PiecePosition {get; private set;}
    private SpriteRenderer sr;

    [SerializeField] private Teams _pieceSide;

    public void InitializePieceObj(Vector2 position, Piece piece)
    {
        ControlledPiece = piece;
        PiecePosition = position;

        _pieceSide = piece.Team;

        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = ControlledPiece.Art;

        sr.color = (piece.Team == Teams.Player) ? Color.white : Color.black;
    }
}
