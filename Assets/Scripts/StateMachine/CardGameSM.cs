using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameSM : StateMachine
{
    [SerializeField] InputController _input;
    public InputController Input => _input;
    [SerializeField] CardsManager _cardsManager;
    public CardsManager CardsManager => _cardsManager;

    [SerializeField] int _playerHealth;
    public int PlayerHealth => _playerHealth;
    [SerializeField] int _enemyHealth;
    public int EnemyHealth => _enemyHealth;
    void Start()
    {
        ChangeState<SetupCardGameState>();
    }
}
