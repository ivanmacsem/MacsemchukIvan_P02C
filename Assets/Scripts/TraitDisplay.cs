using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitDisplay : MonoBehaviour
{
    public Trait trait;

    private Image icon;
    public Text traitTxt;
    public List<Sprite> traitIcons = new List<Sprite>();
    private string[] traitList = new string[] {"Rush", "Shield", "Spray", "Banish", "Callback", "Ranged", "Double Strike", "Taunt"};

    void Awake(){
        icon = GetComponent<Image>();
    }
    void OnEnable(){
        int idx = -1;
        for(int i = 0; i<traitList.Length; i++){
            if(trait.name == traitList[i]){
                idx = i;
                break;
            }
        }
        icon.sprite = traitIcons[idx];
        icon.gameObject.SetActive(true);
        traitTxt.text = trait.name;
    }
}
