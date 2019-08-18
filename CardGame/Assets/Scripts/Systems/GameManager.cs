using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event Action OnRetreat;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DeckManager.instance.SpawnDeck();
        DeckManager.instance.AddCardsToPlayer(6);
        DeckManager.instance.AddCardsToOpponent(6);
    }

    public void Retreat()
    {
        OnRetreat?.Invoke();
    }
}
