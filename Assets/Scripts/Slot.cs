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
        if(eventData.pointerDrag !=null){
            if(!eventData.pointerDrag.GetComponent<CardDisplay>().inSlot){
                if(cardsManager.Dropped(this)){
                        card = eventData.pointerDrag.GetComponent<CardDisplay>();
                        eventData.pointerDrag.GetComponent<CardDisplay>().inSlot = true;
                        eventData.pointerDrag.GetComponent<RectTransform>().SetParent(parent);
                        eventData.pointerDrag.GetComponent<RectTransform>().localPosition = this.GetComponent<RectTransform>().localPosition;
                }
            }
        }
    }
}
