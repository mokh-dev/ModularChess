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
    [SerializeField] private Transform _discardPilePosition;




    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }     
    
    public void DisplayTableState(TableState tableState)
    {

        ClearTable();

        GenerateCards(tableState.PlayerSide.Hand, _playerHandGroup);
        GenerateCards(tableState.PlayerSide.Field, _playerFieldGroup);

        GenerateCards(tableState.EnemySide.Hand, _enemyHandGroup);
        GenerateCards(tableState.EnemySide.Field, _enemyFieldGroup);
    }

    private void ClearTable()
    {
        DeleteCardObjectsFromParent(_playerHandGroup);
        DeleteCardObjectsFromParent(_playerFieldGroup);
        DeleteCardObjectsFromParent(_enemyHandGroup);
        DeleteCardObjectsFromParent(_enemyFieldGroup);
    }

    private void DeleteCardObjectsFromParent(GameObject parentObj)
    {
        foreach (Transform child in parentObj.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void GenerateCards(List<Card> cards, GameObject parentObj)
    {
        foreach (Card card in cards)
        {
            GameObject newCard = Instantiate(_emptyCard, parentObj.transform);
            
            newCard.GetComponent<CardBuilder>().CardData = card;
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
