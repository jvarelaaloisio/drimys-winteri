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
		protected Transform transform;

		public CharacterFlags Flags;
		public bool IsAiming;
		public Func<float, float> ProcessSpeed = f => f;
		public CharacterModel(IView view, CharacterProperties properties) : base(view)
		{
			Properties = properties;
			transform = View.Transform;
			Flags = new CharacterFlags();
			//TODO:Add FSM setup
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

		public CharacterProperties Properties { get; }

		//TODO:Let states handle movement
		public void Move(Vector2 direction)
		{
			Vector3 movementDirection = direction.HorizontalPlaneToVector3();
			float rotationAngle = Vector3.Angle(View.Transform.rotation.eulerAngles, movementDirection);
			transform.Rotate(new Vector3(0, rotationAngle * Properties.TurnSpeed, 0));
			View.Velocity = movementDirection.ReplaceY(View.Velocity.y) * ProcessSpeed(Properties.Speed);
		}

		//TODO:Check if currentState is not Jump
		public void Jump() => View.Jump(Properties.JumpForce);

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

	[CreateAssetMenu(menuName = "Characters/Properties", fileName = "CharacterProperties", order = 0)]
	public class CharacterProperties : ScriptableObject
	{
		[SerializeField]
		private float jumpForce;

		[SerializeField]
		private float speed;

		[SerializeField]
		private float turnSpeed;

		public float JumpForce => jumpForce;

		public float Speed => speed;

		public float TurnSpeed => turnSpeed;
	}
}