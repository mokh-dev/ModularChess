using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BoardStateManager : MonoBehaviour
{
    private static BoardStateManager instance;

    public static BoardStateManager Instance { get { return instance; } }


    
    public Players CurrentTurn {get; private set;} 



    public UnityEvent BoardUpdate;


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }


        BoardUpdate.AddListener(BoardUpdated);
    }

    void Start()
    {
        StartGame();
    }


    private void BoardUpdated()
    {
        
    }

    private void StartGame()
    {
        CurrentTurn = Players.White;
    }
    
    public void EndPlayerTurn()
    {
        BoardUpdate.Invoke();
        CurrentTurn = (CurrentTurn == Players.White) ? Players.Black : Players.White;
    }


}

public enum Players
{
    White,
    Black,
}

public enum Pieces
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King,
}
