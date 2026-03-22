using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Piece", menuName = "Scriptable Objects/Piece")]
public class Piece : ScriptableObject
{
    public PieceLogicType Type;
    public Players Team;
    public Sprite Art;
    public int Health;
    public int MaxHealth;

    private PieceMoveLogic logic => BoardDataManager.Instance.GetPieceMoveLogic(Type);

    public List<Vector2> GetMovements(Vector2 piecePosition, GameState gameState)
    {  
        return logic.FindMovements(piecePosition, this, gameState);
    }

    public List<Vector2> GetAttacks(Vector2 piecePosition, GameState gameState)
    {
        return logic.FindAttacks(piecePosition, this, gameState);
    }
}
