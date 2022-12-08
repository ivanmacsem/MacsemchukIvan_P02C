using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyTurnCardGameState : CardGameState
{
    public static event Action EnemyTurnBegan;
    public static event Action EnemyTurnEnded;
    public static event Action<bool> TauntPlayer;
    public static event Action<bool> TauntEnemy;

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
        if(StateMachine.PlayerTaunted){
            TauntPlayer.Invoke(true);
        }
        if(StateMachine.EnemyTaunted){
            TauntEnemy.Invoke(true);
        }

        CardDisplay.destroyed += OnDestroyed;

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

        CardDisplay.destroyed -= OnDestroyed;
    }

    IEnumerator EnemyThinkingRoutine(float pauseDuration){
        yield return new WaitForSeconds(pauseDuration*1.5f);
        List<CardDisplay> added = new List<CardDisplay>();
        List<CardDisplay> temp = StateMachine.CardsManager.enemyHand;
        foreach(CardDisplay card in temp){
            yield return new WaitForSeconds(pauseDuration);
            if(StateMachine.EnemyEnergy >= card.card.cost){
                AddCard(card);
                added.Add(card);
            }
        }
        foreach(CardDisplay card in added){
            StateMachine.CardsManager.enemyHand.Remove(card);
        }
        foreach(Slot slot in StateMachine.CardsManager.enemySlots){
            if(slot.card != null && slot.card.canAttack){
                yield return new WaitForSeconds(pauseDuration);
                StartCoroutine(AttackPlayer(slot.card));
                slot.card.canAttack = false;
                if(slot.card.canDblAttack){
                    StartCoroutine(AttackPlayer(slot.card));
                    slot.card.canDblAttack = false;
                }
                yield return new WaitForSeconds(pauseDuration);
            }
        }
        EnemyTurnEnded?.Invoke();
        if(StateMachine.CurrentState.Equals(this)){
            StateMachine.ChangeState<PlayerTurnCardGameState>();
        }
    }

    IEnumerator AttackPlayer(CardDisplay card){
        if(!StateMachine.EnemyTaunted){
            int highestEval = card.card.power;
            Slot bestMove = null;
            foreach(Slot s in StateMachine.CardsManager.playerSlots){
                if(s.card != null){
                    int curEval = 0;
                    CardDisplay playerCard = s.card;

                    int dmgDone = card.card.power;
                    int dmgTaken = playerCard.card.power;
                    bool kills = (dmgDone-playerCard.currHP >= 0) ? true: false;
                    bool dies = (dmgTaken-card.currHP >= 0) ? true: false;

                    if(playerCard.isShielded){
                        dmgDone = 1;
                        kills = false;
                    }
                    if(card.isShielded){
                        dmgTaken = 1;
                        dies = false;
                    }
                    if(card.card.ranged){
                        dmgTaken = 0;
                        dies = false;
                    }

                    if(kills && !dies){
                        curEval =  (playerCard.card.cost-card.card.cost) + playerCard.card.cost*2 - dmgTaken;
                    }
                    else if(kills && dies){
                        curEval = (playerCard.card.cost-card.card.cost)*2;
                    }
                    else if(!kills && !dies){
                        curEval = (dmgDone-dmgTaken) + (playerCard.card.cost-card.card.cost);
                    }
                    else{
                        curEval = dmgDone - card.card.cost*2 + (playerCard.card.cost-card.card.cost);
                    }

                    if(curEval >= highestEval){
                        highestEval = curEval;
                        bestMove = s;
                    }
                }
            }
            if(bestMove != null){
                yield return card.AttackAnimation(bestMove.card.gameObject.GetComponent<RectTransform>());
                StartCoroutine(bestMove.card.TakeDamage(card.card.power, false));
                StartCoroutine(card.TakeDamage(bestMove.card.card.power, true));
            }
            else{
                yield return card.AttackAnimation(_playerBase.gameObject.GetComponent<RectTransform>());
                StateMachine.AttackPlayerBase(card.card.power);
                _playerBase.TakeDmgAnimation(card.card.power);
                _playerBase.healthTxt.text = StateMachine.PlayerHealth.ToString();
            }
        }
        else{
            foreach (Slot s in StateMachine.CardsManager.playerSlots)
            {
                if(s.card != null){
                    if(s.card.card.taunt){
                        StartCoroutine(s.card.TakeDamage(card.card.power, false));
                        StartCoroutine(card.TakeDamage(s.card.card.power, true));
                        break;
                    }
                }
            }
        }
    }
    void AddCard(CardDisplay card){
        for(int i = 0; i < StateMachine.CardsManager.availableEnemySlots.Length; i++){
            if(StateMachine.CardsManager.availableEnemySlots[i]){
                if(card.card.callback){
                    StateMachine.CardsManager.Callback(false);
                    if(StateMachine.PlayerTaunted){
                        TauntPlayer.Invoke(false);
                        StateMachine.TauntPlayer(false);
                    }
                }
                card.inSlot = true;
                StateMachine.CardsManager.enemySlots[i].card = card;
                StateMachine.CardsManager.availableEnemySlots[i] = false;
                card.GetComponent<RectTransform>().SetParent(StateMachine.CardsManager.enemySlots[i].parent);
                card.GetComponent<RectTransform>().localPosition = StateMachine.CardsManager.enemySlots[i].GetComponent<RectTransform>().localPosition;
                StateMachine.ChangeEnemyEnergy(-card.card.cost);
                if(card.card.taunt){
                    TauntPlayer.Invoke(true);
                    StateMachine.TauntPlayer(true);
                }
                if(card.card.rush){
                    card.canAttack = true;
                    if(card.card.dblStrike){
                        card.canDblAttack = true;
                    }
                }
                if(card.card.spray){
                    StateMachine.CardsManager.Spray(true);
                }
                if(card.card.banish){
                    StateMachine.CardsManager.Banish(true);
                }
                return;
            }
        }
    }
    void OnDestroyed(CardDisplay card, bool banished){
        if(card.isPlayer){
            for(int i = 0; i < StateMachine.CardsManager.playerSlots.Count; i++){
                if(StateMachine.CardsManager.playerSlots[i] != null){
                    if(StateMachine.CardsManager.playerSlots[i].card.Equals(card)){
                        StateMachine.CardsManager.availablePlayerSlots[i] = true;
                        if(card.card.taunt){
                            TauntEnemy.Invoke(false);
                            StateMachine.TauntEnemy(false);
                        }
                        Destroy(card.gameObject);
                        if(!banished){
                            GameObject slashInst = Instantiate(StateMachine.slashPrefab, card.GetComponent<RectTransform>().parent);
                            slashInst.GetComponent<RectTransform>().anchoredPosition = card.GetComponent<RectTransform>().anchoredPosition + new Vector2(-50, 15);
                        }
                        else{
                            GameObject explInst = Instantiate(StateMachine.explosionPrefab, card.GetComponent<RectTransform>().parent);
                            explInst.GetComponent<RectTransform>().anchoredPosition = card.GetComponent<RectTransform>().anchoredPosition + new Vector2(-50, 15);
                        }
                        return;
                    }
                }
            }
        }
        else{
            for(int i = 0; i < StateMachine.CardsManager.enemySlots.Count; i++){
                if(StateMachine.CardsManager.enemySlots[i] != null){
                    if(StateMachine.CardsManager.enemySlots[i].card.Equals(card)){
                        StateMachine.CardsManager.availableEnemySlots[i] = true;
                        if(card.card.taunt){
                            TauntPlayer.Invoke(false);
                            StateMachine.TauntPlayer(false);
                        }
                        Destroy(card.gameObject);
                        if(!banished){
                            GameObject slashInst = Instantiate(StateMachine.slashPrefab, card.GetComponent<RectTransform>().parent);
                            slashInst.GetComponent<RectTransform>().anchoredPosition = card.GetComponent<RectTransform>().anchoredPosition + new Vector2(-50, 15);
                        }
                        else{
                            GameObject explInst = Instantiate(StateMachine.explosionPrefab, card.GetComponent<RectTransform>().parent);
                            explInst.GetComponent<RectTransform>().anchoredPosition = card.GetComponent<RectTransform>().anchoredPosition + new Vector2(-50, 15);
                        }
                        return;
                    }
                }
            }
        }
    }
}
