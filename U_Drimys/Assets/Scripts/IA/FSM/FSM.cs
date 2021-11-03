using System;
using UnityEngine;

namespace IA.FSM
{
	/// <summary>
	/// Finite state machine
	/// </summary>
	/// <typeparam name="T">The key Type to access the different states</typeparam>
	public class FSM<T>
	{
		private readonly string _tag;
		private bool _isLoggingTransitions = false;

		private FSM(State<T> initialState,
					string ownerTag = "")
		{
			CurrentState = initialState;
			CurrentState.Awake();
			_tag = ownerTag != "" ? $"<b>{ownerTag} (FSM)</b>" : "<b>FSM</b>";
		}

		private ILogger _logger;

		/// <summary>
		/// Event triggered when the state changes. First state is the last and second is the new one.
		/// </summary>
		public event Action<State<T>, State<T>> OnTransition = delegate { };

		/// <summary>
		/// Current state running in the FSM
		/// </summary>
		public State<T> CurrentState { get; private set; }

		/// <summary>
		/// Change the current state to another one in the dictionary.
		/// </summary>
		/// <param name="key">Key for the next state</param>
		public void TransitionTo(T key)
		{
			if (!CurrentState.TryGetTransition(key, out var transition))
				return;

			if (transition == CurrentState)
				return;
			CurrentState?.Sleep();

			if (_isLoggingTransitions)
				_logger.Log(_tag, $"changed state: {CurrentState.GetName()} -> {transition.GetName()}");

			CurrentState = transition;
			CurrentState.Awake();
			OnTransition(CurrentState, transition);
		}

		/// <summary>
		/// Update method that runs the current state's OnUpdate.
		/// </summary>
		public void Update(float deltaTime)
			=> CurrentState.Update(deltaTime);

		public static Builder Build(State<T> initialState,
									string ownerTag = "")
			=> new Builder(initialState,
							ownerTag);

		public static Builder Build(FSM<T> fsm)
			=> new Builder(fsm);

		public class Builder
		{
			private readonly FSM<T> _fsm;

			internal Builder(State<T> initialState,
							string ownerTag = "")
				=> _fsm = new FSM<T>(initialState,
									ownerTag);

			internal Builder(FSM<T> fsm)
				=> _fsm = fsm;

			public FSM<T> Done()
				=> _fsm;

			public Builder WithThisLogger(ILogger logger)
			{
				_fsm._logger = logger;
				return this;
			}

			public Builder ThatLogsTransitions(bool value = true)
			{
				if (value)
				{
					if (_fsm._logger == null)
					{
						throw new
							ArgumentException("FSM should have a logger set." +
											" Please use the method <b>WithThisLogger(<i>Logger</i>)</b> before calling this one");
					}

					_fsm._logger.Log(_fsm._tag, "Now this FSM logs transitions");
				}

				_fsm._isLoggingTransitions = value;
				return this;
			}

			public Builder ThatTriggersOnTransition(Action<State<T>, State<T>> eventHandler)
			{
				_fsm.OnTransition += eventHandler;
				return this;
			}
		}
	}
}