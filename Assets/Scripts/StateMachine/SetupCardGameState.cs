using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupCardGameState : CardGameState
{
    [SerializeField] int _startingCardNumber = 25;

    bool _activated = false;

    public override void Enter(){
        Debug.Log("Setup: Entering");
        _activated = false;
    }

    public override void Tick()
    {
        if(_activated == false){
            _activated = true;
            StateMachine.ChangeState<PlayerTurnCardGameState>();
        }
    }

    public override void Exit()
    {
        _activated = false;
        Debug.Log("Setup: Exiting");
    }
}
