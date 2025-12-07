using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceScriptableObject", menuName = "Scriptable Objects/Piece")]
public class PieceScriptableObject : ScriptableObject
{
    [field: SerializeField] public string PieceName {get; private set;} = "Piece Name";
    [field: SerializeField] public Sprite PieceSprite {get; private set;}

    [field: SerializeField] public MovementPattern PieceMovementPattern {get; private set;}

    //system for giving a piece a customizable movement pattern
}

[CreateAssetMenu(fileName = "MovementPattern", menuName = "Scriptable Objects/Movement Pattern")]
public abstract class MovementPattern : ScriptableObject
{
    public abstract List<Vector2> FindPossibleMoves(GameObject[] boardPieces, Vector2 currentPos, int team);
}