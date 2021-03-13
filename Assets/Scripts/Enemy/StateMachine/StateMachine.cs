﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private State _firstState;

    public State Current => _currentState;

    private Player _target;
    private State _currentState;

    private void OnEnable()
    {
        _target = GetComponent<Enemy>().Target;
        Reset(_firstState);
    }

    private void Update()
    {
        if (_currentState == null)
            return;

        var nextState = _currentState.GetNextState();
        if (nextState != null)
            Transit(nextState);
    }

    private void Reset(State startState)
    {
        _currentState = startState;

        if (_currentState != null)
            _currentState.Enter(_target);
    }

    private void Transit(State nextState)
    {
        if (_currentState != null)
            _currentState.Exit();

        _currentState = nextState;

        if (_currentState != null)
            _currentState.Enter(_target);

    }

}