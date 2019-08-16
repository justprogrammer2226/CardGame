using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Deck
{
    private Stack<Card> deck;
    private Card trumpCard;

    public Deck(List<Card> cards)
    {
        deck?.Clear();
        ShuffleCards(cards);
        deck = new Stack<Card>(cards);
        trumpCard = deck.Pop();
    }

    private void ShuffleCards(List<Card> cards)
    {
        for (int i = 0; i < cards.Count - 1; i++)
        {
            int r = UnityEngine.Random.Range(i, cards.Count);
            Card tmp = cards[i];
            cards[i] = cards[r];
            cards[r] = tmp;
        }
    }

    public CardSuits GetTrump()
    {
        return GetTrumpCard().Suit;
    }

    public Card GetTrumpCard()
    {
        return trumpCard;
    }

    public List<Card> GetCards()
    {
        return deck.ToList();
    }

    public Card TakeCard()
    {
        return deck.Pop();
    }
}
