using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Deck : MonoBehaviour
{
    private Stack<CardDisplay> deck;
    private CardDisplay trumpCard;
    private CardSuits trumpSuit;

    public Deck(List<Card> cards, GameObject cardPrefab, Transform deckSpawnPoint)
    {
        ShuffleCards(cards);
        SpawnCards(cards, cardPrefab, deckSpawnPoint);
        trumpCard = deck.Pop();
        trumpSuit = trumpCard.card.Suit;
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

    private void SpawnCards(List<Card> cards, GameObject cardPrefab, Transform deckSpawnPoint)
    {
        deck = new Stack<CardDisplay>();
        foreach (Card card in cards)
        {
            CardDisplay cardDisplay = Instantiate(cardPrefab, deckSpawnPoint.position, Quaternion.identity, deckSpawnPoint).GetComponent<CardDisplay>();
            cardDisplay.card = card;
            cardDisplay.UpdateUI();
            deck.Push(cardDisplay);
        }
    }

    public CardSuits GetTrump()
    {
        return trumpSuit;
    }

    public CardDisplay GetTrumpCard()
    {
        return trumpCard;
    }

    public List<CardDisplay> GetCards()
    {
        return deck.ToList();
    }

    public CardDisplay TakeCard()
    {
        if (deck.Count != 0)
        {
            return deck.Pop();
        }
        else if (deck.Count == 0 && trumpCard != null)
        {
            CardDisplay temp = trumpCard;
            trumpCard = null;
            return temp;
        }
        else
        {
            return null;
        }
    }
}
