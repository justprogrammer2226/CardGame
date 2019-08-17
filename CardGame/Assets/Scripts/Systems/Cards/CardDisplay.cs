using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    [SerializeField] private SpriteRenderer frontSide;
    [SerializeField] private SpriteRenderer backSide;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        frontSide.sprite = card.FrontSprite;
        backSide.sprite = SettingsManager.instance.GetBackendSprite();
    }

    public void SetOrderInLayer(int order)
    {
        frontSide.sortingOrder = order;
        backSide.sortingOrder = order;
    }
}