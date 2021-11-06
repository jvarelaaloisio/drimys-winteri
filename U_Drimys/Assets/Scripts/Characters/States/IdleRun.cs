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
		protected float Speed;
		protected float MaxSpeed;
		private bool canMove;

		/*TODO:CharacterProperties can be avoided
		 Implement when the characterHelper is received in constructor.
		 If then, speed, maxSpeed and turnSpeed could be either received via constructor
		 or could be contained in a series of characterHelper instances that would contain said data.
		 It depends on the impl of LUTs and such.
		*/
		public IdleRun(CharacterModel model,
						ICoroutineRunner coroutineRunner)
			: base(model,
					coroutineRunner)
		{
			canMove = true;
			CharacterProperties = Model.Properties;
			transform = model.transform;
			Speed = CharacterProperties.GroundSpeed;
			MaxSpeed = CharacterProperties.MaxSpeed;
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
																			() => canMove = true,
																			Speed,
																			MaxSpeed)
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