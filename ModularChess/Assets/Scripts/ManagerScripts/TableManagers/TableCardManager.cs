using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TableCardManager : Singleton<TableCardManager>
{
    [SerializeField] private GameObject _emptyCard;
    [SerializeField] private GameObject _playerHandGroup;
    [SerializeField] private GameObject _playerFieldGroup;
    [SerializeField] private Transform _discardPilePosition;


    public void DisplayTableState(TableState tableState)
    {

        ClearTable();

        GenerateCards(tableState.PlayerSide.Hand, _playerHandGroup, Teams.Player);
        GenerateCards(tableState.PlayerSide.Field, _playerFieldGroup, Teams.Player);
    }

    private void ClearTable()
    {
        DeleteCardObjectsFromParent(_playerHandGroup);
        DeleteCardObjectsFromParent(_playerFieldGroup);
    }

    private void DeleteCardObjectsFromParent(GameObject parentObj)
    {
        foreach (Transform child in parentObj.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void GenerateCards(List<Card> cards, GameObject parentObj, Teams cardOwnerTeam)
    {
        foreach (Card card in cards)
        {
            GameObject newCard = Instantiate(_emptyCard, parentObj.transform);
            
            CardBuilder cardBuilder = newCard.GetComponent<CardBuilder>();
            cardBuilder.CardData = card;
            cardBuilder.CardOwnerTeam = cardOwnerTeam;
        }
    }
}
