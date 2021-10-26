using System;
using System.Collections;
using System.Collections.Generic;
using Core.Extensions;
using JetBrains.Annotations;
using MVC;
using UnityEngine;

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
			transform = View.Transform;
			Speed = speed;
			TurnSpeed = turnSpeed;
			Flags = new CharacterFlags();
		}

		public void Move(Vector2 direction, Func<float, float> processSpeed)
		{
			Vector3 movementDirection = direction.HorizontalPlaneToVector3();
			float rotationAngle = Vector3.Angle(View.Transform.rotation.eulerAngles, movementDirection);
			transform.Rotate(new Vector3(0, rotationAngle * TurnSpeed, 0));
			View.Velocity = movementDirection.ReplaceY(View.Velocity.y) * processSpeed(Speed);
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