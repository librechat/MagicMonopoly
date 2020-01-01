using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreStageController : StageControllerBase
{
    public PreStageController(FiniteStateMachine<MatchManager.MatchState> fsm)
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
        // feature: PlayerEnterEvent
        PlayerManager.CurrentPlayer.Buffs.Update(BuffData.BuffTiming.PreStage);

        parentFSM.IssueCommand("PreStage-MoveStage");
        
        base.Update(timeStep);
    }
}