using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
public class DragDropCardHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private CardSlot nearestCardSlot;
    [SerializeField] private bool onSlot;

    private Vector3 startCoord;
    private Vector3 currentTouchCoord;
    private Vector3 lastTouchCoord;

    private CardDisplay currentCardDisplay;

    private void Start()
    {
        currentCardDisplay = gameObject.GetComponent<CardDisplay>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startCoord = transform.position;
        currentTouchCoord = Camera.main.ScreenToWorldPoint(eventData.position);
        lastTouchCoord = currentTouchCoord;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!onSlot && DeckManager.instance.IsPlayerCard(currentCardDisplay))
        {
            currentTouchCoord = Camera.main.ScreenToWorldPoint(eventData.position);
            transform.position += currentTouchCoord - lastTouchCoord;
            lastTouchCoord = currentTouchCoord;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(nearestCardSlot != null && nearestCardSlot.CanPutCard(currentCardDisplay))
        {
            nearestCardSlot.CardDisplay = currentCardDisplay;
            onSlot = true;
            CardSlotsHandler.instance.numberOfClosedSlots++;
            DeckManager.instance.SmoothMove(currentCardDisplay, nearestCardSlot.transform.position);
            DeckManager.instance.DeleteFromPlayer(currentCardDisplay);
        }
        else
        {
            DeckManager.instance.SmoothMove(currentCardDisplay, startCoord);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "CardSlot" && collision.GetComponent<CardSlot>().CanPutCard(currentCardDisplay))
        {
            nearestCardSlot = collision.GetComponent<CardSlot>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CardSlot")
        {
            nearestCardSlot = null;
        }
    }
}
