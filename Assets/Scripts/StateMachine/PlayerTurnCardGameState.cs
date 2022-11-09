using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnCardGameState : CardGameState
{
    [SerializeField] Text _playerTurnTextUI = null;
    [SerializeField] GameObject _playerTurnUI = null;

    private Color faded;

    void Start(){
        faded = _playerTurnTextUI.color;
    }

    public override void Enter(){
        Debug.Log("Player Turn: Entering");
        _playerTurnUI.SetActive(true);
        faded.a = 1;
        _playerTurnTextUI.color = faded;

        StateMachine.CardsManager.PlayerDrawCard();

        StateMachine.Input.PressedEndTurn += OnPressedEndTurn;
    }

    public override void Tick()
    {
        if(_playerTurnTextUI.color.a>0f){
            faded.a = _playerTurnTextUI.color.a - 0.001f;
            _playerTurnTextUI.color = faded;
        }
    }

    public override void Exit()
    {
        _playerTurnUI.SetActive(false);
        StateMachine.Input.PressedEndTurn -= OnPressedEndTurn;

        Debug.Log("Player Turn: Exit");
    }

    void OnPressedEndTurn(){
        StateMachine.ChangeState<EnemyTurnCardGameState>();
    }
}
