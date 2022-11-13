using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerHand : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] float transitionSpeed = 1f;
    private bool expanding = false;
    private bool dragging = false;

    void Start(){
        rectTransform = GetComponent<RectTransform>();
    }
    void FixedUpdate(){
        if(!dragging){
            if(expanding){
                if(rectTransform.anchoredPosition.y < -350){
                    rectTransform.anchoredPosition += new Vector2(0, transitionSpeed);
                }
            }
            else{
                if(rectTransform.anchoredPosition.y > -500){
                    rectTransform.anchoredPosition -= new Vector2(0, transitionSpeed);
                }
            }
        }
    }
    public void expand(bool isHovered){
        expanding = isHovered;
    }
    public void drag(){
        rectTransform.anchoredPosition = new Vector2(0, -500);
        dragging = true;
    }
    public void endDrag(){
        dragging = false;
    }
}
