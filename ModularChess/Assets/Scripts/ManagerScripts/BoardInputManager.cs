using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BoardInputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private static BoardInputManager _instance;

    public static BoardInputManager Instance { get { return _instance; } }

    private GameObject _selectedPiece;
    private Vector2 oldMouseDownPos = new Vector2(-1,-1); //means not in use

    private Vector2 mouseDownPos;
    private Vector2 mouseUpPos;
    private int intMouseDownPos;


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

        mouseDownPos = new Vector2(Mathf.RoundToInt(eventData.pointerPressRaycast.worldPosition.x), Mathf.RoundToInt(eventData.pointerPressRaycast.worldPosition.y));
        intMouseDownPos = BoardHelper.ConvertVector2PosToIntPos(mouseDownPos);

        if (BoardStateManager.Instance.BoardGameObjects[intMouseDownPos] != null)
        {
            _selectedPiece = BoardStateManager.Instance.BoardGameObjects[intMouseDownPos];
            _selectedPiece.GetComponent<PieceController>().SpawnPossibleMoveMarkers();
            
        }
    }
        
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_selectedPiece != null)
        {
            mouseUpPos = new Vector2(Mathf.RoundToInt(eventData.pointerCurrentRaycast.worldPosition.x), Mathf.RoundToInt(eventData.pointerCurrentRaycast.worldPosition.y));

            List<Vector2> possibleMoves = _selectedPiece.GetComponent<PieceController>().FindCurrentPossibleMoves();

            if (possibleMoves.Contains(mouseUpPos))
            {
                if (oldMouseDownPos != new Vector2(-1,-1))
                {
                    BoardStateManager.Instance.MovePiece(oldMouseDownPos, mouseUpPos);
                    oldMouseDownPos = new Vector2(-1,-1);        
                }
                else
                {
                    BoardStateManager.Instance.MovePiece(mouseDownPos, mouseUpPos);
                }

                BoardStateManager.Instance.ClearPossibleMoveMarkers();
            }
            else
            {
                if (mouseUpPos != mouseDownPos)
                {
                    BoardStateManager.Instance.ClearPossibleMoveMarkers();
                }
                else
                {
                    oldMouseDownPos = mouseDownPos;
                }
            }
        }
        
        if (BoardStateManager.Instance.BoardGameObjects[intMouseDownPos] == null)
        {
            BoardStateManager.Instance.ClearPossibleMoveMarkers();
        }
        
    }
}
