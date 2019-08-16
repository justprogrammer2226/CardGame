using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private List<Card> cards;
    [SerializeField] private Transform deckSpawnPoint;
    [SerializeField] private Deck currentDeck;

    [SerializeField] private GameObject cardPrefab;

    private void Awake()
    {
        #region Checks for the correctness of the filled cards

        if (cards.Count != 36)
        {
            throw new Exception("The number of cards must be equal to 36.");
        }

        foreach(Card card in cards)
        {
            if(card == null)
            {
                throw new Exception("Card cannot be null.");
            }
        }

        #region Checks on card suits
        if (cards.Where(_ => _.Suit == CardSuits.Clubs).Count() != 9)
        {
            throw new Exception("The number of cards of the suit of clubs should be 9.");
        }

        if (cards.Where(_ => _.Suit == CardSuits.Diamonds).Count() != 9)
        {
            throw new Exception("The number of cards of the diamond of clubs should be 9.");
        }

        if (cards.Where(_ => _.Suit == CardSuits.Hearts).Count() != 9)
        {
            throw new Exception("The number of cards of the heart of clubs should be 9.");
        }

        if (cards.Where(_ => _.Suit == CardSuits.Spades).Count() != 9)
        {
            throw new Exception("The number of cards of the spade of clubs should be 9.");
        }
        #endregion

        #region Checks on card types
        if (cards.Where(_ => _.Type == CardTypes.Six).Count() != 4)
        {
            throw new Exception("The number of sixes should be 4.");
        }

        if (cards.Where(_ => _.Type == CardTypes.Seven).Count() != 4)
        {
            throw new Exception("The number of sevens should be 4.");
        }

        if (cards.Where(_ => _.Type == CardTypes.Eight).Count() != 4)
        {
            throw new Exception("The number of eights should be 4.");
        }

        if (cards.Where(_ => _.Type == CardTypes.Nine).Count() != 4)
        {
            throw new Exception("The number of nines should be 4.");
        }

        if (cards.Where(_ => _.Type == CardTypes.Ten).Count() != 4)
        {
            throw new Exception("The number of tens should be 4.");
        }

        if (cards.Where(_ => _.Type == CardTypes.Jack).Count() != 4)
        {
            throw new Exception("The number of jacks should be 4.");
        }

        if (cards.Where(_ => _.Type == CardTypes.Queen).Count() != 4)
        {
            throw new Exception("The number of queens should be 4.");
        }

        if (cards.Where(_ => _.Type == CardTypes.King).Count() != 4)
        {
            throw new Exception("The number of kings should be 4.");
        }

        if (cards.Where(_ => _.Type == CardTypes.Ace).Count() != 4)
        {
            throw new Exception("The number of aces should be 4.");
        }
        #endregion
        #endregion

        SpawnDeck();
    }

    public void SpawnDeck()
    {
        currentDeck = new Deck(cards);

        foreach(Card card in currentDeck.GetCards())
        {
            CardDisplay cardDisplay = Instantiate(cardPrefab, deckSpawnPoint, true).GetComponent<CardDisplay>();
            cardDisplay.card = card;
            cardDisplay.UpdateUI();
        }
    }
}
