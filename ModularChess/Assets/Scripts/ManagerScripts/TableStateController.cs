using System.Collections.Generic;
using UnityEngine;

public class TableStateController : MonoBehaviour
{



    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public struct TableState
    {
        TableSide PlayerSide;
        TableSide EnemySide;

        List<Card> DiscardPile;
    }



    public struct TableSide
    {
        List<Card> Hand;
        List<Card> Field;
        List<Card> Deck;
    }

}
