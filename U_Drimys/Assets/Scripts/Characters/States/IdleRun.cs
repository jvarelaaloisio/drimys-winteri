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
		private bool _canMove;
		protected bool IsStepping;

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
			_canMove = true;
			CharacterProperties = Model.Properties;
			transform = model.transform;
			Speed = CharacterProperties.GroundSpeed;
			MaxSpeed = CharacterProperties.MaxSpeed;
		}

		public override string GetName() => "Idle";

		public override void MoveTowards(Vector2 direction)
		{
			if (IsStepping)
				return;

			MovementDirection = direction.HorizontalPlaneToVector3();
			if (_canMove)
			{
				_canMove = false;
				CoroutineRunner.StartCoroutine(CharacterHelper.MoveHorizontally(Model,
																					MovementDirection,
																					() => _canMove = true,
																					Speed,
																					MaxSpeed)
											);
			}

			ManageStairSteps();
		}

		protected virtual void ManageStairSteps()
		{
			if (CharacterHelper.IsInFrontOfStep(Model.StepValidationPositionLow,
												Model.StepValidationPositionHigh,
												transform.forward,
												CharacterProperties.StepDistanceCheck,
												CharacterProperties.FloorLayer,
												out var stepPosition))
			{
				IsStepping = true;
				// Model.Step();
				CoroutineRunner.StartCoroutine(CharacterHelper.GoOverStep(transform,
																		stepPosition
																		+ Vector3.up * CharacterProperties
																			.GroundDistanceCheck,
																		CharacterProperties.StepUpTime,
																		() => IsStepping = false));
			}
			// else if (CharacterHelper.IsInFrontOfStepDown(Model.StepValidationPositionLow,
			// 											Model.StepValidationPositionHigh,
			// 											transform.forward,
			// 											CharacterProperties.StepDistanceCheck,
			// 											CharacterProperties.FloorLayer,
			// 											out stepPosition))
			// {
			// 	IsStepping = true;
			// 	Debug.Log("Stepping2");
			// 	CoroutineRunner.StartCoroutine(CharacterHelper.GoOverStep(transform,
			// 															stepPosition
			// 															+ Vector3.up * CharacterProperties
			// 																.GroundDistanceCheck,
			// 															CharacterProperties.StepDownTime,
			// 															() =>
			// 															{
			// 																Debug.Log("Stepped2");
			// 																IsStepping = false;
			// 															}));
			// }
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