using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BoardInputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{

    private static BoardInputManager _instance;

    public static BoardInputManager Instance { get { return _instance; } }

    private GameObject _selectedPiece;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 mouseDownPos = new Vector2(Mathf.RoundToInt(eventData.pointerPressRaycast.worldPosition.x), Mathf.RoundToInt(eventData.pointerPressRaycast.worldPosition.y));
        int intMouseDownPos = BoardHelper.ConvertVector2PosToIntPos(mouseDownPos);

        if (BoardStateManager.Instance.BoardGameObjects[intMouseDownPos] != null)
        {
            Debug.Log("clicked piece");
            _selectedPiece = BoardStateManager.Instance.BoardGameObjects[intMouseDownPos];
            _selectedPiece.GetComponent<PieceController>().SpawnPossibleMoveMarkers();
            
        }
        else
        {
            Debug.Log("Empty");
            _selectedPiece = null;
        }
    }
        
    public void OnPointerUp(PointerEventData eventData)
    {
        // Debug.Log("Pointer Up");
        if (_selectedPiece != null)
        {
            Vector2 mouseUpPos = new Vector2(Mathf.RoundToInt(eventData.pointerCurrentRaycast.worldPosition.x), Mathf.RoundToInt(eventData.pointerCurrentRaycast.worldPosition.y));

            List<Vector2> possibleMoves = _selectedPiece.GetComponent<PieceController>().FindCurrentPossibleMoves();

            if (possibleMoves.Contains(mouseUpPos))
            {
                Debug.Log("make move");
            }
            else
            {
                Debug.Log("Invalid Move");
            }
        }
        

        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 mouseDownPos = new Vector2(Mathf.RoundToInt(eventData.pointerPressRaycast.worldPosition.x), Mathf.RoundToInt(eventData.pointerPressRaycast.worldPosition.y));
        Vector2 mouseUpPos = new Vector2(Mathf.RoundToInt(eventData.pointerCurrentRaycast.worldPosition.x), Mathf.RoundToInt(eventData.pointerCurrentRaycast.worldPosition.y));

        // if (mouseDownPos == mouseUpPos)
        // {
        //     Debug.Log("Click");
        // }
        // else
        // {
        //     Debug.Log("Drag");
        // }

    }
}
