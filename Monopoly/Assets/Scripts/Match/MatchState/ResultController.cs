using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultController : StageControllerBase
{
    public ResultController(FiniteStateMachine<MatchManager.MatchState> fsm)
        : base(fsm)
    {


    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update(float timeStep)
    {
        base.Update(timeStep);
    }
}