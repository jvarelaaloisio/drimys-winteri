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
		public ThrowerFlags throwerFlags;
		protected readonly Throwable ThrowablePrefab;

		[Tooltip("Transform from where to spawn throwable prefab")]
		protected readonly Transform Hand;

		private Coroutine _throwAimingCoroutine;

		public ThrowerModel(Transform transform,
							Rigidbody rigidbody,
							ThrowerProperties properties,
							Throwable throwablePrefab,
							Transform hand,
							ICoroutineRunner coroutineRunner,
							Transform stepPositionLow,
							Transform stepPositionHigh,
							bool shouldLogFsmTransitions = false)
			: base(transform,
					rigidbody,
					properties,
					coroutineRunner,
					stepPositionLow,
					stepPositionHigh,
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

		public void Aim(bool throwToTargetIfLocked = true)
		{
			throwerFlags.IsAiming = true;
			_throwAimingCoroutine = CoroutineRunner.StartCoroutine(ThrowAiming(ThrowablePrefab,
																				Hand,
																				Properties.AimDelay,
																				throwToTargetIfLocked));
		}

		public virtual void ReleaseAimAndThrow()
		{
			if (throwerFlags.IsAiming && !throwerFlags.CanThrow)
			{
				CoroutineRunner.StopCoroutine(_throwAimingCoroutine);
				onAimCanceled();
			}

			throwerFlags.IsAiming = false;
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

		protected IEnumerator ThrowAiming(Throwable prefab,
										Transform hand,
										float delay,
										bool throwToTargetIfLocked = true)
		{
			onAim();
			yield return new WaitForSeconds(delay);
			throwerFlags.CanThrow = true;
			if (!throwerFlags.IsAiming)
				yield break;

			yield return new WaitWhile(() => throwerFlags.IsAiming);

			var throwable = Object.Instantiate(prefab, hand.position, hand.rotation);
			//TODO:ADD throw distance if there is no target
			if (Flags.IsLocked && throwToTargetIfLocked)
				throwable.FlyTargeted(LockTargetTransform, Properties.ThrowableSpeed);
			else
				throwable.FlyTo(transform.position + transform.forward * 10, Properties.ThrowableSpeed);
			throwerFlags.CanThrow = false;
			onThrow();
		}

		protected IEnumerator SimpleThrow(Throwable prefab,
										Transform hand,
										[CanBeNull] Transform target,
										float spawnDelay,
										float duration)
		{
			yield return new WaitForSeconds(spawnDelay);
			var throwable = Object.Instantiate(prefab, hand.position, hand.rotation);
			throwable.FlyTo(target ? target.position : transform.position + transform.forward * 10,
							Properties.ThrowableSpeed);
			yield return new WaitForSeconds(duration - spawnDelay);
		}

		public struct ThrowerFlags
		{
			public bool IsAiming;
			public bool CanThrow;
		}
	}
}