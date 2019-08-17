using UnityEngine;

public class CardSlot : MonoBehaviour
{
    [SerializeField] private CardDisplay _cardDisplay;
    public CardDisplay CardDisplay
    {
        get => _cardDisplay;
        set
        {
            if (CardSlotsHandler.instance.numberOfClosedSlots == CardSlotsHandler.instance.GetIndexByCardSlot(this))
            {
                if (_cardDisplay == null && value != null)
                {
                    if (Parent == null)
                    {
                        _cardDisplay = value;
                        _cardDisplay.SetOrderInLayer(-1);
                    }
                    else if (Parent != null && Parent.CardDisplay != null && Parent.CardDisplay.card.Suit == value.card.Suit && Parent.CardDisplay.card.Type < value.card.Type)
                    {
                        _cardDisplay = value;
                        _cardDisplay.SetOrderInLayer(0);
                    }
                }
                else if (value == null)
                {
                    _cardDisplay = value;
                }
            }
        }
    }

    [SerializeField] private CardSlot _parent;
    public CardSlot Parent
    {
        get => _parent;
    }

    public bool CanPutCard(CardDisplay cardDisplay)
    {
        bool result = false;

        if (CardSlotsHandler.instance.numberOfClosedSlots == CardSlotsHandler.instance.GetIndexByCardSlot(this))
        {
            if (_cardDisplay == null && cardDisplay != null)
            {
                if (Parent == null)
                {
                    result = true;
                }
                else if (Parent != null && Parent.CardDisplay != null && Parent.CardDisplay.card.Suit == cardDisplay.card.Suit && Parent.CardDisplay.card.Type < cardDisplay.card.Type)
                {
                    result = true;
                }
            }
            else if (cardDisplay == null)
            {
                result = true;
            }
        }

        return result;
    }
}
