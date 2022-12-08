using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDropHandler
{
    public Card card;
    private RectTransform rectTransform;
    public static event Action<CardDisplay> zoomCard = delegate { };
    public static event Action<CardDisplay> destroyed = delegate { };
    public Text nameText;
    public Text costText;
    public Text powerText;
    public Text healthText;

    public Image background;
    [SerializeField] Image shieldImg;
    [SerializeField] Image tauntImg;
    [SerializeField] Image borderImg;

    public TraitDisplay trait1;
    public TraitDisplay trait2;
    public bool inSlot = false;
    public bool isPlayer = false;
    public int currHP = 0;
    public bool canAttack = false;
    public bool canDblAttack = false;
    public bool isShielded = false;
    public bool hasShielded = false;
    public bool isTaunted = false;
    private RectTransform toAttack;
    private Vector2 attackDirection;
    private Vector2 startingPos;
    public bool attacking = false;
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
        isShielded = card.shield;
        shieldImg.gameObject.SetActive(isShielded);
        tauntImg.gameObject.SetActive(card.taunt);
    }
    public void Update(){
        healthText.text = currHP.ToString();
        if(canAttack || canDblAttack){
            borderImg.color = Color.white;
        }
        else{
            borderImg.color = Color.grey;
        }
        if(attacking){
            Vector2 worldPos = new Vector2(1100, 640) + rectTransform.anchoredPosition;
            Vector2 toHitWorldPos = new Vector2(1100, 640) + toAttack.anchoredPosition;
            rectTransform.anchoredPosition += attackDirection*Time.deltaTime*3f;
            if((toHitWorldPos-worldPos).magnitude < 25){
                attacking = false;
                rectTransform.anchoredPosition = startingPos;
            }
        }
    }
    void OnEnable(){
        EnemyTurnCardGameState.EnemyTurnEnded += OnEnemyTurnEnd;
        EnemyTurnCardGameState.EnemyTurnEnded += OnEnemyTurnBegin;

        if(isPlayer){
            EnemyTurnCardGameState.TauntPlayer += OnTaunt;
            PlayerTurnCardGameState.TauntPlayer += OnTaunt;
        }
        else{
            EnemyTurnCardGameState.TauntEnemy += OnTaunt;
            PlayerTurnCardGameState.TauntEnemy += OnTaunt;
        }
    }
    void OnDisable(){
        EnemyTurnCardGameState.EnemyTurnEnded -= OnEnemyTurnEnd;
        EnemyTurnCardGameState.EnemyTurnEnded -= OnEnemyTurnBegin;

        if(isPlayer){
            EnemyTurnCardGameState.TauntPlayer -= OnTaunt;
            PlayerTurnCardGameState.TauntPlayer -= OnTaunt;
        }
        else{
            EnemyTurnCardGameState.TauntEnemy -= OnTaunt;
            PlayerTurnCardGameState.TauntEnemy -= OnTaunt;
        }
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
    void OnTaunt(bool taunted){
        isTaunted = taunted;
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

    public void OnDrop(PointerEventData eventData)
    {
        if((!eventData.pointerDrag.GetComponent<CardDisplay>().isTaunted) || (eventData.pointerDrag.GetComponent<CardDisplay>().isTaunted && card.taunt)){
            if(!isPlayer){
                if(eventData.pointerDrag.GetComponent<CardDisplay>().canAttack){
                    this.TakeDamage(eventData.pointerDrag.GetComponent<CardDisplay>().card.power, false); 
                    eventData.pointerDrag.GetComponent<CardDisplay>().TakeDamage(card.power, true);
                    eventData.pointerDrag.GetComponent<CardDisplay>().canAttack = false;
                }
                else if(eventData.pointerDrag.GetComponent<CardDisplay>().canDblAttack){
                    this.TakeDamage(eventData.pointerDrag.GetComponent<CardDisplay>().card.power, false);
                    eventData.pointerDrag.GetComponent<CardDisplay>().TakeDamage(card.power, true);
                    eventData.pointerDrag.GetComponent<CardDisplay>().canDblAttack = false;
                }
            }
        }
    }
    public IEnumerator AttackAnimation(RectTransform toHit){
        attacking = true;
        toAttack = toHit.GetComponent<RectTransform>();
        Vector2 worldPos = new Vector2(1100, 640) + rectTransform.anchoredPosition;
        Vector2 toHitWorldPos = new Vector2(1100, 640) + toAttack.anchoredPosition;
        attackDirection = toHitWorldPos-worldPos;
        startingPos = rectTransform.anchoredPosition;
        do
        {
            yield return null;
        } while ( attacking );
    }

    public void TakeDamage(int dmg, bool attacking){
        if(!(attacking && card.ranged)){
            if(!isShielded){
                currHP -= dmg;
            }
            else{
                shieldImg.gameObject.SetActive(false);
                isShielded = false;
            }
        }
        if(currHP <= 0){
            destroyed?.Invoke(this);
        }
    }
}
