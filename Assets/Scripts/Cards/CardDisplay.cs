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
    public static event Action<CardDisplay, bool> destroyed = delegate { };
    public static event Action cardDmg = delegate { };
    public Text nameText;
    public Text costText;
    public Text powerText;
    public Text healthText;
    public Text dmgTakenText;
    private int startingFont = 127;

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
    private bool attackingMe;
    private bool expanding;
    private bool useMe = false;
    private RectTransform toAttack;
    private Vector2 attackDirection;
    private Vector2 startingPos;
    public bool attacking = false;
    public bool takingDamage = false;
    public void Start()
    {
        nameText.text = card.name;
        costText.text = card.cost.ToString();
        powerText.text = card.power.ToString();
        healthText.text = card.health.ToString();
        currHP = card.health;
        background.sprite = card.tribeIcon;
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
        if(!isPlayer){
            borderImg.color = Color.red;
        }
    }
    public void Update(){
        healthText.text = currHP.ToString();
        if(isPlayer){
            if(canAttack || canDblAttack){
                borderImg.color = Color.white;
            }
            else{
                useMe = false;
                borderImg.color = Color.grey;
            }
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
    void FixedUpdate(){
        if((!isPlayer && inSlot && attackingMe) || useMe){
            if(rectTransform.localScale.x < 1.05f && rectTransform.localScale.x > 0.95f){
                if(expanding){
                    rectTransform.localScale += new Vector3(Time.deltaTime*0.25f, Time.deltaTime*0.25f, 0);
                }
                else{
                    rectTransform.localScale -= new Vector3(Time.deltaTime*0.25f, Time.deltaTime*0.25f, 0);
                }
            }
            else{
                if(expanding){
                    rectTransform.localScale -= new Vector3(Time.deltaTime*0.25f, Time.deltaTime*0.25f, 0);
                    expanding = false;
                }
                else{
                    rectTransform.localScale += new Vector3(Time.deltaTime*0.25f, Time.deltaTime*0.25f, 0);
                    expanding = true;
                }
            }
        }
        else if(takingDamage){
            if(dmgTakenText.fontSize>35){
                dmgTakenText.fontSize -= 1;
            }
            rectTransform.anchoredPosition += new Vector2(Mathf.Sin(Time.time * 35f) * 2f, 0);
        }
    }
    void OnEnable(){
        EnemyTurnCardGameState.EnemyTurnEnded += OnEnemyTurnEnd;
        EnemyTurnCardGameState.EnemyTurnBegan += OnEnemyTurnBegin;

        if(isPlayer){
            EnemyTurnCardGameState.TauntPlayer += OnTaunt;
            PlayerTurnCardGameState.TauntPlayer += OnTaunt;
            Draggable.startDraggingTaunted += DontUseMe;
            Draggable.startDragging += DontUseMe;
            Draggable.endDragging += UseMe;
        }
        else{
            EnemyTurnCardGameState.TauntEnemy += OnTaunt;
            PlayerTurnCardGameState.TauntEnemy += OnTaunt;
            Draggable.startDragging += AttackMe;
            Draggable.endDragging += StopAttackingMe;
            if(card.taunt){
                Draggable.startDraggingTaunted += AttackMe;
            }
        }
    }
    void OnDisable(){
        EnemyTurnCardGameState.EnemyTurnEnded -= OnEnemyTurnEnd;
        EnemyTurnCardGameState.EnemyTurnBegan -= OnEnemyTurnBegin;

        if(isPlayer){
            EnemyTurnCardGameState.TauntPlayer -= OnTaunt;
            PlayerTurnCardGameState.TauntPlayer -= OnTaunt;
            Draggable.startDraggingTaunted -= DontUseMe;
            Draggable.startDragging -= DontUseMe;
            Draggable.endDragging -= UseMe;
        }
        else{
            EnemyTurnCardGameState.TauntEnemy -= OnTaunt;
            PlayerTurnCardGameState.TauntEnemy -= OnTaunt;
            Draggable.startDragging -= AttackMe;
            Draggable.endDragging -= StopAttackingMe;
            if(card.taunt){
                Draggable.startDraggingTaunted -= AttackMe;
            }
        }
    }
    void OnEnemyTurnEnd(){
        if(inSlot){
            if(isPlayer){
                canAttack = true;
                if(card.dblStrike){
                    canDblAttack = true;
                }
                useMe = true;
            }
            else{
                canAttack = false;
                if(card.dblStrike){
                    canDblAttack = false;
                }
            }
        }
    }
    void OnEnemyTurnBegin(){
        if(inSlot){
            if(!isPlayer){
                canAttack = true;
                if(card.dblStrike){
                    canDblAttack = true;
                }
                useMe = false;
            }
            else{
                canAttack = false;
                if(card.dblStrike){
                    canDblAttack = false;
                }
            }
        }
    }
    void AttackMe(){
        attackingMe = true;
    }
    void StopAttackingMe(){
        attackingMe = false;
        rectTransform.localScale = new Vector3(1, 1, 1);
        expanding = true;
    }
    void DontUseMe(){
        useMe = false;
        rectTransform.localScale = new Vector3(1, 1, 1);
        expanding = true;
    }
    void UseMe(){
        if(canAttack || canDblAttack){
            useMe = true;
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
            if(!isPlayer && inSlot){
                if(eventData.pointerDrag.GetComponent<CardDisplay>().canAttack){
                    StartCoroutine(this.TakeDamage(eventData.pointerDrag.GetComponent<CardDisplay>().card.power, false)); 
                    StartCoroutine(eventData.pointerDrag.GetComponent<CardDisplay>().TakeDamage(card.power, true));
                    eventData.pointerDrag.GetComponent<CardDisplay>().canAttack = false;
                }
                else if(eventData.pointerDrag.GetComponent<CardDisplay>().canDblAttack){
                    StartCoroutine(this.TakeDamage(eventData.pointerDrag.GetComponent<CardDisplay>().card.power, false));
                    StartCoroutine(eventData.pointerDrag.GetComponent<CardDisplay>().TakeDamage(card.power, true));
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

    public IEnumerator TakeDamage(int dmg, bool attacking){
        if(!(attacking && card.ranged)){
            if(!isShielded){
                currHP -= dmg;
                dmgTakenText.fontSize = startingFont;
                dmgTakenText.gameObject.SetActive(true);
                dmgTakenText.text = "-" + dmg;
            }
            else{
                shieldImg.gameObject.SetActive(false);
                isShielded = false;
            }
            takingDamage = true;
            cardDmg?.Invoke();
            yield return new WaitForSeconds(0.5f);
            takingDamage = false;
            dmgTakenText.gameObject.SetActive(false);
        }
        if(currHP <= 0){
            if(dmg > 10){
                destroyed?.Invoke(this, true);
            }
            else{
                destroyed?.Invoke(this, false);
            }
        }
    }
}
