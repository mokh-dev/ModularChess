using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PositionObject
{
    public Vector2 pos;
    public GameObject obj;
}

public class BoardStateManager : MonoBehaviour
{
    private static BoardStateManager _instance;

    public static BoardStateManager Instance { get { return _instance; } }

    public List<PositionObject> BoardState { get; private set;} = new List<PositionObject>();

    [SerializeField] private int boardSize;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        InitializeBoardDictionary();
        Debug.Log(BoardState);
    }

    private void InitializeBoardDictionary()
    {
        for (int i  = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                PositionObject newObj = new PositionObject();
                newObj.pos = new Vector2(i,j);
                newObj.obj = null;

                BoardState.Add(newObj);
            }
        }
    }


    private void GetAllBoardPieces()
    {
        // for (int i = 0; i < length; i++)
        // {
            
        // }
    }
}
