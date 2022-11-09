using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupCardGameState : CardGameState
{
    bool _activated = false;

    public override void Enter(){
        StateMachine.CardsManager.Setup(0, 1);
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
