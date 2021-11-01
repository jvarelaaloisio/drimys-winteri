using System;
using System.Collections;
using Core.Extensions;
using UnityEngine;

namespace Characters.States
{
	public class IdleRun<T> : CharacterState<T>
	{
		protected readonly Action<IEnumerator> StartCoroutine;
		protected readonly Action<string> StopCoroutine;
		protected readonly CharacterProperties CharacterProperties;
		protected Vector3 MovementDirection;
		protected readonly Transform transform;

		public IdleRun(CharacterModel model) : base(model)
		{
			StartCoroutine = c => model.View.StartCoroutine(c);
			StopCoroutine = model.View.StopCoroutine;
			CharacterProperties = Model.Properties;
			transform = model.transform;
		}

		public override string GetName() => "Idle";

		public override void MoveTowards(Vector2 direction)
		{
			MovementDirection = direction.HorizontalPlaneToVector3();
			StopCoroutine(nameof(CharacterHelper.MoveHorizontally));
			StartCoroutine(CharacterHelper.MoveHorizontally(Model.rigidbody,
													MovementDirection,
													CharacterProperties.Speed,
													CharacterProperties.MaxSpeed)
												);
		}

		public override void Update(float deltaTime)
		{
			Debug.DrawRay(transform.position,
						Model.rigidbody.velocity,
						Color.blue);
			if (MovementDirection.magnitude > .1f)
				transform.rotation = Quaternion.Slerp(transform.rotation,
													Quaternion.LookRotation(MovementDirection),
													CharacterProperties.TurnSpeed * deltaTime);
			base.Update(deltaTime);
		}
	}
}