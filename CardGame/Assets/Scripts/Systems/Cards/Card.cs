using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    [SerializeField] private Sprite _frontSprite;
    public Sprite FrontSprite => _frontSprite;

    [SerializeField] private Sprite _backSprite;
    public Sprite BackSprite => _backSprite;

    [SerializeField] private CardTypes _type;
    public CardTypes Type => _type;

    [SerializeField] private CardSuits _suit;
    public CardSuits Suit => _suit;
}
