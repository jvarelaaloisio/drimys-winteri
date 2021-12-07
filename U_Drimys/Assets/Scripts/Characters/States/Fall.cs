using Core.Helpers;
using UnityEngine;

namespace Characters.States
{
	public class Fall<T> : IdleRun<T>
	{
		private bool _landing;

		public Fall(CharacterModel model,
					ICoroutineRunner coroutineRunner)
			: base(model, coroutineRunner)
		{
			Speed = CharacterProperties.AirSpeed;
		}

		public override string GetName() => "Fall";

		public override void Awake()
		{
			base.Awake();
			_landing = false;
		}

		public override void MoveTowards(Vector2 direction)
		{
			base.MoveTowards(direction);
			bool velocityYIsPositive = Model.rigidbody.velocity.y > -.05f;
			bool isAtLandDistance = Physics.Raycast(transform.position,
													Vector3.down,
													CharacterProperties.LandDistance,
													CharacterProperties.FloorLayer);
			if (_landing
				|| Model.Flags.IsStepping
				|| velocityYIsPositive
				|| !isAtLandDistance)
				return;
			// string dirMagnitudeColor = directionMagnitudeIsNotZero ? "green" : "red";
			// string velocityColor = velocityYIsPositive ? "green" : "red";
			// string landingColor = _landing ? "green" : "red";
			// string isLandDistanceColor = isAtLandDistance ? "green" : "red";
			// Debug.Log($"<color=blue>forcing land" +
			// 		$"\ndir magnitude < .1f?: <color={dirMagnitudeColor}>{directionMagnitudeIsNotZero}</color>, " +
			// 		$"vel Y (should be >-.05f): <color={velocityColor}>{velocityYIsPositive}</color>, " +
			// 		$"landing? (should be true): <color={landingColor}>{_landing}</color>, " +
			// 		$"isAtLandDistance? (should be false): <color={isLandDistanceColor}>{isAtLandDistance}</color></color>");
			ForceLanding();
		}

		private void ForceLanding()
		{
			Debug.Log("Force landing");
			_landing = true;
			CoroutineRunner.StartCoroutine(CharacterHelper.AddForce(Model.rigidbody,
																	Vector3.down * CharacterProperties.LandingForce,
																	ForceMode.Impulse));
		}
	}
}