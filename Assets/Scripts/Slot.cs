using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    public CardsManager cardsManager;
    public RectTransform parent;
    private CardDisplay card;
    public void OnDrop(PointerEventData eventData){
        Debug.Log("OnDrop");
        if(eventData.pointerDrag !=null){
            if(!eventData.pointerDrag.GetComponent<Draggable>().inSlot){
                if(cardsManager.Dropped(this)){
                        card = eventData.pointerDrag.GetComponent<CardDisplay>();
                        eventData.pointerDrag.GetComponent<Draggable>().inSlot = true;
                        eventData.pointerDrag.GetComponent<RectTransform>().SetParent(parent);
                        eventData.pointerDrag.GetComponent<RectTransform>().localPosition = this.GetComponent<RectTransform>().localPosition;
                }
            }
        }
    }
}
