using System;
using System.Collections;
using System.Collections.Generic;
using Characters.States;
using IA.FSM;
using JetBrains.Annotations;
using MVC;
using UnityEngine;

namespace Characters
{
	public class CharacterModel : BaseModel
	{
		public new IView View { get; }
		protected readonly FSM<string> StateMachine;
		private const string JumpState = "Jump";
		private const string IdleState = "Idle";

		public CharacterFlags Flags;
		public bool IsAiming;

		private IdleRun<string> _idleRun;
		private Jump<string> _jump;

		public CharacterModel(IView view, CharacterProperties properties) : base(view)
		{
			View = view;
			Properties = properties;
			transform = view.Transform;
			rigidbody = view.Rigidbody;
			Flags = new CharacterFlags();
			_jump = new Jump<string>(this);
			_idleRun = new IdleRun<string>(this);
			_jump.AddTransition(IdleState, _idleRun);
			_idleRun.AddTransition(JumpState, _jump);

			StateMachine = FSM<string>
							.Build(_idleRun, nameof(transform.gameObject))
							.WithThisLogger(Debug.unityLogger)
							.ThatLogsTransitions()
							.Done();
		}

		#region Events

		/// <summary>
		/// Event risen when the attack starts.
		/// Transform can be null
		/// </summary>
		public event Action<Transform> onAttacking = delegate { };

		/// <summary>
		/// Event risen when the attack ends.
		/// Transform can be null
		/// </summary>
		public event Action<Transform> onAttacked = delegate { };

		/// <summary>
		/// Event risen when the character moves.
		/// Vector3 is direction;
		/// </summary>
		public event Action<Vector2> onMove = delegate { };

		/// <summary>
		/// Event risen when the character stops.
		/// Vector3 is direction;
		/// </summary>
		public event Action onStop = delegate { };

		/// <summary>
		/// Event risen when the character jumps.
		/// </summary>
		public event Action onJump = delegate { };

		/// <summary>
		/// Event risen when the character lands.
		/// </summary>
		public event Action onLand = delegate { };

		/// <summary>
		/// Event risen when the character is stunned.
		/// </summary>
		public event Action onStunned;

		/// <summary>
		/// Event risen when the character is stunned.
		/// </summary>
		public event Action onRecovered;

		#endregion

		public Transform transform { get; }
		public Rigidbody rigidbody { get; }

		public CharacterProperties Properties { get; }

		public void Update(float deltaTime)
			=> StateMachine.Update(deltaTime);

		public void MoveTowards(Vector2 direction)
		{
			((CharacterState<string>)StateMachine.CurrentState).MoveTowards(direction);
			if (direction.magnitude > 0)
				onMove(direction);
		}

		public void Jump()
			=> StateMachine.TransitionTo(JumpState);

		public void Land()
			=> StateMachine.TransitionTo(IdleState);

		public void OnCollisionEnter(Collision other)
		{
		}

		public void OnCollisionExit(Collision other)
		{
		}

		public IEnumerator Attack(IEnumerator<Transform> behaviour,
								[CanBeNull] Transform target)
		{
			onAttacking(target);
			yield return behaviour;
			onAttacked(target);
		}

		public struct CharacterFlags
		{
			public bool IsAiming;
			public bool IsStunned;
		}
	}
}