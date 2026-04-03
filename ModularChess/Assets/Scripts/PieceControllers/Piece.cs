using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Piece", menuName = "Scriptable Objects/Piece")]
public class Piece : ScriptableObject //TODO make all piece data into a struct
{
    public PieceLogicType Type;
    public Teams Team;
    public Sprite Art;
    public int Health;
    public int MaxHealth;

    //TODO change PieceMoveLogic to be a helper class that just returns lists
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
