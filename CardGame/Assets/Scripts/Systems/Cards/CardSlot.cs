using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlot : MonoBehaviour
{
    [SerializeField] private bool _isAvailability;
    public bool IsAvailability
    {
        get => _isAvailability;
        set => _isAvailability = value;
    }

    [SerializeField] private CardSlotTypes _cardSlotType;
    public CardSlotTypes CardSlotType
    {
        get => _cardSlotType;
    }

    [SerializeField] private CardSlot _child;
    public CardSlot Child
    {
        get => _child;
    }
}
