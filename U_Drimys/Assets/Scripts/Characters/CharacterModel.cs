using System;
using System.Collections;
using System.Collections.Generic;
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
		private const string LockJumpState = "LockedJump";
		public CharacterFlags Flags;
		private LockTarget lockTarget;

		private IdleRun<string> _idleRun;
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
			Flags = new CharacterFlags();
			_jump = new Jump<string>(this, coroutineRunner);
			_jump.OnAwake += RaiseOnJump;
			_jump.OnSleep += RaiseOnLand;
			
			_idleRun = new IdleRun<string>(this, coroutineRunner);

			_jump.AddTransition(IdleState, _idleRun);
			_idleRun.AddTransition(JumpState, _jump);

			StateMachine = FSM<string>
							.Build(_idleRun, nameof(transform.gameObject))
							.WithThisLogger(Debug.unityLogger)
							.ThatLogsTransitions(shouldLogFsmTransitions)
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

		public void TryLock(string targetTag)
		{
			Flags.IsLocked = !Flags.IsLocked;
			if (!Flags.IsLocked)
			{
				onUnlock();
				return;
			}
			
			lockTarget = GameObject.FindObjectsOfType<LockTarget>()
									.OrderBy(candidate => Vector3.Distance(
																			transform.position,
																			candidate.transform.position))
									.First(candidate => candidate.CompareTag(targetTag));
			LockTargetTransform = lockTarget.transform;
			lockTarget.onDisabling.AddListener(HandleTargetDisables);
			onLock(LockTargetTransform);
		}

		private void HandleTargetDisables()
		{
			Flags.IsLocked = false;
			lockTarget = null;
			LockTargetTransform = null;
		}

		private void RaiseOnJump()
		{
			onJump();
		}

		private void RaiseOnLand()
		{
			onLand();
		}

		public struct CharacterFlags
		{
			public bool IsAiming;
			public bool IsStunned;
			public bool IsLocked;
		}
	}
}