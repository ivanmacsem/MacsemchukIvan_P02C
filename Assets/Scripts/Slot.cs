using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IDropHandler
{
    public static event Action<Slot, CardDisplay> drop = delegate { };
    public CardsManager cardsManager;
    public bool player;
    public RectTransform parent;
    public void OnDrop(PointerEventData eventData){
        if(eventData.pointerDrag !=null){
            if(!eventData.pointerDrag.GetComponent<CardDisplay>().inSlot){
                drop?.Invoke(this, eventData.pointerDrag.GetComponent<CardDisplay>());
            }
        }
    }
}
