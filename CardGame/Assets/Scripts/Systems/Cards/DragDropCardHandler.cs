using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
public class DragDropCardHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private CardSlot nearestCardSlot;

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
        if(!currentCardDisplay.OnSlot && DeckManager.instance.IsPlayerCard(currentCardDisplay))
        {
            currentTouchCoord = Camera.main.ScreenToWorldPoint(eventData.position);
            transform.position += currentTouchCoord - lastTouchCoord;
            lastTouchCoord = currentTouchCoord;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(!currentCardDisplay.OnSlot && nearestCardSlot != null && nearestCardSlot.CanPutCard(currentCardDisplay))
        {
            DeckManager.instance.DeleteFromPlayer(currentCardDisplay);
            nearestCardSlot.CardDisplay = currentCardDisplay;
            nearestCardSlot.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            AudioManager.PlaySound("cardPlace2");
            TransformHelper.SmoothMove(currentCardDisplay.transform, nearestCardSlot.transform.position);
        }
        else
        {
            TransformHelper.SmoothMove(currentCardDisplay.transform, startCoord);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!currentCardDisplay.OnSlot && collision.tag == "CardSlot" && collision.GetComponent<CardSlot>().CanPutCard(currentCardDisplay))
        {
            nearestCardSlot = collision.GetComponent<CardSlot>();
            nearestCardSlot.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 100f / 255);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CardSlot" && nearestCardSlot != null && collision.GetComponent<CardSlot>() == nearestCardSlot)
        { 
            nearestCardSlot.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 15f / 255);
            nearestCardSlot = null;
        }
    }
}
