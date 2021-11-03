using System;
using System.Collections;
using Core.Helpers;
using Core.Interactions.Throwables;
using MVC;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Characters
{
	public class ThrowerModel : CharacterModel
	{
		protected readonly Throwable ThrowablePrefab;
		protected readonly Transform Hand;

		public ThrowerModel(Transform transform,
							Rigidbody rigidbody,
							CharacterProperties properties,
							Throwable throwablePrefab,
							Transform hand,
							ICoroutineRunner coroutineRunner)
			: base(transform,
					rigidbody,
					properties,
					coroutineRunner)
		{
			ThrowablePrefab = throwablePrefab;
			Hand = hand;
		}

		#region Events

		public event Action onThrowing = delegate { };
		public event Action onThrowed = delegate { };

		#endregion
		
		protected IEnumerator Throw(Transform target,
									float delay)
		{
			onThrowing();
			yield return new WaitForSeconds(delay);
			while (Flags.IsAiming)
				yield return new WaitForSeconds(.1f);

			var throwable = Object.Instantiate(ThrowablePrefab, Hand.position, Hand.rotation);
			throwable.Throw(target ? target.position : transform.position + transform.forward * 10, 10);
			onThrowed();
		}
	}
}