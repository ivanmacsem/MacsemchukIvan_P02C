using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Card card;
    private RectTransform rectTransform;
    public static event Action<CardDisplay> zoomCard = delegate { };
    public Text nameText;
    public Text costText;
    public Text powerText;
    public Text healthText;

    public Image background;

    public TraitDisplay trait1;
    public TraitDisplay trait2;
    public bool inSlot = false;
    public int currHP = 0;
    public bool canAttack = false;
    public bool canDblAttack = false;
    public bool isShielded = false;
    public void Start()
    {
        nameText.text = card.name;
        costText.text = card.cost.ToString();
        powerText.text = card.power.ToString();
        healthText.text = card.health.ToString();
        currHP = card.health;
        background.color = card.color;
        if(card.traits.Count == 1){
            trait2.gameObject.SetActive(false);
            trait1.trait = card.traits[0];
            trait1.gameObject.SetActive(true);
        }
        else if(card.traits.Count == 2){
            trait2.trait = card.traits[1];
            trait2.gameObject.SetActive(true);
            trait1.trait = card.traits[0];
            trait1.gameObject.SetActive(true);
        }
        else{
            trait1.gameObject.SetActive(false);
            trait2.gameObject.SetActive(false);
        }
        rectTransform = GetComponent<RectTransform>();
    }
    void OnEnable(){
        EnemyTurnCardGameState.EnemyTurnEnded += OnEnemyTurnEnd;
        EnemyTurnCardGameState.EnemyTurnEnded += OnEnemyTurnBegin;
    }
    void OnDisable(){
        EnemyTurnCardGameState.EnemyTurnEnded -= OnEnemyTurnEnd;
        EnemyTurnCardGameState.EnemyTurnEnded -= OnEnemyTurnBegin;
    }
    void OnEnemyTurnEnd(){
        if(inSlot){
            canAttack = true;
            if(card.dblStrike){
                canDblAttack = true;
            }
        }
    }
    void OnEnemyTurnBegin(){
        if(inSlot){
            canAttack = true;
            if(card.dblStrike){
                canDblAttack = true;
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData){
        if(!inSlot && rectTransform.parent.GetComponent<PlayerHand>()!=null){
            rectTransform.parent.GetComponent<PlayerHand>().expand(true);
        }
    }
 
    public void OnPointerExit(PointerEventData eventData){
        if(!inSlot && rectTransform.parent.GetComponent<PlayerHand>()!=null){
            rectTransform.parent.GetComponent<PlayerHand>().expand(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button == PointerEventData.InputButton.Right){
            zoomCard?.Invoke(this);
        }
    }
}
