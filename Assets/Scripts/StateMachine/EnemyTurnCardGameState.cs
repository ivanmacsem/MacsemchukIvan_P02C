using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyTurnCardGameState : CardGameState
{
    public static event Action EnemyTurnBegan;
    public static event Action EnemyTurnEnded;

    [SerializeField] float _pauseDuration = 1.5f;
    [SerializeField] Base _playerBase = null;

    public override void Enter(){
        Debug.Log("Enemy Turn: Enter");
        EnemyTurnBegan?.Invoke();

        _playerBase.interactable = true;

        StartCoroutine(EnemyThinkingRoutine(_pauseDuration));
        _playerBase.BaseAttacked += AttackPlayer;
    }

    public override void Exit()
    {
        Debug.Log("Enemy Turn: Exit");
        _playerBase.interactable = false;
        _playerBase.BaseAttacked -= AttackPlayer;
    }

    IEnumerator EnemyThinkingRoutine(float pauseDuration){
        yield return new WaitForSeconds(pauseDuration);
        EnemyTurnEnded?.Invoke();

        StateMachine.ChangeState<PlayerTurnCardGameState>();
    }

    void AttackPlayer(){
        StateMachine.AttackPlayer(_playerBase.dmg);
        _playerBase.healthTxt.text = StateMachine.PlayerHealth.ToString();
    }
}
