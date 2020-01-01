using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BasicGridEventController : StageControllerBase
{
    public enum BasicGridStageState
    {
        Begin,

        Build,
        Trapped,
        Upgrade,
        Destroy,
        
        Leave
    }

    private FiniteStateMachine<BasicGridStageState> m_StateMachine;

    public event Action<Player, Action> onEnterBuildUIEvent;
    public event Action onExitBuildUIEvent;

    public event Action onEnterTrappedUIEvent;
    public event Action onExitTrappedUIEvent;

    public event Action<Player, Action> onEnterUpgradeUIEvent;
    public event Action onExitUpgradeUIEvent;

    public event Action<Player, Action> onEnterDestroyUIEvent;
    public event Action onExitDestroyUIEvent;

    private BasicGrid currentGrid;
    private Player currentPlayer;
    
    public BasicGridEventController(FiniteStateMachine<MatchManager.MatchState> fsm)
        : base(fsm)
    {
        m_StateMachine = FiniteStateMachine<BasicGridStageState>.FromEnum();

        m_StateMachine
            .AddTransition(BasicGridStageState.Begin, BasicGridStageState.Build)
            .AddTransition(BasicGridStageState.Build, BasicGridStageState.Leave)
            .AddTransition(BasicGridStageState.Begin, BasicGridStageState.Trapped)
            .AddTransition(BasicGridStageState.Trapped, BasicGridStageState.Upgrade)
            .AddTransition(BasicGridStageState.Upgrade, BasicGridStageState.Leave)
            .AddTransition(BasicGridStageState.Trapped, BasicGridStageState.Destroy)
            .AddTransition(BasicGridStageState.Destroy, BasicGridStageState.Leave);

        // Begin state
        m_StateMachine.OnUpdate(BasicGridStageState.Begin, (float timeStep) => {

            currentPlayer = PlayerManager.CurrentPlayer;
            currentGrid = GridManager.GetGrid(currentPlayer.Position) as BasicGrid;
            
            int owner = currentGrid.OwnerId;
            if(owner == -1) m_StateMachine.IssueCommand("Begin-Build");
            else m_StateMachine.IssueCommand("Begin-Trapped");
        });

        // Build state
        m_StateMachine
            .OnEnter(BasicGridStageState.Build, () =>
            {
                onEnterBuildUIEvent(PlayerManager.CurrentPlayer, buildCompleted);
            })
            .OnExit(BasicGridStageState.Build, () =>
            {
                onExitBuildUIEvent();
            });

        // Trapped state
        m_StateMachine
            .OnEnter(BasicGridStageState.Trapped, () =>
            {
                currentGrid.AttachedCircle.Trapped(currentPlayer);
                
                onEnterTrappedUIEvent();
            })
            .OnUpdate(BasicGridStageState.Trapped, (float timeStep) =>
            {
                int owner = currentGrid.OwnerId;
                if (owner == currentPlayer.PlayerId) m_StateMachine.IssueCommand("Trapped-Upgrade");
                else m_StateMachine.IssueCommand("Trapped-Destroy");
            })
            .OnExit(BasicGridStageState.Trapped, () =>
            {
                onExitTrappedUIEvent();
            });

        // Upgrade state
        m_StateMachine
            .OnEnter(BasicGridStageState.Upgrade, () =>
            {
                // show update panel
                onEnterUpgradeUIEvent(PlayerManager.CurrentPlayer, upgradeCompleted);
            })
            .OnExit(BasicGridStageState.Upgrade, () =>
            {
                onExitUpgradeUIEvent();
            });

        // Destroy state
        m_StateMachine
            .OnEnter(BasicGridStageState.Destroy, () =>
            {
                // show destroy spell panel
                onEnterDestroyUIEvent(PlayerManager.CurrentPlayer, destroyCompleted);
            })
            .OnExit(BasicGridStageState.Destroy, () =>
            {
                onExitDestroyUIEvent();
            });       

        // Leave
        m_StateMachine.OnEnter(BasicGridStageState.Leave, () => {
            parentFSM.IssueCommand("BasicGridEvent-PostStage");
        });

    }

    public override void Enter()
    {
        m_StateMachine.Begin(BasicGridStageState.Begin);
        
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

    private void buildCompleted()
    {
        m_StateMachine.IssueCommand("Build-Leave");
    }
    private void upgradeCompleted()
    {
        m_StateMachine.IssueCommand("Upgrade-Leave");
    }
    private void destroyCompleted()
    {
        m_StateMachine.IssueCommand("Destroy-Leave");
    }
}