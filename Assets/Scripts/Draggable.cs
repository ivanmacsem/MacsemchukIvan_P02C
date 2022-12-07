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
    public Vector2 worldCenter;
    public GameObject target;
    private bool interactable = true;
    private bool drawingOne = true;
    private bool drawingTwo = false;
    public bool firstThree = true;
    public CardDisplay card;
    private CardsManager cardsManager;
    private CanvasGroup canvasGroup;
    private PlayerHand playerHand;
    public GameObject back;

    private void Awake(){
        cardsManager = FindObjectOfType<CardsManager>();
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        card = GetComponent<CardDisplay>();
        startingPos = rectTransform.anchoredPosition;
        target.SetActive(false);
    }
    void Update(){
        if(card.inSlot && !card.canAttack && !card.canDblAttack){
            interactable = false;
        }
        if(!firstThree){
            if(drawingOne){
                playerHand = rectTransform.parent.GetComponent<PlayerHand>();
                rectTransform.SetParent(playerHand.gameObject.GetComponent<RectTransform>().parent);
                rectTransform.anchoredPosition = new Vector2(1900, -900);
                rectTransform.Rotate(new Vector3(0,180,0));
                drawingOne = false;
                drawingTwo = true;
            }
            else if(drawingTwo){
                if(rectTransform.anchoredPosition.x > 1000){
                    interactable = false;
                    rectTransform.anchoredPosition -= new Vector2(870, 80)*Time.deltaTime*1.5f;
                    rectTransform.Rotate(new Vector3(0,0.6f,0));
                }
                else{
                    drawingTwo = false;
                    interactable = true;
                    rectTransform.SetParent(playerHand.gameObject.GetComponent<RectTransform>());
                    rectTransform.SetPositionAndRotation(rectTransform.position, Quaternion.identity);
                }
                if(rectTransform.rotation.y > 0.65){
                    back.SetActive(true);
                }
                else{
                    back.SetActive(false);
                }
            }
        }
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
                rectTransform.SetAsLastSibling();
                target.SetActive(true);
                Vector2 parentWorldPos = worldCenter + rectTransform.anchoredPosition;
                target.gameObject.GetComponent<RectTransform>().anchoredPosition = (eventData.position/canvas.scaleFactor - parentWorldPos);
                Debug.Log(parentWorldPos);
                Debug.Log(eventData.position/canvas.scaleFactor);
                Debug.Log(target.gameObject.GetComponent<RectTransform>().anchoredPosition);
                Debug.Log(rectTransform.anchoredPosition);
            }
            if(!card.inSlot && rectTransform.parent.GetComponent<PlayerHand>()!=null){
                playerHand = rectTransform.parent.GetComponent<PlayerHand>();
                playerHand.drag();
                rectTransform.anchoredPosition += new Vector2(0,165);
            }
        }
    }

    public void OnDrag(PointerEventData eventData){
        if(interactable){
            if(card.inSlot){
                target.gameObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta / canvas.scaleFactor;
            }
            else{
                rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            }
        }
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
                target.SetActive(false);
            }
            gameObject.layer = 5;
        }
    }
}