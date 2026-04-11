using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestItem : MonoBehaviour
{

    public int TestItemComparingValue;
    [SerializeField] private GameObject _itemIndicator;
    [SerializeField] private TextMeshProUGUI _itemText;
    private bool isItemActive;

    public void ChangeComparisonValue(int change)
    {
        TestItemComparingValue += change;
        UpdateItemText();
    }

    private void UpdateItemText()
    {
        _itemText.text = $"Draw a Card if you move a Piece {TestItemComparingValue} or more";
    }

    public void ToggleItem()
    {
        isItemActive = !isItemActive;

        if (isItemActive == true)
        {
            _itemIndicator.SetActive(true);
            ActionSystem.SubscribeReaction<MovePieceGA>(MovePieceReaction, ReactionTiming.POST);
            
            UpdateItemText();
        }
        else
        {
            ActionSystem.UnsubscribeReaction<MovePieceGA>(MovePieceReaction, ReactionTiming.POST);
            _itemIndicator.SetActive(false);

        }
    }

    private void MovePieceReaction(MovePieceGA movePieceGA)
    {
        Vector2 initialPos = movePieceGA.BoardMove.Item1;
        Vector2 endPos = movePieceGA.BoardMove.Item2;

        if (Vector2.Distance(initialPos, endPos) < TestItemComparingValue) return;

        DrawCardGA drawCardGA = new();
        ActionSystem.Instance.AddReaction(drawCardGA);

    }
}
