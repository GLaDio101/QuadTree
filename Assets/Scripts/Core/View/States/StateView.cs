using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Core.View.States
{
  public enum StateViewEvent
  {
    StateChanged
  }

  public class StateView : EventView
  {
    public BaseState currentState
    {
      get { return _currentState; }
    }

    public BaseState previousState;

    [HideInInspector] public float elapsedTimeInState = 0f;


    private Dictionary<System.Type, BaseState> _states = new Dictionary<System.Type, BaseState>();
    private Dictionary<string, BaseState> _map = new Dictionary<string, BaseState>();
    private BaseState _currentState;

    protected void Init(BaseState initialState)
    {
      // setup our initial state
      addState(initialState);
      _currentState = initialState;
      _currentState.begin();
    }

    /// <summary>
    /// adds the state to the machine
    /// </summary>
    protected void addState(BaseState state)
    {
      state.setView(this);
      _states[state.GetType()] = state;
      _map.Add(state.Key, state);
    }

    protected void reloadStates()
    {
      foreach (BaseState state in _map.Values)
      {
        state.setView(this);
      }
    }

    /// <summary>
    /// ticks the state machine with the provided delta time
    /// </summary>
    protected void update(float deltaTime)
    {
      elapsedTimeInState += deltaTime;
      _currentState.reason();
      _currentState.update(deltaTime);
    }

    protected BaseState changeState(string key)
    {
      if (!_map.ContainsKey(key))
        return _currentState;
      // avoid changing to the same state
      var newType = _map[key].GetType();
      if (_currentState.GetType() == newType)
        return _currentState;

      // only call end if we have a currentState
      if (_currentState != null)
        _currentState.end();

#if UNITY_EDITOR
      // do a sanity check while in the editor to ensure we have the given state in our state list
      if (!_states.ContainsKey(newType))
      {
        var error = GetType() + ": state " + newType + " does not exist. Did you forget to add it by calling addState?";
        Debug.LogError(error);
        throw new Exception(error);
      }
#endif

      // swap states and call begin
      previousState = _currentState;
      _currentState = _states[newType];
      _currentState.begin();
      elapsedTimeInState = 0f;

      // fire the changed event if we have a listener
      dispatcher.Dispatch(StateViewEvent.StateChanged);

      return _currentState;
    }
  }
}