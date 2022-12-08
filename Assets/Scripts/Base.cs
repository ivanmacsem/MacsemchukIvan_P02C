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
    public Text healthTxt;
    public bool interactable = false;
    public int dmg;

    void Start(){
        baseImg = GetComponent<Image>();
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
