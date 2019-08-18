using UnityEngine;

public class CardSlot : MonoBehaviour
{
    [SerializeField] private CardDisplay _cardDisplay;
    public CardDisplay CardDisplay
    {
        get => _cardDisplay;
        set
        {
            if (CanPutCard(value))
            {
                if (value == null)
                {
                    _cardDisplay = value;
                }
                else if (Parent == null)
                {
                    _cardDisplay = value;
                    _cardDisplay.SetOrderInLayer(-1);
                }
                else if (Parent != null)
                {
                    _cardDisplay = value;
                    _cardDisplay.SetOrderInLayer(0);
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

        if (cardDisplay == null)
        {
            result = true;
        }
        else if (CardSlotsHandler.instance.numberOfClosedSlots == CardSlotsHandler.instance.GetIndexByCardSlot(this))
        {
            if (CardSlotsHandler.instance.AtLeastTwoSlotIsFull())
            {
                if (_cardDisplay == null && cardDisplay != null)
                {
                    if (Parent == null && CardSlotsHandler.instance.ThereIsType(cardDisplay.card.Type))
                    {
                        result = true;
                    }
                    else if (Parent != null && Parent.CardDisplay != null)
                    {
                        if (Parent.CardDisplay.card.Suit == cardDisplay.card.Suit && Parent.CardDisplay.card.Type < cardDisplay.card.Type)
                        {
                            result = true;
                        }
                        else if (cardDisplay.card.Suit == DeckManager.instance.GetTrump() && Parent.CardDisplay.card.Suit != DeckManager.instance.GetTrump())
                        {
                            result = true;
                        }
                        else if (cardDisplay.card.Suit == DeckManager.instance.GetTrump() && Parent.CardDisplay.card.Suit == DeckManager.instance.GetTrump() && Parent.CardDisplay.card.Type < cardDisplay.card.Type)
                        {
                            result = true;
                        }
                    }
                }
                else if (cardDisplay == null)
                {
                    result = true;
                }
            }
            else
            {
                if (_cardDisplay == null && cardDisplay != null)
                {
                    if (Parent == null)
                    {
                        result = true;
                    }
                    else if (Parent != null && Parent.CardDisplay != null)
                    {
                        if (Parent.CardDisplay.card.Suit == cardDisplay.card.Suit && Parent.CardDisplay.card.Type < cardDisplay.card.Type)
                        {
                            result = true;
                        }
                        else if (cardDisplay.card.Suit == DeckManager.instance.GetTrump() && Parent.CardDisplay.card.Suit != DeckManager.instance.GetTrump())
                        {
                            result = true;
                        }
                        else if (cardDisplay.card.Suit == DeckManager.instance.GetTrump() && Parent.CardDisplay.card.Suit == DeckManager.instance.GetTrump() && Parent.CardDisplay.card.Type < cardDisplay.card.Type)
                        {
                            result = true;
                        }
                    }
                }
                else if (cardDisplay == null)
                {
                    result = true;
                }
            }
        }

        return result;
    }
}
