using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private Vector2 startingPos;
    public Vector2 startingSlot;
    private bool interactable = true;
    public CardDisplay card;
    private CardsManager cardsManager;
    private CanvasGroup canvasGroup;
    private PlayerHand playerHand;

    private void Awake(){
        cardsManager = FindObjectOfType<CardsManager>();
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        card = GetComponent<CardDisplay>();
        startingPos = rectTransform.anchoredPosition;
    }

    private void OnEnable(){
        EnemyTurnCardGameState.EnemyTurnBegan += OnEnemyTurnBegin;
        EnemyTurnCardGameState.EnemyTurnEnded += OnEnemyTurnEnded;
    }

    private void OnDisable(){
        EnemyTurnCardGameState.EnemyTurnBegan -= OnEnemyTurnBegin;
        EnemyTurnCardGameState.EnemyTurnEnded -= OnEnemyTurnEnded;
    }

    private void OnEnemyTurnBegin(){
        interactable = false;
    }

    private void OnEnemyTurnEnded(){
        interactable = true;
    }

    public void OnBeginDrag(PointerEventData eventData){
        if(interactable){
            canvasGroup.alpha = 0.9f;
            canvasGroup.blocksRaycasts = false;
            startingPos = rectTransform.anchoredPosition;
            if(card.inSlot){
                startingSlot = rectTransform.localPosition;
            }
            if(!card.inSlot && rectTransform.parent.GetComponent<PlayerHand>()!=null){
                playerHand = rectTransform.parent.GetComponent<PlayerHand>();
                playerHand.drag();
                rectTransform.anchoredPosition += new Vector2(0,165);
            }
        }
    }

    public void OnDrag(PointerEventData eventData){
        if(interactable){rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;}
    }

    public void OnEndDrag(PointerEventData eventData){
        playerHand?.endDrag();
        if(interactable){
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            if(!card.inSlot){
                rectTransform.anchoredPosition = startingPos;
            }
            else{
                if(!startingSlot.Equals(new Vector2(0,0))){
                    rectTransform.localPosition = startingSlot;
                }
            }
        }
    }
}