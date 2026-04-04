using UnityEngine;
using UnityEngine.EventSystems;


public class TableInputManager : Singleton<TableInputManager>
{
    public void OnClickCard(Card card, Teams cardSide)
    {
        switch (card.Type)
        {
            case CardType.Character:
                TableStateManager.Instance.PlayCharacterCardToField(card, cardSide);
                return;

            default:
                Debug.LogError("Clicked Card Doesnt have a valid Type");
                return;
        }
    }


    public void OnClickDeck() 
    {
        TableStateManager.Instance.DrawCardFromDeck();
    }
}
