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

    public bool canAttack = false;

}
