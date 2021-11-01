using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace IA.FSM
{
	/// <summary>
	/// State used by the Finite State Machine (FSM) Class
	/// </summary>
	public abstract class State<T>
	{
		private readonly Dictionary<T, State<T>> _transitions = new Dictionary<T, State<T>>();
		public static implicit operator bool(State<T> state) => state != null;

		/// <summary>
		/// Called once when the FSM enters the State
		/// </summary>
		public event Action OnAwake = delegate { };

		/// <summary>
		/// Called once everytime the FSM's Update method is called
		/// </summary>
		public event Action<float> OnUpdate = delegate { };

		/// <summary>
		/// Called once when the FSM exits the State
		/// </summary>
		public event Action OnSleep = delegate { };

		public abstract string GetName();

		/// <summary>
		/// Method called once when entering this state and after exiting the last one.
		/// Always call base method so the corresponding event is raised
		/// </summary>
		public virtual void Awake()
			=> OnAwake();

		/// <summary>
		/// Update method for the State.
		/// Always call base method so the corresponding event is raised
		/// </summary>
		public virtual void Update(float deltaTime)
			=> OnUpdate(deltaTime);

		/// <summary>
		/// Method called once when exiting this state and before entering another.
		/// Always call base method so the corresponding event is raised
		/// </summary>
		public virtual void Sleep()
			=> OnSleep();

		public void AddTransition(T key, State<T> transition)
		{
			if (!_transitions.ContainsKey(key))
				_transitions.Add(key, transition);
		}

		[CanBeNull]
		public State<T> GetTransition(T key) =>
			_transitions.TryGetValue(key, out var transition)
				? transition
				: null;

		public bool TryGetTransition(T key, out State<T> transition)
			=> _transitions.TryGetValue(key, out transition);
	}
}