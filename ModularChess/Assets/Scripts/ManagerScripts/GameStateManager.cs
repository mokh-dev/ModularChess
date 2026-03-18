using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager instance;
    public static GameStateManager Instance { get { return instance; } }

    public Players CurrentPlayerTurn;

    [SerializeField] private RawImage _turnIndicatior;


    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndPlayerTurn() //UI Button
    {
        CurrentPlayerTurn = (CurrentPlayerTurn == Players.White) ? Players.Black : Players.White;
        _turnIndicatior.color = (CurrentPlayerTurn == Players.White) ? Color.white : Color.black;
    }
}
