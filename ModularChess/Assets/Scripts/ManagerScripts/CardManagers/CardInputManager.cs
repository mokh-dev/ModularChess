using UnityEngine;
using UnityEngine.EventSystems;


public class CardInputManager : Singleton<CardInputManager>
{
    public void OnClickCard(Card card)
    {
        PlayCardGA playCardGA = new()
        {
            CardToPlay = card
        };
        ActionSystem.Instance.Perform(playCardGA);
    }


    public void OnClickDeck() 
    {
        DrawCardGA drawCardGA = new();
        ActionSystem.Instance.Perform(drawCardGA);
    }
}
