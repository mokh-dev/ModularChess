using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class Card : ScriptableObject
{
    public string Name;
    public Sprite Art;
    public CardType Type;
}

public enum CardType
{
    Character,
    Modifier,
    Board,
    Action,
}