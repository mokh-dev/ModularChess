using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardBuilder : MonoBehaviour
{
    public Card CardData;

    [Header("Child References")]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _typeText;
    [SerializeField] private TextMeshProUGUI _energyCostText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Image _mainArtImage;
    [SerializeField] private Image _frameImage;

    private Button cardButton;

    void Start()
    {
        cardButton = gameObject.GetComponent<Button>();
        cardButton.onClick.AddListener(ClickedCard);

        if (CardData != null)
        {
            _titleText.text = CardData.Title;
            _typeText.text = CardData.Type.ToString();
            _energyCostText.text = CardData.Cost.ToString();
            _descriptionText.text = CardData.Description;

            _mainArtImage.sprite = CardData.Art;
            _frameImage.sprite = CardData.Frame;
        }
    }

    private void ClickedCard()
    {
        TableInputManager.Instance.OnClickCard(CardData);
    }
    
}
