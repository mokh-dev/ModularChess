using UnityEngine;
using UnityEngine.EventSystems;

public class BoardInputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField] private GameObject _selectedPiece;
    private Vector2 mouseDownPos;
    private Vector2 mouseUpPos;

    private void SelectPiece(GameObject pieceToSelect)
    {
        if (IsCorrectTeam(pieceToSelect) == false) return;

        _selectedPiece = pieceToSelect;
        BoardPiecesManager.Instance.SpawnMarkersForPieceObj(pieceToSelect.GetComponent<PieceController>());
    }

    private void UnselectPiece()
    {
        _selectedPiece = null;
        BoardPiecesManager.Instance.ClearAllMarkers();
    }

    private bool IsCorrectTeam(GameObject piece)
    {
        if (GameStateManager.Instance.CurrentPlayerTurn == piece.GetComponent<PieceController>().ControlledPiece.PieceTeam) return true;
        return false;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        mouseDownPos = new Vector2(Mathf.RoundToInt(eventData.pointerPressRaycast.worldPosition.x), Mathf.RoundToInt(eventData.pointerPressRaycast.worldPosition.y));

        if (BoardPiecesManager.Instance.BoardPieceObjects.TryGetValue(mouseDownPos, out PieceController pieceController) == true)
        {
            GameObject pieceObj = pieceController.gameObject;
            if (IsCorrectTeam(pieceObj) == true)
            {
                SelectPiece(pieceObj);
                return;
            }
        }

        if (_selectedPiece == null) return;

        SendTryBoardMove(mouseDownPos);
        UnselectPiece();
    }
        
    public void OnPointerUp(PointerEventData eventData)
    {
        mouseUpPos = new Vector2(Mathf.RoundToInt(eventData.pointerCurrentRaycast.worldPosition.x), Mathf.RoundToInt(eventData.pointerCurrentRaycast.worldPosition.y));

        if ( _selectedPiece == null) return;

        if (mouseUpPos == mouseDownPos) return;

        SendTryBoardMove(mouseUpPos);
        UnselectPiece();
    }

    private bool SendTryBoardMove(Vector2 endPos)
    {
        BoardMove boardMove = new BoardMove();
        boardMove.PieceMove = ((Vector2)_selectedPiece.transform.position, endPos);
        
        if (BoardStateManager.Instance.TryPlayBoardMove(BoardStateManager.Instance.CurrentBoardState, boardMove) == false) return false;

        return true;
    }
}
