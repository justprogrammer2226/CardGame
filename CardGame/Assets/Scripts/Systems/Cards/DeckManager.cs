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
    [Tooltip("Length of tline on which player and opponent cards will be located")]
    [Range(0, 10)] public int lineLength;

    [Header("Movement settings")]
    public float movementDuration;
    public AnimationCurve movementCurve;

    [Header("Debug")]
    [SerializeField] private Deck currentDeck;
    [SerializeField] private List<CardDisplay> cardDisplays;
    [SerializeField] private CardDisplay trumpCardDisplay;

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
        currentDeck = new Deck(cards);

        SpawnDeck();
        StartCoroutine(GiveCardsToPlayer());
        StartCoroutine(GiveCardsToOpponent());
    }

    private void SpawnDeck()
    {
        playerCardsDisplays = new List<CardDisplay>();
        opponentCardsDisplays = new List<CardDisplay>();
        retreat = new Stack<CardDisplay>();


        // Spawn trump card
        trumpCardDisplay = Instantiate(cardPrefab, deckSpawnPoint.position, Quaternion.identity, deckSpawnPoint).GetComponent<CardDisplay>();
        trumpCardDisplay.card = currentDeck.GetTrumpCard();
        trumpCardDisplay.UpdateUI();
        trumpCardDisplay.transform.localPosition = new Vector3(0.3f, 0, 0);
        trumpCardDisplay.transform.localEulerAngles = new Vector3(0, 0, 90);
        
        // Spawn other card
        foreach (Card card in currentDeck.GetCards())
        {
            CardDisplay cardDisplay = Instantiate(cardPrefab, deckSpawnPoint.position, Quaternion.identity, deckSpawnPoint).GetComponent<CardDisplay>();
            cardDisplay.card = card;
            cardDisplay.UpdateUI();
            cardDisplay.transform.localEulerAngles = new Vector3(0, 180, 0);
            cardDisplays.Add(cardDisplay);
        }
    }

    private IEnumerator GiveCardsToPlayer()
    {
        List<Vector3> points = GetPointsForCards(playerCardsPosition.position, lineLength, 6);

        for (int i = 0; i < points.Count; i++)
        {
            Card takedCard = currentDeck.TakeCard();
            CardDisplay cardDisplay = cardDisplays.Where(_ => _.card == takedCard).Single();
            cardDisplay.SetOrderInLayer(i);
            SmoothRotate(cardDisplay, new Vector3(0, 0, 0));
            SmoothMove(cardDisplay, points[i]);
            AddToPlayer(cardDisplay);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator GiveCardsToOpponent()
    {
        List<Vector3> points = GetPointsForCards(opponentCardsPosition.position, lineLength, 6);

        for (int i = 0; i < points.Count; i++)
        {
            Card takedCard = currentDeck.TakeCard();
            CardDisplay cardDisplay = cardDisplays.Where(_ => _.card == takedCard).Single();
            cardDisplay.SetOrderInLayer(i);
            SmoothRotate(cardDisplay, new Vector3(0, 180, 0));
            SmoothMove(cardDisplay, points[i]);
            AddToOpponent(cardDisplay);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private List<Vector3> GetPointsForCards(Vector3 cardsPosition, int length, int numberOfCards)
    {
        List<Vector3> points = new List<Vector3>();

        float step = (float)length / numberOfCards;
        float temp = cardsPosition.x - (float)length / 2 + step / 2;

        for (int i = 0; i < numberOfCards; i++)
        {
            points.Add(new Vector3(cardsPosition.x + temp, cardsPosition.y, cardsPosition.z));
            temp += step;
        }

        return points;
    }

    public void SmoothMove(CardDisplay cardDisplay, Vector3 endPosition)
    {
        StartCoroutine(MoveCard(cardDisplay, endPosition));
    }

    private IEnumerator MoveCard(CardDisplay cardDisplay, Vector3 endPosition)
    {
        Vector3 startPosition = cardDisplay.transform.position;
        float timer = 0.0f;
        while (timer < movementDuration)
        {
            timer += Time.deltaTime;
            cardDisplay.transform.position = Vector3.Lerp(startPosition, endPosition, movementCurve.Evaluate(timer / movementDuration));
            yield return null;
        }
        cardDisplay.transform.position = endPosition;
    }

    public void SmoothRotate(CardDisplay cardDisplay, Vector3 endPosition)
    {
        StartCoroutine(RotateCard(cardDisplay, endPosition));
    }

    private IEnumerator RotateCard(CardDisplay cardDisplay, Vector3 targetRotation)
    {
        while (cardDisplay.transform.rotation != Quaternion.Euler(targetRotation))
        {
            cardDisplay.transform.rotation = Quaternion.RotateTowards(cardDisplay.transform.rotation, Quaternion.Euler(targetRotation), 200 * Time.deltaTime);
            yield return null;
        }
    }

    public void AddToRetreat(CardDisplay cardDisplay)
    {
        SmoothMove(cardDisplay, retreatSpawnPoint.position);
        if (Mathf.RoundToInt(UnityEngine.Random.value) == 0)
        {
            SmoothRotate(cardDisplay, new Vector3(180, 0, 180));
        }
        else
        {
            SmoothRotate(cardDisplay, new Vector3(0, 180, 180));
        }
        retreat.Push(cardDisplay);
    }

    public void AddToPlayer(CardDisplay cardDisplay)
    {
        playerCardsDisplays.Add(cardDisplay);
    }

    public void AddToOpponent(CardDisplay cardDisplay)
    {
        opponentCardsDisplays.Add(cardDisplay);
    }

    public void DeleteFromPlayer(CardDisplay cardDisplay)
    {
        playerCardsDisplays.Remove(cardDisplay);
        RebuildCardDisplays(playerCardsDisplays, playerCardsPosition.position, lineLength);
    }

    public void DeleteFromOpponent(CardDisplay cardDisplay)
    {
        opponentCardsDisplays.Remove(cardDisplay);
        RebuildCardDisplays(opponentCardsDisplays, opponentCardsPosition.position, lineLength);
    }

    public bool IsPlayerCard(CardDisplay cardDisplay)
    {
        return playerCardsDisplays.Contains(cardDisplay);
    }

    public void RebuildCardDisplays(List<CardDisplay> cardDisplays, Vector3 cardsPosition, int length)
    {
        List<Vector3> points = GetPointsForCards(cardsPosition, length, cardDisplays.Count);

        for (int i = 0; i < points.Count; i++)
        {
            SmoothMove(cardDisplays[i], points[i]);
        }
    }
}
