using System;
using System.Collections;
using Core.Helpers;
using Core.Interactions.Throwables;
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

		public void Aim(Transform target)
		{
			Flags.IsAiming = true;
			CoroutineRunner.StartCoroutine(Throw(target, Properties.AimDelay));
		}
		public void Throw()
		{
			Flags.IsAiming = false;
		}
		protected IEnumerator Throw(Transform target,
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

			var throwable = Object.Instantiate(ThrowablePrefab, Hand.position, Hand.rotation);
			throwable.Throw(target ? target.position : transform.position + transform.forward * 10, 10);
			onThrow();
		}

		public new struct StateFlags
		{
			public bool IsMoving;
			public bool IsAiming;
			public bool IsStunned;
			public bool IsLocked;
		}
	}
}