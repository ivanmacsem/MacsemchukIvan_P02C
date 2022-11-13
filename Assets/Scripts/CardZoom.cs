using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardZoom : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject zoomedCard;
    private CardDisplay card;
    private Image blur;

    void Awake(){
        blur = GetComponent<Image>();
        card = zoomedCard.GetComponent<CardDisplay>();
        CardDisplay.zoomCard += Zoom;
    }

    void Zoom(CardDisplay toZoom){
        card = zoomedCard.GetComponent<CardDisplay>();
        card.card = toZoom.card;
        blur.enabled = true;
        card.Start();
        card.currHP = toZoom.currHP;
        card.healthText.text = card.currHP.ToString();
        zoomedCard.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData){
        blur.enabled = false;
        zoomedCard.SetActive(false);
    }
}
