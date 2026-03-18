using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class Card : ScriptableObject
{
    public string Title;
    public CardType Type;
    public Sprite Art;
    public Sprite Frame;
    public int Cost;
    public string Description;
}

public enum CardType
{
    Character,
    Modifier,
    Board,
    Action,
}