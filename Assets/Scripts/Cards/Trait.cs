using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="NewTrait", menuName ="Trait")]
public class Trait : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite icon;
}
