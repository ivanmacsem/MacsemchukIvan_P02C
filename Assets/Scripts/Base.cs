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
    private RectTransform rectTransform;
    public Text healthTxt;
    public bool interactable = false;
    public int dmg;
    private bool attackingMe = false;
    private bool expanding;

    void Start(){
        baseImg = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        Draggable.startDragging += AttackMe;
        Draggable.endDragging += StopAttackingMe;
    }
    void AttackMe(){
        Debug.Log("attackMyBase");
        attackingMe = true;
    }
    void StopAttackingMe(){
        Debug.Log("DontAttackMyBase");
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
    }

    public void OnDrop(PointerEventData eventData){
        if(interactable){
            if(eventData.pointerDrag !=null){
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
}
