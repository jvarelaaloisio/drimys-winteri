using System;
using System.Collections;
using System.Collections.Generic;
using Core.Extensions;
using JetBrains.Annotations;
using MVC;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Characters
{
	public class CharacterModel : BaseModel
	{
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
		public event Action<Vector3> onMove = delegate { };

		/// <summary>
		/// Event risen when the character stops.
		/// Vector3 is direction;
		/// </summary>
		public event Action<Vector3> onStop = delegate { };

		/// <summary>
		/// Event risen when the character is stunned.
		/// </summary>
		public event Action onStunned;

		/// <summary>
		/// Event risen when the character is stunned.
		/// </summary>
		public event Action onRecovered;

		#endregion

		protected Transform transform;
		
		public CharacterFlags Flags;
		public bool IsAiming;
		public float Speed { get; set; }
		public float TurnSpeed { get; set; }

		public CharacterModel(IView view, float speed, float turnSpeed) : base(view)
		{
			this.transform = View.Transform;
			Speed = speed;
			TurnSpeed = turnSpeed;
			Flags = new CharacterFlags();
		}

		public void Move(Vector2 direction, Func<float, float> processSpeed)
		{
			Vector3 movementDirection = direction.HorizontalPlaneToVector3();
			float rotationAngle = Vector3.Angle(View.Transform.rotation.eulerAngles, movementDirection);
			View.Transform.Rotate(new Vector3(0, rotationAngle * TurnSpeed, 0));
			View.SetVelocity(movementDirection.ReplaceY(View.GetVelocity().y) * processSpeed(Speed));
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

			public CharacterFlags(bool isAiming, bool isStunned)
			{
				IsAiming = isAiming;
				IsStunned = isStunned;
			}
		}
	}

	public class ThrowerModel : CharacterModel
	{
		#region Events

		public event Action onThrowing = delegate { }; 
		public event Action onThrowed = delegate { }; 

		#endregion
		
		//Cambiar por Throwable
		private readonly GameObject _throwablePrefab;
		private readonly Transform _hand;
		
		public ThrowerModel(IView view,
							float speed,
							float turnSpeed,
							GameObject throwablePrefab,
							Transform hand)
			: base(view, speed, turnSpeed)
		{
			_throwablePrefab = throwablePrefab;
			_hand = hand;
		}
		
		private IEnumerator Throw(Transform target,
								float delay)
		{
			onThrowing();
			yield return new WaitForSeconds(delay);
			while (Flags.IsAiming)
				yield return new WaitForSeconds(.1f);

			//Cambiar por Throwable
			var throwable = Object.Instantiate(_throwablePrefab, _hand.position, _hand.rotation);
			//throwable.Throw(target ? target.position : transform.position + transform.forward * 10, 10);
			onThrowed();
		}
	}
}