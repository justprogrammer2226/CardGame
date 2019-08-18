using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    [SerializeField] private SpriteRenderer _frontSide;
    [SerializeField] private SpriteRenderer _backSide;
    [SerializeField] private bool _onSlot;
    public bool OnSlot
    {
        get => _onSlot;
        set => _onSlot = value;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        _frontSide.sprite = card.FrontSprite;
        _backSide.sprite = SettingsManager.instance.GetBackendSprite();
    }

    public void SetOrderInLayer(int order)
    {
        _frontSide.sortingOrder = order;
        _backSide.sortingOrder = order;
    }
}