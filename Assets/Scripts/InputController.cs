using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputController : MonoBehaviour
{
    public event Action PressedEndTurn = delegate { };
    public void EndTurn(){
        PressedEndTurn?.Invoke();
    }
}
