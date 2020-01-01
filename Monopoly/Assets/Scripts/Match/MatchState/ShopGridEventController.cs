﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopGridEventController : StageControllerBase
{
    public ShopGridEventController(FiniteStateMachine<MatchManager.MatchState> fsm)
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
        // Run GridEventFSM
        parentFSM.IssueCommand("ShopGridEvent-PostStage");

        base.Update(timeStep);
    }
}