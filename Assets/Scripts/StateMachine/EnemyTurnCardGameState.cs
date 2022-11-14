using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyTurnCardGameState : CardGameState
{
    public static event Action EnemyTurnBegan;
    public static event Action EnemyTurnEnded;

    [SerializeField] Text _enemyTurnTextUI = null;
    [SerializeField] Text _enemyEnergy = null;
    [SerializeField] GameObject _enemyTurnUI = null;
    [SerializeField] float _pauseDuration = 1.5f;
    [SerializeField] Base _playerBase = null;
    private Color faded;
    void Start(){
        faded = _enemyTurnTextUI.color;
    }

    public override void Enter(){
        Debug.Log("Enemy Turn: Enter");
        _enemyTurnUI.SetActive(true);
        faded.a = 1;
        _enemyTurnTextUI.color = faded;
        EnemyTurnBegan?.Invoke();

        CardDisplay.destroyed += OnDestroy;

        _playerBase.interactable = true;
        StateMachine.ChangeEnemyEnergy(StateMachine.EnergyProd);
        StateMachine.CardsManager.EnemyDrawCard();
        StartCoroutine(EnemyThinkingRoutine(_pauseDuration));
    }

    public override void Tick()
    {
        if(_enemyTurnTextUI.color.a>0f){
            faded.a = _enemyTurnTextUI.color.a - 0.001f;
            _enemyTurnTextUI.color = faded;
        }
        _enemyEnergy.text = StateMachine.EnemyEnergy.ToString();
        if(StateMachine.PlayerHealth <= 0){
            StateMachine.ChangeState<PlayerLoseCardGameState>();
        }
    }

    public override void Exit()
    {
        Debug.Log("Enemy Turn: Exit");
        _enemyTurnUI.SetActive(false);
        _playerBase.interactable = false;

        CardDisplay.destroyed -= OnDestroy;
    }

    IEnumerator EnemyThinkingRoutine(float pauseDuration){
        yield return new WaitForSeconds(pauseDuration*1.5f);
        List<CardDisplay> added = new List<CardDisplay>();
        foreach(CardDisplay card in StateMachine.CardsManager.enemyHand){
            yield return new WaitForSeconds(pauseDuration);
            if(StateMachine.EnemyEnergy >= card.card.cost){
                AddCard(card);
                added.Add(card);
            }
        }
        foreach(CardDisplay card in added){
            StateMachine.CardsManager.enemyHand.Remove(card);
        }
        foreach(CardDisplay card in StateMachine.CardsManager.enemyCardsInSlots){
            if(card!= null && card.canAttack){
                yield return new WaitForSeconds(pauseDuration);
                AttackPlayer(card);
                yield return new WaitForSeconds(pauseDuration);
            }
        }
        EnemyTurnEnded?.Invoke();
        if(StateMachine.CurrentState.Equals(this)){
            StateMachine.ChangeState<PlayerTurnCardGameState>();
        }
    }

    void AttackPlayer(CardDisplay card){
        StateMachine.AttackPlayer(card.card.power);
        _playerBase.healthTxt.text = StateMachine.PlayerHealth.ToString();
    }
    void AddCard(CardDisplay card){
        for(int i = 0; i < StateMachine.CardsManager.availableEnemySlots.Length; i++){
            if(StateMachine.CardsManager.availableEnemySlots[i]){
                card.inSlot = true;
                StateMachine.CardsManager.enemyCardsInSlots[i] = card;
                StateMachine.CardsManager.availableEnemySlots[i] = false;
                card.GetComponent<RectTransform>().SetParent(StateMachine.CardsManager.enemySlots[i].parent);
                card.GetComponent<RectTransform>().localPosition = StateMachine.CardsManager.enemySlots[i].GetComponent<RectTransform>().localPosition;
                StateMachine.ChangeEnemyEnergy(-card.card.cost);
                return;
            }
        }
    }
    void OnDestroy(CardDisplay card){
        if(card.isPlayer){
            for(int i = 0; i < StateMachine.CardsManager.playerSlots.Count; i++){
                if(StateMachine.CardsManager.playerSlots[i].card == card){
                    StateMachine.CardsManager.availablePlayerSlots[i] = true;
                }
            }
        }
        else{
            for(int i = 0; i < StateMachine.CardsManager.enemySlots.Count; i++){
                if(StateMachine.CardsManager.enemySlots[i].card == card){
                    StateMachine.CardsManager.availableEnemySlots[i] = true;
                }
            }
        }
    }
}
