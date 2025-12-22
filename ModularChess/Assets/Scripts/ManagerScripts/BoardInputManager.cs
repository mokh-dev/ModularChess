using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BoardInputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private static BoardInputManager _instance;

    public static BoardInputManager Instance { get { return _instance; } }

    [SerializeField] private GameObject _selectedPiece;
    private Vector2 mouseDownPos;
    private Vector2 mouseUpPos;


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
        if (IsCorrectTeam(pieceToSelect) == false) return;

        _selectedPiece = pieceToSelect;
        _selectedPiece.GetComponent<BasePieceController>().SpawnMarkers();
    }

    private void UnselectPiece()
    {
        _selectedPiece = null;
        BoardStateManager.Instance.ClearAllMarkers();
    }

    private void PlayMove(Vector2 endPos)
    {
        BoardStateManager.Instance.MovePiece(_selectedPiece, endPos);
        BoardStateManager.Instance.EndPlayerTurn();

        UnselectPiece();
    }

    private bool IsCorrectTeam(GameObject piece)
    {
        if (BoardStateManager.Instance.CurrentTurn == piece.GetComponent<BasePieceController>().PieceTeam) return true;
        return false;
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
        
    }
}
