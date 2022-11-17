using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWinCardGameState : CardGameState
{
    [SerializeField] GameObject _winScreenUI = null;
    public override void Enter(){
        Debug.Log("Win: Enter");
        StopAllCoroutines();
        _winScreenUI.SetActive(true);
    }
}
