using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public enum MatchState {
        Prepare = 0,
        PreStage,
        MoveStage,

        IdleGridEvent,
        BasicGridEvent,
        ShopGridEvent,        

        PostStage,
        Result
    };

    public static List<MatchState> StateList
    {
        get { return ((MatchState[])Enum.GetValues(typeof(MatchState))).ToList(); }
    }
    public Dictionary<MatchState, StageControllerBase> StateControllers
    {
        get
        {
            Dictionary<MatchState, StageControllerBase> dic = new Dictionary<MatchState, StageControllerBase>();
            Dictionary<MatchState, StateController<MatchState>> states = m_MatchStateMachine.States;
            List<MatchManager.MatchState> stateNames = ((MatchManager.MatchState[])Enum.GetValues(typeof(MatchManager.MatchState))).ToList();

            for (int i = 0; i < stateNames.Count; i++)
            {
                dic.Add(stateNames[i], states[stateNames[i]] as StageControllerBase);
            }
            return dic;
        }
    }
    private FiniteStateMachine<MatchState> m_MatchStateMachine;

    private PlayerManager m_PlayerManager;
    private GridManager m_GridManager;

    #region DisplayCallbacks
    public Action<MatchManager, PlayerManager, GridManager> onInitDisplay;
    #endregion

    private static MatchManager s_Instance;

    #region MonoBehavior Functions
    private void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(this);
            return;
        }
        s_Instance = this;

        init();
    }

    private void Update()
    {
        float timeStep = Time.deltaTime;
        m_MatchStateMachine.Update(timeStep);
    }
    #endregion

    private void init()
    {
        m_PlayerManager = GetComponent<PlayerManager>();
        m_GridManager = GetComponent<GridManager>();
        
        initMatchFSM();

        onInitDisplay(this, m_PlayerManager, m_GridManager);

        m_MatchStateMachine.Begin(MatchState.Prepare);
    }

    private void initMatchFSM()
    {
        m_MatchStateMachine = FiniteStateMachine<MatchState>.FromEnum();

        // Transitions
        m_MatchStateMachine.AddTransition(MatchState.Prepare, MatchState.PreStage)
            .AddTransition(MatchState.PreStage, MatchState.MoveStage)
            .AddTransition(MatchState.MoveStage, MatchState.IdleGridEvent)
            .AddTransition(MatchState.MoveStage, MatchState.BasicGridEvent)
            .AddTransition(MatchState.MoveStage, MatchState.ShopGridEvent)
            .AddTransition(MatchState.IdleGridEvent, MatchState.PostStage)
            .AddTransition(MatchState.BasicGridEvent, MatchState.PostStage)
            .AddTransition(MatchState.ShopGridEvent, MatchState.PostStage)
            .AddTransition(MatchState.PostStage, MatchState.PreStage)
            .AddTransition(MatchState.PostStage, MatchState.Result);

        // Behaviors of each state
        Action<MatchState, Action, Action, Action<float>> setStateBehavior =
            (MatchState state, Action onEnter, Action onExit, Action<float> onUpdate) =>
            {
                m_MatchStateMachine.OnEnter(state, onEnter)
                    .OnExit(state, onExit)
                    .OnUpdate(state, onUpdate);
            };

        m_MatchStateMachine.SetStateController(MatchState.Prepare, new PrepareController(m_MatchStateMachine));
        m_MatchStateMachine.SetStateController(MatchState.PreStage, new PreStageController(m_MatchStateMachine));
        m_MatchStateMachine.SetStateController(MatchState.MoveStage, new MoveStageController(m_MatchStateMachine));
        m_MatchStateMachine.SetStateController(MatchState.IdleGridEvent, new IdleGridEventController(m_MatchStateMachine));
        m_MatchStateMachine.SetStateController(MatchState.BasicGridEvent, new BasicGridEventController(m_MatchStateMachine));
        m_MatchStateMachine.SetStateController(MatchState.ShopGridEvent, new ShopGridEventController(m_MatchStateMachine));
        m_MatchStateMachine.SetStateController(MatchState.PostStage, new PostStageController(m_MatchStateMachine));
        m_MatchStateMachine.SetStateController(MatchState.Result, new ResultController(m_MatchStateMachine));
    }

    #region MoveStageUICallback
    public static void OnDiceDrawed()
    {
        if (s_Instance.m_MatchStateMachine.CurrentState == MatchState.MoveStage)
        {
            MoveStageController stateController = s_Instance.m_MatchStateMachine.CurrentStateController as MoveStageController;
            stateController.OnDiceDrawed();
        }
    }
    public static void OnBranchSelected(int branch)
    {
        if (s_Instance.m_MatchStateMachine.CurrentState == MatchState.MoveStage)
        {
            MoveStageController stateController = s_Instance.m_MatchStateMachine.CurrentStateController as MoveStageController;
            stateController.OnBranchSelected(branch);
        }
    }
    #endregion
}
