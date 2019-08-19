using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private Turns _whoIsAttacking;
    public Turns WhoIsAttacking
    {
        get => _whoIsAttacking;
        private set
        {
            _whoIsAttacking = value;
            OnChangeAttaker?.Invoke(_whoIsAttacking);
        }
    }

    [SerializeField] private Turns _currentTurn;
    public Turns CurrentTurn
    {
        get => _currentTurn;
        private set
        {
            _currentTurn = value;
            OnChangeTurn?.Invoke(_currentTurn);
        }
    }

    public event Action OnRetreat;
    public event Action<Turns> OnChangeAttaker;
    public event Action<Turns> OnChangeTurn;

    private void Awake()
    {
        instance = this;
        // Starting init
        WhoIsAttacking = Turns.Player;
        CurrentTurn = Turns.Player;
    }

    private void Start()
    {
        DeckManager.instance.SpawnDeck();
        DeckManager.instance.AddCardsToPlayer(6);
        DeckManager.instance.AddCardsToOpponent(6);
        // We change the attacker after retreat
        OnRetreat += () => ChangeAttaker();
    }

    public void Retreat()
    {
        Debug.Log("Отбой");
        OnRetreat?.Invoke();
    }

    /// <summary>
    /// Change attacker.
    /// </summary>
    public void ChangeAttaker()
    {
        Debug.Log("Смена нападающего");
        if (WhoIsAttacking == Turns.Player)
        {
            //Debug.Log("Атакует бот");
            //Debug.Log("Ход бота");
            WhoIsAttacking = Turns.Bot;
            CurrentTurn = Turns.Bot;
        }
        else
        {
            Debug.Log("Атакует игрок");
            Debug.Log("Ход игрока");
            WhoIsAttacking = Turns.Player;
            CurrentTurn = Turns.Player;
        }
    }

    public void NextTurn()
    {
        if (CurrentTurn == Turns.Player)
        {
            //Debug.Log("Ход бота");
            CurrentTurn = Turns.Bot;
        }
        else
        {
            //Debug.Log("Ход игрока");
            CurrentTurn = Turns.Player;
        }
    }
}
