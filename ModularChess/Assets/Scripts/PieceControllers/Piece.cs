using System.Collections.Generic;
using UnityEngine;

public class Piece
{
    public string Title;
    public PieceMovementType MovementType;
    public PieceAttackType AttackType;
    public Teams Team;
    public Sprite Art;
    public int Health;
    public int Damage;
    public int MovementRange;
    public int AttackingRange;
    readonly PieceData data;

    public Piece(PieceData pieceData)
    {
        Title = pieceData.Title;
        MovementType = pieceData.MovementType;
        AttackType = pieceData.AttackType;
        Team = pieceData.Team;
        Art = pieceData.Art;
        Health = pieceData.Health;
        MovementRange = pieceData.MovementRange;
        AttackingRange = pieceData.AttackingRange;
    }

    // //TODO change PieceMoveLogic to be a helper class that just returns lists
    // private PieceMoveLogic logic => BoardDataManager.Instance.GetPieceMoveLogic(Type);

    // public List<Vector2> GetMovements(Vector2 piecePosition, GameState gameState)
    // {  
    //     return logic.FindMovements(piecePosition, this, gameState);
    // }

    // public List<Vector2> GetAttacks(Vector2 piecePosition, GameState gameState)
    // {
    //     return logic.FindAttacks(piecePosition, this, gameState);
    // }
}
