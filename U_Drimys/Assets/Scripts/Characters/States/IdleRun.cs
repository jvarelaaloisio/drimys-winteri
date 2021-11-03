using Core.Extensions;
using Core.Helpers;
using UnityEngine;

namespace Characters.States
{
	public class IdleRun<T> : CharacterState<T>
	{
		protected readonly CharacterProperties CharacterProperties;
		protected Vector3 MovementDirection;
		protected readonly Transform transform;
		private bool canMove;

		public IdleRun(CharacterModel model,
						ICoroutineRunner coroutineRunner)
			: base(model,
					coroutineRunner)
		{
			canMove = true;
			CharacterProperties = Model.Properties;
			transform = model.transform;
		}

		public override string GetName() => "Idle";

		public override void MoveTowards(Vector2 direction)
		{
			MovementDirection = direction.HorizontalPlaneToVector3();
			if (!canMove)
				return;
			canMove = false;
			CoroutineRunner.StartCoroutine(CharacterHelper.MoveHorizontally(Model,
																			MovementDirection,
																			() => canMove = true)
										);
		}

		public override void Update(float deltaTime)
		{
			Debug.DrawRay(transform.position,
						Model.rigidbody.velocity,
						Color.blue);
			if (Model.Flags.IsLocked)
				transform.LookAt(Model.LockTargetTransform.position.ReplaceY(transform.position.y));
			else if (MovementDirection.magnitude > .1f)
				transform.rotation = Quaternion.Slerp(transform.rotation,
													Quaternion.LookRotation(MovementDirection),
													CharacterProperties.TurnSpeed * deltaTime);
			base.Update(deltaTime);
		}
	}
}