using Core.Extensions;
using Core.Helpers;
using UnityEngine;

namespace Characters.States
{
	public class Step<T> : CharacterState<T>
	{
		private Vector3 _movementDirection;
		private readonly Transform _transform;
		private readonly CharacterProperties _properties;

		private Vector3 _stepPosition;

		public Step(CharacterModel model,
					ICoroutineRunner coroutineRunner)
			: base(model, coroutineRunner)
		{
			_transform = model.transform;
			_properties = Model.Properties;

			OnAwake += () => model.rigidbody.isKinematic = true;
			OnAwake += ValidateIsInFrontOfStep;
			OnAwake += StepOver;
			OnSleep += () => model.rigidbody.isKinematic = false;
		}

		public override string GetName() => "Step";

		public override void MoveTowards(Vector2 direction)
		{
			_movementDirection = direction.HorizontalPlaneToVector3();
		}

		public override void Update(float deltaTime)
		{
			if (_movementDirection.magnitude > .1f)
				Model.transform.rotation = Quaternion.Slerp(Model.transform.rotation,
															Quaternion.LookRotation(_movementDirection),
															Model.Properties.TurnSpeed * deltaTime);
			base.Update(deltaTime);
		}

		private void StepOver()
		{
			CoroutineRunner.StartCoroutine(CharacterHelper.GoOverStep(_transform,
																	_stepPosition
																	+ Vector3.up * _properties
																		.GroundDistanceCheck,
																	_properties.StepUpTime,
																	DecideIfShouldKeepStepping));
		}

		private void ValidateIsInFrontOfStep()
		{
			if (!IsInFrontOfStep())
				Model.Land();
		}

		private void DecideIfShouldKeepStepping()
		{
			if (_movementDirection.magnitude > .1f
				&& IsInFrontOfStep())
				Awake();
			else
				Model.Land();
		}

		private bool IsInFrontOfStep()
		{
			return CharacterHelper.IsInFrontOfStep(Model.StepValidationPositionLow,
													Model.StepValidationPositionHigh,
													_transform.forward,
													_properties.StepDistanceCheck,
													_properties.FloorLayer,
													out _stepPosition);
		}
	}
}