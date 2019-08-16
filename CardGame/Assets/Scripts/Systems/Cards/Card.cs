using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    public Sprite Sprite => _sprite;

    [SerializeField] private CardTypes _type;
    public CardTypes Type => _type;

    [SerializeField] private CardSuits _suit;
    public CardSuits Suit => _suit;
}
