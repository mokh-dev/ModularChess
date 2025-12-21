using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BoardInputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private static BoardInputManager _instance;

    public static BoardInputManager Instance { get { return _instance; } }

    [SerializeField ]private GameObject _selectedPiece;
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

    private void SelectPiece(GameObject pieceToSelect)
    {
        _selectedPiece = pieceToSelect;
        _selectedPiece.GetComponent<PieceController>().SpawnMarkers();
    }

    private void UnselectPiece()
    {
        _selectedPiece = null;
        BoardStateManager.Instance.ClearAllMarkers();
    }

    private void PlayMove(Vector2 endPos)
    {
        BoardStateManager.Instance.MovePiece(_selectedPiece, endPos);
        UnselectPiece();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        mouseDownPos = new Vector2(Mathf.RoundToInt(eventData.pointerPressRaycast.worldPosition.x), Mathf.RoundToInt(eventData.pointerPressRaycast.worldPosition.y));

        if (BoardStateManager.Instance.BoardGameObjects.TryGetValue(mouseDownPos, out GameObject piece) == true)
        {
            SelectPiece(piece);
            return;
        }


        if (_selectedPiece == null) return;

        if (BoardStateManager.Instance.CheckLegalMove(_selectedPiece, mouseDownPos) == true)
        {
            PlayMove(mouseDownPos);
            return;
        }

        UnselectPiece();
    }
        
    public void OnPointerUp(PointerEventData eventData)
    {
        mouseUpPos = new Vector2(Mathf.RoundToInt(eventData.pointerCurrentRaycast.worldPosition.x), Mathf.RoundToInt(eventData.pointerCurrentRaycast.worldPosition.y));

        if ( _selectedPiece == null) return;

        if (mouseUpPos == mouseDownPos) return;

        if (BoardStateManager.Instance.CheckLegalMove(_selectedPiece, mouseUpPos) == false)
        {
            UnselectPiece();
            return;
        }

        PlayMove(mouseUpPos);

        //


        // if (_selectedPiece != null)
        // {
        //     mouseUpPos = new Vector2(Mathf.RoundToInt(eventData.pointerCurrentRaycast.worldPosition.x), Mathf.RoundToInt(eventData.pointerCurrentRaycast.worldPosition.y));

        //     List<Vector2> possibleMoves = _selectedPiece.GetComponent<PieceController>().FindCurrentPossibleMoves(BoardStateManager.Instance.BoardGameObjects);

        //     if (possibleMoves.Contains(mouseUpPos))
        //     {
        //         if (oldMouseDownPos != new Vector2(-1,-1))
        //         {
        //             BoardStateManager.Instance.MovePiece(oldMouseDownPos, mouseUpPos); 
        //             // there's some bug here because old mouse down i should make a better input drag system
        //             oldMouseDownPos = new Vector2(-1,-1);        
        //         }
        //         else
        //         {
        //             BoardStateManager.Instance.MovePiece(mouseDownPos, mouseUpPos);
        //         }

        //         BoardStateManager.Instance.ClearPossibleMoveMarkers();
        //     }
        //     else
        //     {
        //         if (mouseUpPos != mouseDownPos)
        //         {
        //             BoardStateManager.Instance.ClearPossibleMoveMarkers();
        //         }
        //         else
        //         {
        //             oldMouseDownPos = mouseDownPos;
        //         }
        //     }
        // }
        
        // if (BoardStateManager.Instance.BoardGameObjects.TryGetValue(mouseDownPos, out GameObject piece) == false)
        // {
        //     BoardStateManager.Instance.ClearPossibleMoveMarkers();
        // }
        
    }
}
