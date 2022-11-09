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

    public bool inSlot = false;
    private bool interactible = true;
    public CardDisplay cardDisplay;
    private CardsManager cardsManager;
    private CanvasGroup canvasGroup;

    private void Awake(){
        cardsManager = FindObjectOfType<CardsManager>();
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
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
        interactible = false;
    }

    private void OnEnemyTurnEnded(){
        interactible = true;
    }

    public void OnBeginDrag(PointerEventData eventData){
        if(interactible){
            canvasGroup.alpha = 0.7f;
            canvasGroup.blocksRaycasts = false;
            startingPos = rectTransform.anchoredPosition;
            if(inSlot){
                startingSlot = rectTransform.localPosition;
            }
        }
    }

    public void OnDrag(PointerEventData eventData){
        if(interactible){rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;}
    }

    public void OnEndDrag(PointerEventData eventData){
        if(interactible){
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            if(!inSlot){
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