using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControllerBase : StateController<MatchManager.MatchState>
{
    public event Action onEnterUIEvent;
    public event Action onExitUIEvent;

    public StageControllerBase(FiniteStateMachine<MatchManager.MatchState> fsm)
        : base(fsm)
    {
        
    }

    public override void Enter()
    {
        onEnterUIEvent();
        base.Enter();
    }
    public override void Exit()
    {
        onExitUIEvent();
        base.Exit();
    }
    public override void Update(float timeStep)
    {
        base.Update(timeStep);
    }
}
