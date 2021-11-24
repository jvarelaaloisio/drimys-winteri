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
		public const string JUMP_STATE = "Jump";
		public const string IDLE_STATE = "Idle";
		public const string FALL_STATE = "Fall";

		public StateFlags Flags;

		private LockTarget _lockTarget;
		private IdleRun<string> _idleRun;
		private Fall<string> _fall;
		private Jump<string> _jump;

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

			_idleRun = new IdleRun<string>(this, coroutineRunner);

			_jump = new Jump<string>(this, coroutineRunner);
			_jump.OnAwake += HandleJump;

			_fall = new Fall<string>(this, coroutineRunner);
			_fall.OnAwake += HandleFall;
			_fall.OnSleep += HandleLanding;

			_idleRun.AddTransition(JUMP_STATE, _jump);
			_idleRun.AddTransition(FALL_STATE, _fall);

			_jump.AddTransition(IDLE_STATE, _idleRun);
			_jump.AddTransition(FALL_STATE, _fall);

			_fall.AddTransition(IDLE_STATE, _idleRun);

			StateMachine = FSM<string>
							.Build(_idleRun, transform.gameObject.name)
							.WithThisLogger(Debug.unityLogger)
							.ThatLogsTransitions(shouldLogFsmTransitions)
							.Done();
		}

		private void HandleFall()
			=> onFall();

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
										? IDLE_STATE
										: FALL_STATE);
		}

		public void MoveTowards(Vector2 direction)
		{
			((CharacterState<string>)StateMachine.CurrentState).MoveTowards(direction);
			bool willMove = direction.magnitude > 0;
			if (Flags.IsMoving && !willMove && StateMachine.CurrentState == _idleRun)
			{
				Debug.Log($"stopped: isMoving: {Flags.IsMoving} ; willMove: {willMove}");
				onStop();
			}

			Flags.IsMoving = willMove;
		}

		public void Jump()
			=> StateMachine.TransitionTo(JUMP_STATE);

		public void Land()
			=> StateMachine.TransitionTo(IDLE_STATE);

		public void Melee(IEnumerator behaviour, Transform target)
		{
			CoroutineRunner.StartCoroutine(Attack(behaviour, target));
		}

		protected IEnumerator Attack(IEnumerator behaviour,
									[CanBeNull] Transform target)
		{
			if (Flags.IsAttacking)
				yield break;
			Flags.IsAttacking = true;
			onAttacking(target);
			yield return behaviour;
			onAttacked(target);
			Flags.IsAttacking = false;
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
									.FirstOrDefault(candidate => candidate.CompareTag(targetTag));
			if (!_lockTarget)
			{
				Flags.IsLocked = false;
				Debug.Log($"target with tag {targetTag} not found");
				return;
			}

			LockTargetTransform = _lockTarget.transform;
			_lockTarget.onDisabling.AddListener(HandleTargetDisables);
			onLock(LockTargetTransform);
		}

		private void HandleTargetDisables()
		{
			Flags.IsLocked = false;
			_lockTarget = null;
			LockTargetTransform = null;
			onUnlock();
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
			public bool IsAttacking;
			public bool IsMoving;
			public bool IsStunned;
			public bool IsLocked;
		}
	}
}