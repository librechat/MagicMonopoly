using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_WSA
using System.Reflection;
#endif

/* Modified from https://github.com/dubit/unity-fsm */

public class FiniteStateMachine<TState> where TState : IComparable {

    public TState CurrentState { get; private set; }
    public StateController<TState> CurrentStateController
    {
        get { return this.states[CurrentState]; }
    }
    public Dictionary<TState, StateController<TState>> States
    {
        get { return states; }
    }
    public bool IsTransitioning { get { return currentTransition != null; } }

    public string Name { get; set; }

    private Transition<TState> currentTransition;
    
    private Dictionary<TState, StateController<TState>> states;
    private readonly Dictionary<TState, Dictionary<string, Transition<TState>>> transitions;
    private bool isInitialisingState;
    private string stateMachineName;

    private event Action<TState> OnStateEnter;
    private event Action<TState> OnStateExit;
    private event Action<TState, TState> OnStateChange;
    private event Action<float> OnStateUpdate;

    // Init the FSM
    public static FiniteStateMachine<TState> FromEnum()
    {
        if (!typeof(Enum).IsAssignableFrom(typeof(TState)))
        {
            throw new Exception("Cannot create finite");
        }

        var states = new List<TState>();
        foreach (TState value in Enum.GetValues(typeof(TState)))
        {
            states.Add(value);
        }

        return new FiniteStateMachine<TState>(states.ToArray());
    }
    public FiniteStateMachine(params TState[] states)
    {
        if (states.Length < 1) { throw new ArgumentException("A FiniteStateMachine needs at least 1 state", "states"); }

        transitions = new Dictionary<TState, Dictionary<string, Transition<TState>>>();
        this.states = new Dictionary<TState, StateController<TState>>();
        foreach (var value in states)
        {
            this.states.Add(value, new StateController<TState>(this));
            transitions.Add(value, new Dictionary<string, Transition<TState>>());
        }
    }

    // Set up Transition Callbacks
    public FiniteStateMachine<TState> AddTransition(TState from, TState to, string command = null)
    {
        string cmd = checkTransition(from, to, command);

        // add the transition to the db (new it if it does not exist)
        transitions[from][cmd] = new DefaultStateTransition<TState>(from, to);

        return this;
    }
    public FiniteStateMachine<TState> AddTransition(TState from, TState to, Transition<TState> transition, string command = null)
    {
        string cmd = checkTransition(from, to, command);

        // add the transition to the db (new it if it does not exist)
        transitions[from][cmd] = transition;

        return this;
    }
    public FiniteStateMachine<TState> AddTransition(TState from, TState to, Func<bool> condition, string command = null)
    {
        string cmd = checkTransition(from, to, command);

        // add a default transition to the db
        transitions[from][cmd] = new DefaultStateTransition<TState>(from, to, condition);

        return this;
    }
    private string checkTransition(TState from, TState to, string command)
    {
        if (from == null) { throw new ArgumentNullException("state"); }
        if (to == null) { throw new ArgumentNullException("to"); }
        if (!states.ContainsKey(from)) { throw new ArgumentException("unknown state", "from"); }
        if (!states.ContainsKey(to)) { throw new ArgumentException("unknown state", "to"); }

        //if (string.IsNullOrEmpty(command)) { throw new ArgumentException("command cannot be null or empty", "command"); }
        string cmd = command;
        if (string.IsNullOrEmpty(command))
        {
            cmd = from.ToString() + "-" + to.ToString();
        }
        return cmd;
    }

    public FiniteStateMachine<TState> OnEnter(TState state, Action handler)
    {
        if (state == null) { throw new ArgumentNullException("state"); }
        if (handler == null) { throw new ArgumentNullException("handler"); }
        if (!states.ContainsKey(state)) { throw new ArgumentException("unknown state", "state"); }

        OnStateEnter += enteredState =>
        {
            if (enteredState.Equals(state))
            {
                handler();
            }
        };

        return this;
    }
    public FiniteStateMachine<TState> OnExit(TState state, Action handler)
    {
        if (state == null) { throw new ArgumentNullException("state"); }
        if (handler == null) { throw new ArgumentNullException("handler"); }
        if (!states.ContainsKey(state)) { throw new ArgumentException("unknown state", "state"); }

        OnStateExit += exitedState =>
        {
            if (exitedState.Equals(state))
            {
                handler();
            }
        };
        return this;
    }

    public FiniteStateMachine<TState> OnChange(Action<TState, TState> handler)
    {
        if (handler == null) { throw new ArgumentNullException("handler"); }

        OnStateChange += handler;

        return this;
    }
    public FiniteStateMachine<TState> OnChange(TState from, TState to, Action handler)
    {
        if (from == null) { throw new ArgumentNullException("from"); }
        if (to == null) { throw new ArgumentNullException("to"); }
        if (!states.ContainsKey(from)) { throw new ArgumentException("unknown state", "from"); }
        if (!states.ContainsKey(to)) { throw new ArgumentException("unknown state", "to"); }
        if (handler == null) { throw new ArgumentNullException("handler"); }

        OnStateChange += (fromState, toState) =>
        {
            if (fromState.Equals(from) &&
                toState.Equals(to))
            {
                handler();
            }
        };

        return this;
    }

    public FiniteStateMachine<TState> OnUpdate(TState state, Action<float> handler)
    {
        if (state == null) { throw new ArgumentNullException("state"); }
        if (handler == null) { throw new ArgumentNullException("handler"); }
        if (!states.ContainsKey(state)) { throw new ArgumentException("unknown state", "state"); }

        this.states[state].OnUpdate += handler;
        
        return this;
    }

    public FiniteStateMachine<TState> SetStateController(TState state, StateController<TState> controller)
    {
        this.states[state] = controller;
        return this;
    }

    // Start to run the FSM
    public void Begin(TState firstState)
    {
        if (firstState == null) { throw new ArgumentNullException("firstState"); }
        if (!states.ContainsKey(firstState)) { throw new ArgumentException("unknown state", "firstState"); }

        CurrentState = firstState;
    }
    public void Update(float timeStep)
    {
        if (!IsTransitioning)
        {
            states[CurrentState].Update(timeStep);
        }
    }

    // Raise a command of transition
    public void IssueCommand(string command)
    {
        //Commands set during transitioning will be ignored
        if (IsTransitioning)
            return;

        var transitionsForCurrentState = transitions[CurrentState];
        if (transitionsForCurrentState.ContainsKey(command))
        {
            //Commands should not be issued from code called by
            //OnStateChange and OnStateEnter and will be ignored
            if (isInitialisingState)
            {
                Debug.LogWarning("Do not call IssueCommand from OnStateChange and OnStateEnter handlers");
                return;
            }

            var transition = transitionsForCurrentState[command];

            if (transition.TransitionCondition())
            {
                transition.OnComplete += HandleTransitionComplete;
                currentTransition = transition;

                states[CurrentState].Exit();
                if (OnStateExit != null)
                {
                    OnStateExit(CurrentState);
                }
                transition.Begin();
            }
        }
    } 
    private void HandleTransitionComplete()
    {
        currentTransition.OnComplete -= HandleTransitionComplete;

        var previousState = CurrentState;
        CurrentState = currentTransition.ToState;

        currentTransition = null;

        isInitialisingState = true;

        if (OnStateChange != null)
        {
            OnStateChange(previousState, CurrentState);
        }

        states[CurrentState].Enter();
        if (OnStateEnter != null)
        {
            OnStateEnter(CurrentState);
        }

        isInitialisingState = false;
    }
}
