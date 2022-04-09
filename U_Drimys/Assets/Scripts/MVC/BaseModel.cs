using Core.Helpers;
using IA.FSM;
using UnityEngine;

namespace MVC
{
	public class BaseModel
	{
		/// <summary>
		/// Build in the constructor
		/// </summary>
		protected FiniteStateMachine<string> StateMachine;
		protected readonly ICoroutineRunner CoroutineRunner;

		public BaseModel(Transform transform,
						ICoroutineRunner coroutineRunner)
		{
			this.transform = transform;
			CoroutineRunner = coroutineRunner;
		}
		public Transform transform { get; }

		public void ForceTransition(string key)
			=> StateMachine.TransitionTo(key);

		public State<string> GetCurrentState => StateMachine.CurrentState;
	}
}