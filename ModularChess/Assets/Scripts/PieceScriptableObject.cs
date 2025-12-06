using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceScriptableObject", menuName = "Scriptable Objects/Piece")]
public class PieceScriptableObject : ScriptableObject
{
    [field: SerializeField] public string Name {get; private set;} = "PieceName";

    [field: SerializeField] public MovementPattern MovementPattern {get; private set;}

    //system for giving a piece a customizable movement pattern
}

[CreateAssetMenu(fileName = "MovementPattern", menuName = "Scriptable Objects/Movement Pattern")]
public abstract class MovementPattern : ScriptableObject
{
    public abstract List<Vector2> FindPossibleMoves(int boardSize, Vector2 currentPos, int team);
}