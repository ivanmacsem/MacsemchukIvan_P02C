using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="NewCard", menuName ="Card")]
public class Card : ScriptableObject
{
    public new string name;

    public Sprite tribeIcon;
    public Color color;

    public List<Trait> traits = new List<Trait>();

    public int cost;
    public int power;
    public int health;

    public bool dblStrike = false;
    public bool shield = false;
    public bool banish = false;
    public bool callback = false;
    public bool ranged = false;
    public bool rush = false;
    public bool spray = false;
    public bool taunt = false;

    void Awake(){
        foreach(Trait t in traits){
            if(t.name=="Double Strike"){
                dblStrike = true;
            }
            else if(t.name=="Shield"){
                shield = true;
            }
            else if(t.name=="Banish"){
                banish = true;
            }
            else if(t.name=="Callback"){
                callback = true;
            }
            else if(t.name=="Ranged"){
                ranged = true;
            }
            else if(t.name=="Rush"){
                rush = true;
            }
            else if(t.name=="Spray"){
                spray = true;
            }
            else if(t.name=="Taunt"){
                taunt = true;
            }
        }
    }

}
