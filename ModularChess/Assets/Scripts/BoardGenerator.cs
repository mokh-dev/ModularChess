using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private float boardSize;
    [SerializeField] private float boxSize;

    [SerializeField] private Color lightSquareColor;
    [SerializeField] private Color darkSquareColor;

    [SerializeField] private Sprite squareSprite;
    


    void DrawSquare(Vector2 pos, Color color)
    {
        GameObject square = new GameObject("Square");
        square.transform.parent = gameObject.transform;

        square.transform.position = pos;
        square.transform.localScale = new Vector3(boxSize, boxSize, 1);


        SpriteRenderer squareSpriteRenderer = square.AddComponent<SpriteRenderer>();

        squareSpriteRenderer.color = color;
        squareSpriteRenderer.sprite = squareSprite;
    }

  
    public void DrawBoard()
    {
        int colorTracker = 0;

        for (int file = 0; file < boardSize; file++)
        {
            for (int rank = 0; rank < boardSize; rank++)
            {
                Vector2 drawPos = new Vector2(rank * boxSize, file * boxSize);

                if ((colorTracker + file) % 2 == 0)
                {
                    DrawSquare(drawPos, darkSquareColor);
                }
                else
                {
                    DrawSquare(drawPos, lightSquareColor);
                }

                colorTracker++;
            }
        }
    }

    public void EraseBoard() //Called in editor
    {
        int numChildren = transform.childCount;
        for( int i=numChildren-1 ; i>=0 ; i-- ) 
        {
            GameObject.DestroyImmediate( transform.GetChild(i).gameObject );
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
