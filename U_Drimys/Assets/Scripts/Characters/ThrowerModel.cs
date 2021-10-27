using System;
using System.Collections;
using Interactables.Throwables;
using MVC;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Characters
{
	public class ThrowerModel : CharacterModel
	{
		#region Events

		public event Action onThrowing = delegate { }; 
		public event Action onThrowed = delegate { }; 

		#endregion
		
		protected readonly Throwable ThrowablePrefab;
		protected readonly Transform Hand;
		
		public ThrowerModel(IView view,
							float speed,
							float turnSpeed,
							Throwable throwablePrefab,
							Transform hand)
			: base(view, speed, turnSpeed)
		{
			ThrowablePrefab = throwablePrefab;
			Hand = hand;
		}

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