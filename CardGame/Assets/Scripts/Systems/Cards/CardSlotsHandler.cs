using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlotsHandler : MonoBehaviour
{
    public static CardSlotsHandler instance;

    public List<CardSlot> cardSlots;
    public int numberOfClosedSlots = 0;

    private void Awake()
    {
        instance = this;
    }

    public void Retreat()
    {
        StartCoroutine(SmoothRetreat());
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
        numberOfClosedSlots = 0;
    }

    public int GetIndexByCardSlot(CardSlot cardSlot)
    {
        return cardSlots.IndexOf(cardSlot);
    }
}
