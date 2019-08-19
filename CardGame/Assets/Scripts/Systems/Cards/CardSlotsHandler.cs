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

        GameManager.instance.OnRetreat += () =>
        {
            // Saves cardDisplays
            List<CardDisplay> cardDisplays = GetClosedSlots().Select(_ => _.CardDisplay).ToList();
            // Resets cardDisplay in closed slots
            foreach (CardSlot cardSlot in GetClosedSlots())
            {
                cardSlot.CardDisplay = null;
                cardSlot.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 15f / 255);
            }
            // Start moving cardDisplays
            StartCoroutine(SmoothRetreat(cardDisplays));
        };
    }

    public List<CardSlot> GetClosedSlots()
    {
        return cardSlots.Where(_ => _.CardDisplay != null).ToList();
    }

    private IEnumerator SmoothRetreat(List<CardDisplay> cardDisplays)
    {
        foreach (CardDisplay cardDisplay in cardDisplays)
        {
            DeckManager.instance.AddToRetreat(cardDisplay);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public int GetIndexByCardSlot(CardSlot cardSlot)
    {
        return cardSlots.IndexOf(cardSlot);
    }

    public bool ThereIsType(CardTypes cardType)
    {
        return cardSlots.Where(_ => _.CardDisplay != null).Select(_ => _.CardDisplay.card.Type).Contains(cardType);
    }

    public bool AtLeastTwoSlotIsFull()
    {
        return cardSlots.Where(_ => _.CardDisplay != null).Count() >= 2;
    }

    public bool AtLeastOneSlotIsFull()
    {
        return cardSlots.Where(_ => _.CardDisplay != null).Count() >= 1;
    }

    public CardSlot GetFirstFree()
    {
        return cardSlots.Where(_ => _.CardDisplay == null).FirstOrDefault();
    }
}
