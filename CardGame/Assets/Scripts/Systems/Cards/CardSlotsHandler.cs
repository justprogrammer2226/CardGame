using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardSlotsHandler : MonoBehaviour
{
    public static CardSlotsHandler instance;

    public List<CardSlot> cardSlots;
    [SerializeField] private int _numberOfClosedSlots = 0;
    public int NumberOfClosedSlots
    {
        get => _numberOfClosedSlots;
        private set
        {
            _numberOfClosedSlots = value;
            if(_numberOfClosedSlots == cardSlots.Count)
            {
                GameManager.instance.Retreat();
            }
        }
    }

    private void Awake()
    {
        instance = this;

        foreach (CardSlot cardSlot in cardSlots)
        {
            cardSlot.OnSlotClose += () => NumberOfClosedSlots++;
            cardSlot.OnSlotOpen += () => NumberOfClosedSlots--;
        }

        GameManager.instance.OnRetreat += () => StartCoroutine(SmoothRetreat());
    }

    public List<CardSlot> GetClosedSlots()
    {
        return cardSlots.Where(_ => _.CardDisplay != null).ToList();
    }

    private IEnumerator SmoothRetreat()
    {
        foreach (CardSlot cardSlot in cardSlots)
        {
            if (cardSlot.CardDisplay != null)
            {
                DeckManager.instance.AddToRetreat(cardSlot.CardDisplay);
                cardSlot.CardDisplay = null;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public int GetIndexByCardSlot(CardSlot cardSlot)
    {
        return cardSlots.IndexOf(cardSlot);
    }

    public bool ThereIsType(CardTypes cardType)
    {
        Debug.Log("Все типы" + string.Join(", ", cardSlots.Where(_ => _.CardDisplay != null).Select(_ => _.CardDisplay.card.Type)));
        return cardSlots.Where(_ => _.CardDisplay != null).Select(_ => _.CardDisplay.card.Type).Contains(cardType);
    }

    public bool AtLeastTwoSlotIsFull()
    {
        return cardSlots.Where(_ => _.CardDisplay != null).Count() >= 2;
    }

    public CardSlot GetFirstFree()
    {
        return cardSlots.Where(_ => _.CardDisplay == null).First();
    }
}
