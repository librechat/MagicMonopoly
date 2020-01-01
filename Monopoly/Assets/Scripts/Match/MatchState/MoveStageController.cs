using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStageController : StageControllerBase
{
    public enum MoveStageState
    {
        Begin,
        Draw,
        Branch,
        Leave
    }

    private FiniteStateMachine<MoveStageState> m_StateMachine;

    public event Action onEnterDrawUIEvent;
    public event Action onExitDrawUIEvent;

    public event Action onEnterBranchUIEvent;
    public event Action onExitBranchUIEvent;

    public MoveStageController(FiniteStateMachine<MatchManager.MatchState> fsm): base(fsm)
    {
        m_StateMachine = FiniteStateMachine<MoveStageState>.FromEnum();

        m_StateMachine
            .AddTransition(MoveStageState.Begin, MoveStageState.Draw)
            .AddTransition(MoveStageState.Draw, MoveStageState.Branch)
            .AddTransition(MoveStageState.Branch, MoveStageState.Branch)
            .AddTransition(MoveStageState.Draw, MoveStageState.Leave)
            .AddTransition(MoveStageState.Branch, MoveStageState.Leave);
        
        // Begin state
        m_StateMachine.OnUpdate(MoveStageState.Begin, (float timeStep) => {
            m_StateMachine.IssueCommand("Begin-Draw");
        });

        // Draw state
        m_StateMachine
            .OnEnter(MoveStageState.Draw, () =>
            {
                onEnterDrawUIEvent();
            })
            .OnExit(MoveStageState.Draw, () => {
                onExitDrawUIEvent();
            });

        // Branch state
        m_StateMachine
            .OnEnter(MoveStageState.Branch, () =>
            {
                onEnterBranchUIEvent();
            })
            .OnExit(MoveStageState.Branch, () =>
            {
                onExitBranchUIEvent();
            });

        // Leave state
        m_StateMachine.OnEnter(MoveStageState.Leave, () => {
            GridBase currentGrid = GridManager.GetGrid(PlayerManager.CurrentPlayer.Position);
            switch (currentGrid.Type)
            {
                case GridTypeData.GridTypeName.Basic:
                    parentFSM.IssueCommand("MoveStage-BasicGridEvent");
                    break;
                case GridTypeData.GridTypeName.Shop:
                    parentFSM.IssueCommand("MoveStage-ShopGridEvent");
                    break;
                default:
                    parentFSM.IssueCommand("MoveStage-IdleGridEvent");
                    break;
            }
            
        });
    }

    public override void Enter()
    {
        m_StateMachine.Begin(MoveStageState.Begin);
        PlayerManager.CurrentPlayer.StepToMove = 0;
        
        base.Enter();
    }

    public override void Exit()
    {        
        base.Exit();
    }

    public override void Update(float timeStep)
    {
        m_StateMachine.Update(timeStep);
        base.Update(timeStep);
    }

    // Input callbacks
    public void OnDiceDrawed()
    {
        PlayerManager.CurrentPlayer.StepToMove = UnityEngine.Random.Range(1, 7);

        PlayerManager.CurrentPlayer.Buffs.Update(BuffData.BuffTiming.Move);

        bool completed = PlayerManager.CurrentPlayer.Move(
            () => { m_StateMachine.IssueCommand("Draw-Leave"); },
            (int remain) => { m_StateMachine.IssueCommand("Draw-Branch"); });
    }

    public void OnBranchSelected(int branch)
    {
        Debug.Log("Remain step: " + PlayerManager.CurrentPlayer.StepToMove);

        bool completed = PlayerManager.CurrentPlayer.Move(
            () => { m_StateMachine.IssueCommand("Branch-Leave"); },
            (int remain) => { m_StateMachine.IssueCommand("Branch-Branch"); }, branch);
    }
}
