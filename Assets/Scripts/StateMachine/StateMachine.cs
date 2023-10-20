using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private Dictionary<Type, IState> _states = new();

    public IState CurrentState { get; private set; }

    public StateMachine(params IState[] states)
    {
        foreach (var state in states)
        {
            state.Initialize(this);
            _states[state.GetType()] = state;
        }
    }

    public void Enter<TState>() where TState : IState
    {
        IExitableStateWithContext context = null;

        switch (CurrentState)
        {
            case IExitableStateWithContext exitableStateWithContext:
                context = exitableStateWithContext.OnExit();
                break;
            case IExitableState exitableState:
                exitableState.OnExit();
                break;
        }

        CurrentState = _states[typeof(TState)];

        Debug.Log("Enter in " + CurrentState);

        switch (CurrentState)
        {
            case IEnterebleStateWithContext enterableStateWithContext:
                enterableStateWithContext.OnEnter(context);
                break;
            case IEnterableState enterableState:
                enterableState.OnEnter();
                break;
        }
    }
}