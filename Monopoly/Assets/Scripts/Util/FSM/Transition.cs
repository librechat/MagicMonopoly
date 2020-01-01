using System;
using System.Collections.Generic;

public abstract class Transition<TState> where TState : IComparable
{
    public TState FromState { get; private set; }
    public TState ToState { get; private set; }

    private readonly Func<bool> transitionConditionFunc;

    public event Action OnComplete;

    protected Transition(TState from, TState to, Func<bool> transitionConditionFunction = null)
    {
        FromState = from;
        ToState = to;
        transitionConditionFunc = transitionConditionFunction;
    }

    protected void Complete()
    {
        if (OnComplete != null) OnComplete();
    }

    public abstract void Begin();

    public bool TransitionCondition()
    {
        return transitionConditionFunc == null || transitionConditionFunc();
    }
}

public class DefaultStateTransition<TState> : Transition<TState> where TState : IComparable
{
    public DefaultStateTransition(TState from, TState to, Func<bool> transitionConditionFunction = null)
        : base(from, to, transitionConditionFunction)
    {
    }

    public override void Begin()
    {
        Complete();
    }
}