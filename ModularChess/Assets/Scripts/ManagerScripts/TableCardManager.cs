using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TableCardManager : MonoBehaviour
{
    private static TableCardManager instance;
    public static TableCardManager Instance { get { return instance; } }

    [SerializeField] private GameObject _emptyCard;
    [SerializeField] private GameObject _playerHandGroup;
    [SerializeField] private GameObject _playerFieldGroup;
    [SerializeField] private GameObject _enemyHandGroup;
    [SerializeField] private GameObject _enemyFieldGroup;
    [SerializeField] private Transform _discardPileCenterPosition;




    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }     
    
    public void DisplayState(TableState tableState)
    {
        DeleteOldCards(_playerHandGroup);
        DeleteOldCards(_playerFieldGroup);
        DeleteOldCards(_enemyHandGroup);
        DeleteOldCards(_enemyFieldGroup);


        GenerateCards(tableState.PlayerSide.Hand, _playerHandGroup);
        GenerateCards(tableState.PlayerSide.Field, _playerFieldGroup);

        GenerateCards(tableState.EnemySide.Hand, _enemyHandGroup);
        GenerateCards(tableState.EnemySide.Field, _enemyFieldGroup);
    }

    private void DeleteOldCards(GameObject parentObj)
    {
        foreach (Transform child in parentObj.transform)
        {
            Destroy(child);
        }
    }

    private void GenerateCards(List<Card> cards, GameObject parentObj)
    {
        foreach (Card card in cards)
        {
            GameObject newCard = Instantiate(_emptyCard);
            
            newCard.GetComponent<CardBuilder>().CardData = card;
            newCard.transform.SetParent(parentObj.transform);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
