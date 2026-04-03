using UnityEngine;

public class Card
{
    public string Title {get; private set;}
    public CardType Type {get; private set;}
    public Sprite Image {get; private set;}
    public Sprite Frame {get; private set;}
    public int Cost {get; private set;}
    public string Description {get; private set;}

    public PieceData CharacterPieceData {get; private set;}


    readonly CardData data;
    public Card(CardData cardData)
    {
        data = cardData;
        Title = cardData.Title;
        Type = cardData.Type;
        Image = cardData.Image;
        Frame = cardData.Frame;
        Cost = cardData.Cost;
        Description = cardData.Description;
        CharacterPieceData = cardData.CharacterPieceData;
    }
}
//TODO make each card type into a separate SO that inherits from Card

public enum CardType
{
    Character,
    Modifier,
    Board,
    Action,
}