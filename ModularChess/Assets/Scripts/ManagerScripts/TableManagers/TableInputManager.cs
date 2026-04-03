using UnityEngine;
using UnityEngine.EventSystems;


public class TableInputManager : MonoBehaviour
{
    private static TableInputManager instance;
    public static TableInputManager Instance { get { return instance; } }



    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

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
