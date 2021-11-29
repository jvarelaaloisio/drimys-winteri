using Camera.States;
using Core.Helpers;
using IA.FSM;
using MVC;
using UnityEngine;

namespace Camera
{
	public class CameraModel : BaseModel
	{
		private const string AutomaticState = "Automatic";
		private const string SemiautomaticState = "SemiAutomatic";
		private const string FullInputState = "FullInput";
		private const string LockState = "Lock";
		public CameraProperties Properties { get; }
		public Transform Target { get; set; }

		public CameraModel(Transform transform,
							CameraProperties properties,
							Transform target,
							ICoroutineRunner coroutineRunner,
							bool shouldLogFsmTransitions = false)
			: base(transform,
					coroutineRunner)
		{
			Properties = properties;
			Target = target;
			var followAutomatic = new FollowAutomatic<string>(this);
			var followSemiAutomatic = new FollowWithPitchInput<string>(this);
			var followWithInput = new FollowFullInput<string>(this);
			var locked = new Lock<string>(this, coroutineRunner);
			
			followAutomatic.AddTransition(SemiautomaticState, followSemiAutomatic);
			followAutomatic.AddTransition(FullInputState, followWithInput);
			followAutomatic.AddTransition(LockState, locked);
			
			followSemiAutomatic.AddTransition(AutomaticState, followAutomatic);
			followSemiAutomatic.AddTransition(FullInputState, followWithInput);
			followSemiAutomatic.AddTransition(LockState, locked);
			followSemiAutomatic.onYield += YieldToAutomatic;
			
			followWithInput.AddTransition(AutomaticState, followAutomatic);
			followWithInput.AddTransition(SemiautomaticState, followSemiAutomatic);
			followWithInput.AddTransition(LockState, locked);
			followWithInput.onYield += YieldToAutomatic;
			
			locked.AddTransition(AutomaticState, followAutomatic);
			
			StateMachine = FSM<string>.Build(followAutomatic, transform.name)
									.WithThisLogger(Debug.unityLogger)
									.ThatLogsTransitions(shouldLogFsmTransitions)
									.Done();
		}

		private void YieldToAutomatic()
		{
			StateMachine.TransitionTo(AutomaticState);
		}

		public void LateUpdate(float deltaTime)
		{
			StateMachine.Update(deltaTime);
		}
		
		public void HandleMoveInput(Vector2 input)
			=> ((CameraState<string>)StateMachine.CurrentState).LastMoveInput = input;

		public void HandleCamInput(Vector2 input)
		{
			((CameraState<string>)StateMachine.CurrentState).LastCamInput = input;
			if(input.magnitude != 0)
				StateMachine.TransitionTo(FullInputState);
			else if(input.y != 0)
				StateMachine.TransitionTo(SemiautomaticState);
		}

		public void Lock()
			=> StateMachine.TransitionTo(LockState);

		public void Unlock()
			=> StateMachine.TransitionTo(AutomaticState);
	}
}
