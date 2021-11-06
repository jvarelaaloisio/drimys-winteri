using System;
using System.Collections;
using System.Linq;
using Characters.States;
using Core.Helpers;
using Core.Interactions;
using IA.FSM;
using JetBrains.Annotations;
using MVC;
using UnityEngine;

namespace Characters
{
	public class CharacterModel : BaseModel
	{
		private const string JumpState = "Jump";
		private const string IdleState = "Idle";
		private const string FallState = "Fall";
		public StateFlags Flags;
		private LockTarget _lockTarget;

		public CharacterModel(Transform transform,
							Rigidbody rigidbody,
							CharacterProperties properties,
							ICoroutineRunner coroutineRunner,
							bool shouldLogFsmTransitions = false)
			: base(transform,
					coroutineRunner)
		{
			Properties = properties;
			this.rigidbody = rigidbody;
			Flags = new StateFlags();
			var jump = new Jump<string>(this, coroutineRunner);
			jump.OnAwake += HandleJump;

			var idleRun = new IdleRun<string>(this, coroutineRunner);
			var fall = new Fall<string>(this, coroutineRunner);
			jump.OnAwake += HandleFall;
			fall.OnSleep += HandleLanding;

			jump.AddTransition(IdleState, idleRun);
			jump.AddTransition(FallState, fall);

			idleRun.AddTransition(JumpState, jump);
			idleRun.AddTransition(FallState, fall);

			fall.AddTransition(IdleState, idleRun);

			StateMachine = FSM<string>
							.Build(idleRun, nameof(transform.gameObject))
							.WithThisLogger(Debug.unityLogger)
							.ThatLogsTransitions(shouldLogFsmTransitions)
							.Done();
		}

		private void HandleFall()
		{
			onFall();
		}

		#region Events

		/// <summary>
		/// Event risen when the attack starts.
		/// Transform can be null
		/// </summary>
		[CanBeNull]
		public event Action<Transform> onAttacking = delegate { };

		/// <summary>
		/// Event risen when the attack ends.
		/// Transform can be null
		/// </summary>
		[CanBeNull]
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
		/// Event risen when the character starts falling.
		/// </summary>
		public event Action onFall = delegate { };

		/// <summary>
		/// Event risen when the character lands.
		/// </summary>
		public event Action onLand = delegate { };

		/// <summary>
		/// Event risen when the character locks on a target.
		/// </summary>
		public event Action<Transform> onLock = delegate { };

		/// <summary>
		/// Event risen when the character unlocks it's target.
		/// </summary>
		public event Action onUnlock = delegate { };

		/// <summary>
		/// Event risen when the character is stunned.
		/// </summary>
		public event Action onStunned;

		/// <summary>
		/// Event risen when the character is stunned.
		/// </summary>
		public event Action onRecovered;

		#endregion

		public Transform LockTargetTransform { get; private set; }

		public Rigidbody rigidbody { get; }

		public CharacterProperties Properties { get; }

		public void Update(float deltaTime)
		{
			StateMachine.Update(deltaTime);
			StateMachine.TransitionTo(CharacterHelper
										.IsGrounded(transform.position
													+ Vector3.down * Properties.GroundDistanceCheck,
													Properties)
										? IdleState
										: FallState);
		}

		public void MoveTowards(Vector2 direction)
		{
			((CharacterState<string>)StateMachine.CurrentState).MoveTowards(direction);
			bool willMove = direction.magnitude > 0;
			if (Flags.IsMoving && !willMove)
				onStop();
			Flags.IsMoving = willMove;
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

		public IEnumerator Attack(IEnumerator behaviour,
								[CanBeNull] Transform target)
		{
			onAttacking(target);
			yield return behaviour;
			onAttacked(target);
		}

		public void TryLock(string targetTag)
		{
			Flags.IsLocked = !Flags.IsLocked;
			if (!Flags.IsLocked)
			{
				onUnlock();
				return;
			}

			_lockTarget = GameObject.FindObjectsOfType<LockTarget>()
									.OrderBy(candidate => Vector3.Distance(
																			transform.position,
																			candidate.transform.position))
									.First(candidate => candidate.CompareTag(targetTag));
			LockTargetTransform = _lockTarget.transform;
			_lockTarget.onDisabling.AddListener(HandleTargetDisables);
			onLock(LockTargetTransform);
		}

		private void HandleTargetDisables()
		{
			Flags.IsLocked = false;
			_lockTarget = null;
			LockTargetTransform = null;
		}

		private void HandleJump()
		{
			onJump();
		}

		private void HandleLanding()
		{
			onLand();
		}

		public struct StateFlags
		{
			public bool IsMoving;
			public bool IsStunned;
			public bool IsLocked;
		}
	}
}