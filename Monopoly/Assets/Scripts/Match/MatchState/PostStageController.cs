using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostStageController : StageControllerBase
{
    public PostStageController(FiniteStateMachine<MatchManager.MatchState> fsm)
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
        // feature: PlayerExitEvent
        PlayerManager.CurrentPlayer.Buffs.Update(BuffData.BuffTiming.PostStage);

        PlayerManager.SwitchToNextPlayer();
        parentFSM.IssueCommand("PostStage-PreStage");        
        base.Update(timeStep);
    }
}