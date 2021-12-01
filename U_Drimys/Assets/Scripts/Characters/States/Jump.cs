using Core.Helpers;
using UnityEngine;

namespace Characters.States
{
	public class Jump<T> : Fall<T>
	{
		public Jump(CharacterModel model,
					ICoroutineRunner coroutineRunner)
			: base(model,
					coroutineRunner)
		{
		}

		public override string GetName() => "Jump";

		public override void Awake()
		{
			CoroutineRunner.StartCoroutine(CharacterHelper.AddForce(Model.rigidbody,
																	Vector3.up * CharacterProperties.JumpForce,
																	ForceMode.Impulse));
			base.Awake();
		}

		protected override void ManageStairSteps()
		{
			//TODO:This is the same logic as it's father, only skipping an if.
			if (CharacterHelper.IsInFrontOfStepUp(Model.StepValidationPositionLow,
												Model.StepValidationPositionHigh,
												transform.forward,
												CharacterProperties.StepDistanceCheck,
												CharacterProperties.FloorLayer,
												out var stepPosition))
			{
				IsStepping = true;
				Debug.Log("Stepping");
				CoroutineRunner.StartCoroutine(CharacterHelper.GoOverStep(transform,
																		stepPosition
																		+ Vector3.up * CharacterProperties
																			.GroundDistanceCheck,
																		CharacterProperties.StepUpTime,
																		() =>
																		{
																			Debug.Log("Stepped");
																			IsStepping = false;
																		}));
			}
		}
	}
}