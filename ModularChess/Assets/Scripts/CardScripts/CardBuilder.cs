using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardBuilder : MonoBehaviour
{
    public Card CardRef;

    [Header("Child References")]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _typeText;

    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Image _mainArtImage;
    [SerializeField] private Image _frameImage;

    private Button cardButton;


    void Start()
    {
        cardButton = gameObject.GetComponent<Button>();
        cardButton.onClick.AddListener(ClickedCard);

        if (CardRef != null)
        {
            _titleText.text = CardRef.Title;
            _typeText.text = CardRef.Type.ToString();
            _descriptionText.text = CardRef.Description;

            _mainArtImage.sprite = CardRef.Image;
            _frameImage.sprite = CardRef.Frame;
        }
    }

    private void ClickedCard()
    {
        CardInputManager.Instance.OnClickCard(CardRef);
    }
    
}
