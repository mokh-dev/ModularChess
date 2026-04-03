using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    [field: SerializeField] public string Title {get; private set;}
    [field: SerializeField] public CardType Type {get; private set;}
    [field: SerializeField] public Sprite Image {get; private set;}
    [field: SerializeField] public Sprite Frame {get; private set;}
    [field: SerializeField] public int Cost {get; private set;}
    [field: SerializeField] public string Description {get; private set;}

    [field: SerializeField] public PieceData CharacterPieceData {get; private set;}
}