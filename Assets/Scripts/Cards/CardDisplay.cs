using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;

    public Text nameText;
    public Text costText;
    public Text powerText;
    public Text healthText;

    public Image background;

    public TraitDisplay trait1;
    public TraitDisplay trait2;
    void Start()
    {
        nameText.text = card.name;
        costText.text = card.cost.ToString();
        powerText.text = card.power.ToString();
        healthText.text = card.health.ToString();
        background.color = card.color;
        if(card.traits.Count == 1){
            trait2.gameObject.SetActive(false);
            trait1.trait = card.traits[0];
            trait1.gameObject.SetActive(true);
        }
        else if(card.traits.Count == 2){
            trait2.trait = card.traits[1];
            trait2.gameObject.SetActive(true);
            trait1.trait = card.traits[0];
            trait1.gameObject.SetActive(true);
        }
    }
}
