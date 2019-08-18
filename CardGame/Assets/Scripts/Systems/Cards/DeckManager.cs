using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Collections;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;

    [Header("Main settings")]
    [SerializeField] private List<Card> cards;
    public GameObject cardPrefab;
    public Transform deckSpawnPoint;
    public Transform opponentCardsPosition;
    public Transform playerCardsPosition;
    public Transform retreatSpawnPoint;
    [Tooltip("Length of line on which player and opponent cards will be located")]
    [Range(0, 10)] public int lineLength;
    public Turns currentTurn = Turns.Player;

    [Header("Debug")]
    [SerializeField] private Deck currentDeck;

    [Header("Cards in game")]
    [SerializeField] private List<CardDisplay> playerCardsDisplays;
    [SerializeField] private List<CardDisplay> opponentCardsDisplays;
    [SerializeField] private Stack<CardDisplay> retreat;

    private void Awake()
    {
        instance = this;
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
    }

    private void Start()
    {
        GameManager.instance.OnRetreat += () =>
        {
            if (playerCardsDisplays.Count < 6) AddCardsToPlayer(6 - playerCardsDisplays.Count);
            if (opponentCardsDisplays.Count < 6) AddCardsToOpponent(6 - opponentCardsDisplays.Count);
        };
    }

    public void SpawnDeck()
    {
        playerCardsDisplays = new List<CardDisplay>();
        opponentCardsDisplays = new List<CardDisplay>();
        retreat = new Stack<CardDisplay>();

        // Create deck
        currentDeck = new Deck(cards, cardPrefab, deckSpawnPoint);

        // Settings trump card
        CardDisplay trumpCardDisplay = currentDeck.GetTrumpCard();
        trumpCardDisplay.transform.localPosition = new Vector3(0.3f, 0, 0);
        trumpCardDisplay.transform.localEulerAngles = new Vector3(0, 0, 90);

        // Settings other cards
        foreach (CardDisplay cardDisplay in currentDeck.GetCards())
        {
            cardDisplay.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }

    public void AddToRetreat(CardDisplay cardDisplay)
    {
        TransformHelper.SmoothMove(cardDisplay.transform, retreatSpawnPoint.position);
        if (Mathf.RoundToInt(UnityEngine.Random.value) == 0)
        {
            TransformHelper.SmoothRotate(cardDisplay.transform, new Vector3(180, 0, 180));
        }
        else
        {
            TransformHelper.SmoothRotate(cardDisplay.transform, new Vector3(0, 180, 180));
        }
        retreat.Push(cardDisplay);
    }

    public void AddCardsToPlayer(int numberOfCards)
    {
        instance.StartCoroutine(instance.GiveCardsToPlayer(numberOfCards));
    }

    private IEnumerator GiveCardsToPlayer(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            CardDisplay cardDisplay = currentDeck.TakeCard();
            TransformHelper.SmoothRotate(cardDisplay.transform, new Vector3(0, 0, 0));
            playerCardsDisplays.Add(cardDisplay);
            RebuildCardDisplays(playerCardsDisplays, playerCardsPosition.position, lineLength);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void AddCardsToOpponent(int numberOfCards)
    {
        instance.StartCoroutine(instance.GiveCardsToOpponent(numberOfCards));
    }

    private IEnumerator GiveCardsToOpponent(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            CardDisplay cardDisplay = currentDeck.TakeCard();
            TransformHelper.SmoothRotate(cardDisplay.transform, new Vector3(0, 180, 0));
            opponentCardsDisplays.Add(cardDisplay);
            RebuildCardDisplays(opponentCardsDisplays, opponentCardsPosition.position, lineLength);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DeleteFromPlayer(CardDisplay cardDisplay)
    {
        playerCardsDisplays.Remove(cardDisplay);
        RebuildCardDisplays(playerCardsDisplays, playerCardsPosition.position, lineLength);
        NextTurn();
    }

    public void DeleteFromOpponent(CardDisplay cardDisplay)
    {
        opponentCardsDisplays.Remove(cardDisplay);
        RebuildCardDisplays(opponentCardsDisplays, opponentCardsPosition.position, lineLength);
        NextTurn();
    }

    private void NextTurn()
    {
        if(currentTurn == Turns.Bot)
        {
            currentTurn = Turns.Player;
        }
        else
        {
            currentTurn = Turns.Bot;

            var cardSlot = CardSlotsHandler.instance.GetFirstFree();

            for (int i = 0; i < opponentCardsDisplays.Count; i++)
            {
                if (cardSlot.CanPutCard(opponentCardsDisplays[i]))
                {
                    cardSlot.CardDisplay = opponentCardsDisplays[i];
                    TransformHelper.SmoothMove(opponentCardsDisplays[i].transform, cardSlot.transform.position);
                    TransformHelper.SmoothRotate(opponentCardsDisplays[i].transform, new Vector3(0, 0, 0));
                    DeleteFromOpponent(opponentCardsDisplays[i]);
                    break;
                }
                if (i == opponentCardsDisplays.Count - 1)
                {
                    Debug.Log("У меня нет чем ходить бро:(");
                    GiveCardsToOpponentFromTable();
                    currentTurn = Turns.Player;
                }
            }
        }
    }

    public bool IsPlayerCard(CardDisplay cardDisplay)
    {
        return playerCardsDisplays.Contains(cardDisplay);
    }

    public void RebuildCardDisplays(List<CardDisplay> cardDisplays, Vector3 cardsPosition, int length)
    {
        List<Vector3> points = TransformHelper.GetPointsForLine(cardsPosition, length, cardDisplays.Count);

        for (int i = 0; i < points.Count; i++)
        {
            cardDisplays[i].SetOrderInLayer(i);
            TransformHelper.SmoothMove(cardDisplays[i].transform, points[i]);
        }
    }

    public CardSuits GetTrump()
    {
        return currentDeck.GetTrump();
    }

    public void GiveCardsToPlayerFromTable()
    {
        List<CardSlot> cardSlots = CardSlotsHandler.instance.GetClosedSlots();

        foreach (CardSlot cardSlot in cardSlots)
        {
            TransformHelper.SmoothRotate(cardSlot.CardDisplay.transform, new Vector3(0, 0, 0));
            playerCardsDisplays.Add(cardSlot.CardDisplay);
            RebuildCardDisplays(playerCardsDisplays, playerCardsPosition.position, lineLength);
            cardSlot.CardDisplay = null;
        }
    }

    public void GiveCardsToOpponentFromTable()
    {
        List<CardSlot> cardSlots = CardSlotsHandler.instance.GetClosedSlots();

        foreach (CardSlot cardSlot in cardSlots)
        {
            TransformHelper.SmoothRotate(cardSlot.CardDisplay.transform, new Vector3(0, 180, 0));
            opponentCardsDisplays.Add(cardSlot.CardDisplay);
            RebuildCardDisplays(opponentCardsDisplays, opponentCardsPosition.position, lineLength);
            cardSlot.CardDisplay = null;
        }
    }
}
