using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLoseCardGameState : CardGameState
{
    [SerializeField] GameObject _loseScreenUI = null;
    public override void Enter(){
        Debug.Log("Lose: Enter");
        StopAllCoroutines();
        _loseScreenUI.SetActive(true);
    }
}
