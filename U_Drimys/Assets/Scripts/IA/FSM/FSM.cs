using System;
using System.Collections.Generic;
using UnityEngine;

namespace IA.FSM
{
	/// <summary>
	/// Finite state machine
	/// </summary>
	/// <typeparam name="T">The key Type to access the different states</typeparam>
	public class FSM<T>
	{
		private readonly Dictionary<T, State> _states = new Dictionary<T, State>();

		private readonly string _tag;
		private bool _isLoggingTransitions = false;

		private FSM(string ownerTag = "")
			=> _tag = ownerTag != "" ? $"<b>{ownerTag} (FSM)</b>" : "<b>FSM</b>";

		private ILogger _logger;
		
		/// <summary>
		/// Event triggered when the state changes. First state is the last and second is the new one.
		/// </summary>
		public event Action<State, State> OnTransition = delegate { };

		/// <summary>
		/// Event triggered when a new state is added to the Dictionary.
		/// </summary>
		public event Action<State> OnAddedState = delegate { };

		/// <summary>
		/// Current state running in the FSM
		/// </summary>
		public State CurrentState { get; private set; }

		/// <summary>
		/// Change the current state to another one in the dictionary.
		/// </summary>
		/// <param name="key">Key for the next state</param>
		public void TransitionTo(T key)
		{
			if (!_states.ContainsKey(key))
			{
				_logger.LogError(_tag, $"key not found -> {key}");
				return;
			}

			if (_states[key] == CurrentState)
				return;
			CurrentState?.Sleep();

			if (_isLoggingTransitions)
				_logger.Log(_tag, $"changed state: {CurrentState} -> {_states[key]}");

			CurrentState = _states[key];
			CurrentState.Awake();
			OnTransition(CurrentState, _states[key]);
		}

		/// <summary>
		/// Add a state to the Dictionary.
		/// </summary>
		/// <param name="key">Key to access the state</param>
		/// <param name="state">The state object</param>
		public void AddState(T key, State state)
		{
			if (_states.ContainsKey(key))
			{
				_logger.LogError("FSM", $"duplicated key -> {key}");
				return;
			}

			_states.Add(key, state);
			OnAddedState(state);
		}

		/// <summary>
		/// Update method that runs the current state's OnUpdate.
		/// </summary>
		public void Update(float deltaTime)
			=> CurrentState.Update(deltaTime);

		public class Builder
		{
			private readonly FSM<T> fsm;

			private Builder(string ownerTag = "")
				=> fsm = new FSM<T>(ownerTag);
			
			private Builder(FSM<T> fsm)
				=> this.fsm = fsm;

			public static Builder BuildAnFSM(string ownerTag = "", FSM<T> fsm = null)
				=> new Builder(ownerTag);

			public FSM<T> Build()
				=> fsm;

			public Builder WithThisLogger(ILogger logger)
			{
				fsm._logger = logger;
				return this;
			}

			public Builder ThatLogsTransitions(bool value = true)
			{
				if (value)
					fsm._logger.Log(fsm._tag, "Now this FSM logs transitions");
				fsm._isLoggingTransitions = value;
				return this;
			}

			public Builder WithThisState(T key, State state)
			{
				fsm.AddState(key, state);
				return this;
			}

			public Builder ThatTriggersOnStateAddition(Action<State> eventHandler)
			{
				fsm.OnAddedState += eventHandler;
				return this;
			}

			public Builder ThatTriggersOnTransition(Action<State, State> eventHandler)
			{
				fsm.OnTransition += eventHandler;
				return this;
			}
		}
	}
}