using System;

namespace IA.FSM
{
	/// <summary>
	/// State used by the Finite State Machine (FSM) Class
	/// </summary>
	public abstract class State
	{
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
	}
}