using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitDisplay : MonoBehaviour
{
    public Trait trait;

    private Image icon;
    public Text traitTxt;

    void Awake(){
        icon = GetComponent<Image>();
    }
    void OnEnable(){
        icon.sprite = trait.icon;
        icon.gameObject.SetActive(true);
        traitTxt.text = trait.name;
    }
}
