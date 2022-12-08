using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Base : MonoBehaviour, IDropHandler
{
    public event Action BaseAttacked;
    private Image baseImg;
    public GameObject backgroundParent;
    private RectTransform rectTransform;
    public Text healthTxt;
    public Text dmgText;
    private int startingFont = 127;
    public bool interactable = false;
    public int dmg;
    private bool takingDamage = false;
    private bool attackingMe = false;
    private bool expanding;

    void Start(){
        baseImg = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        Draggable.startDragging += AttackMe;
        Draggable.endDragging += StopAttackingMe;
    }
    void AttackMe(){
        attackingMe = true;
    }
    void StopAttackingMe(){
        attackingMe = false;
        rectTransform.localScale = new Vector3(1, 1, 1);
    }
    void FixedUpdate(){
        if(attackingMe && interactable){
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
            if(dmgText.fontSize > 100){
                dmgText.fontSize -= 1;
                backgroundParent.GetComponent<RectTransform>().anchoredPosition += new Vector2(Mathf.Sin(Time.time * 35f) * 2f, 0);
            }
            else{
                takingDamage = false;
                dmgText.gameObject.SetActive(false);
            }
        }

    }

    public void OnDrop(PointerEventData eventData){
        if(interactable){
            if(!eventData.pointerDrag.GetComponent<CardDisplay>().isTaunted){
                TakeDmgAnimation(eventData.pointerDrag.GetComponent<CardDisplay>().card.power);
                if(eventData.pointerDrag.GetComponent<CardDisplay>().canAttack){
                    dmg = eventData.pointerDrag.GetComponent<CardDisplay>().card.power;
                    eventData.pointerDrag.GetComponent<CardDisplay>().canAttack = false;
                    BaseAttacked?.Invoke();
                }
                else if(eventData.pointerDrag.GetComponent<CardDisplay>().canDblAttack){
                    dmg = eventData.pointerDrag.GetComponent<CardDisplay>().card.power;
                    eventData.pointerDrag.GetComponent<CardDisplay>().canDblAttack = false;
                    BaseAttacked?.Invoke();
                }
            }
        }
    }
    public void TakeDmgAnimation(int dmg){
        takingDamage = true;
        dmgText.fontSize = startingFont;
        dmgText.text = "-" + dmg;
        dmgText.gameObject.SetActive(true);
    }
}
