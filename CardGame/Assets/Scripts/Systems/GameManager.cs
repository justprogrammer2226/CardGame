using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event Action OnRetreat;
    public event Action OnTake;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Starting init
        DeckManager.instance.SpawnDeck();
        DeckManager.instance.AddCardsToPlayer(6);
    }

    public void Retreat()
    {
        OnRetreat?.Invoke();
    }

    public void Take()
    {
        OnTake?.Invoke();
    }
}
