using System;
using System.Collections;
using Core.Helpers;
using Core.Interactions.Throwables;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Characters
{
	public class ThrowerModel : CharacterModel
	{
		public new StateFlags Flags;
		protected readonly Throwable ThrowablePrefab;

		[Tooltip("Transform from where to spawn throwable prefab")]
		protected readonly Transform Hand;

		public ThrowerModel(Transform transform,
							Rigidbody rigidbody,
							ThrowerProperties properties,
							Throwable throwablePrefab,
							Transform hand,
							ICoroutineRunner coroutineRunner,
							bool shouldLogFsmTransitions = false)
			: base(transform,
					rigidbody,
					properties,
					coroutineRunner,
					shouldLogFsmTransitions)
		{
			Flags = new StateFlags();
			Properties = properties;
			ThrowablePrefab = throwablePrefab;
			Hand = hand;
		}

		#region Events

		public event Action onAim = delegate { };
		public event Action onAimCanceled = delegate { };
		public event Action onThrow = delegate { };

		#endregion

		public new ThrowerProperties Properties { get; }

		public void Aim([CanBeNull] Transform target)
		{
			Flags.IsAiming = true;
			CoroutineRunner.StartCoroutine(ThrowAiming(ThrowablePrefab,
														Hand,
														target,
														Properties.AimDelay));
		}

		public void ReleaseAimAndThrow()
		{
			Flags.IsAiming = false;
		}

		public void ThrowAttack([CanBeNull] Transform target,
								Transform hand,
								float spawnDelay,
								float duration,
								Throwable prefab = null)
		{
			Throwable throwable = prefab ?? ThrowablePrefab;
			CoroutineRunner.StartCoroutine(Attack(SimpleThrow(throwable,
															hand,
															target,
															spawnDelay,
															duration),
												throwable.transform));
		}

		protected IEnumerator SimpleThrow(Throwable prefab,
										Transform hand,
										[CanBeNull] Transform target,
										float spawnDelay,
										float duration)
		{
			yield return new WaitForSeconds(spawnDelay);
			var throwable = Object.Instantiate(prefab, hand.position, hand.rotation);
			throwable.Throw(target ? target.position : transform.position + transform.forward * 10, 10);
			yield return new WaitForSeconds(duration - spawnDelay);
		}

		protected IEnumerator ThrowAiming(Throwable prefab,
										Transform hand,
										[CanBeNull] Transform target,
										float delay)
		{
			onAim();
			yield return new WaitForSeconds(delay);
			if (!Flags.IsAiming)
			{
				onAimCanceled();
				yield break;
			}

			yield return new WaitWhile(() => Flags.IsAiming);

			var throwable = Object.Instantiate(prefab, hand.position, hand.rotation);
			throwable.Throw(target ? target.position : transform.position + transform.forward * 10, 10);
			onThrow();
		}

		public new struct StateFlags
		{
			public bool IsMoving;
			public bool IsAttacking;
			public bool IsAiming;
			public bool IsStunned;
			public bool IsLocked;
		}
	}
}