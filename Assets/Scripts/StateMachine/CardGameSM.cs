using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameSM : StateMachine
{
    [SerializeField] InputController _input;
    public InputController Input => _input;
    [SerializeField] CardsManager _cardsManager;
    public CardsManager CardsManager => _cardsManager;
    [SerializeField] AudioSource _audioManager;
    public AudioSource AudioManager => _audioManager;
    [SerializeField] int _playerHealth;
    public int PlayerHealth => _playerHealth;
    [SerializeField] int _enemyHealth;
    public int EnemyHealth => _enemyHealth;
    [SerializeField] int _playerEnergy;
    public int PlayerEnergy => _playerEnergy;
    [SerializeField] int _enemyEnergy;
    public int EnemyEnergy => _enemyEnergy;
    [SerializeField] int _energyProd = 2;
    public int EnergyProd => _energyProd;
    private bool _playerTaunted = false;
    public bool PlayerTaunted => _playerTaunted;
    private bool _enemyTaunted = false;
    public bool EnemyTaunted => _enemyTaunted;
    public GameObject slashPrefab;
    public GameObject explosionPrefab;
    [SerializeField] AudioClip baseHit;
    [SerializeField] AudioClip winClip;
    [SerializeField] AudioClip loseClip;

    void Start()
    {
        ChangeState<SetupCardGameState>();
    }
    public void IncreaseEnergy(){
        _energyProd += 1;
    }
    public void AttackEnemy(int dmg){
        _enemyHealth -= dmg;
        PlayTrack(baseHit);
    }
    public void AttackPlayerBase(int dmg){
        _playerHealth -= dmg;
        PlayTrack(baseHit);
    }
    public void TauntPlayer(bool taunted){
        _playerTaunted = taunted;
    }
    public void TauntEnemy(bool taunted){
        _enemyTaunted = taunted;
    }
    public void ChangePlayerEnergy(int q){
        _playerEnergy += q;
        if(_playerEnergy > 9){
            _playerEnergy = 9;
        }
    }
    public void ChangeEnemyEnergy(int q){
        _enemyEnergy += q;
        if(_enemyEnergy > 9){
            _enemyEnergy = 9;
        }
    }
    public void PlayTrack(AudioClip clip){
        _audioManager.PlayOneShot(clip);
    }
    public void Win(){
        _audioManager.Stop();
        _audioManager.PlayOneShot(winClip);
    }
    public void Lose(){
        _audioManager.Stop();
        _audioManager.PlayOneShot(loseClip);
    }
}
