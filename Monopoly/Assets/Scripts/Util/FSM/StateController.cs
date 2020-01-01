using System;
using System.Collections.Generic;
using UnityEngine;

public class StateController<TState> where TState : IComparable 
{
    public event Action OnEntered;
    public event Action OnExited;
    public event Action<float> OnUpdate;

    protected FiniteStateMachine<TState> parentFSM;

    public StateController(FiniteStateMachine<TState> parent)
    {
        parentFSM = parent;
    }

    public virtual void Enter()
    {
        if (OnEntered != null) OnEntered();
    }

    public virtual void Exit()
    {
        if (OnExited != null) OnExited();
    }

    public virtual void Update(float timeStep)
    {
        if (OnUpdate != null) OnUpdate(timeStep);
    }
}