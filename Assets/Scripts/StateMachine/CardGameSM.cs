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
    [SerializeField] int _playerEnergy;
    public int PlayerEnergy => _playerEnergy;
    [SerializeField] int _enemyEnergy;
    public int EnemyEnergy => _enemyEnergy;
    void Start()
    {
        ChangeState<SetupCardGameState>();
    }
    public void AttackEnemy(int dmg){
        Debug.Log(dmg);
        _enemyHealth -= dmg;
    }
    public void AttackPlayer(int dmg){
        _playerHealth -= dmg;
    }
    public void ChangePlayerEnergy(int q){
        _playerEnergy += q;
    }
    public void ChangeEnemyEnergy(int q){
        _enemyEnergy += q;
    }
}
