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
			var down = -transform.up;
			bool directionMagnitudeIsNotZero = direction.magnitude < .1f;
			bool velocityYIsPositive = Model.rigidbody.velocity.y > -.05f;
			bool isAtLandDistance = Physics.Raycast(transform.position,
										down,
										CharacterProperties.LandDistance,
										CharacterProperties.FloorLayer);
			if (_landing
				|| velocityYIsPositive
				|| !isAtLandDistance)
				return;
			string dirMagnitudeColor = directionMagnitudeIsNotZero ? "green" : "red";
			string velocityColor = velocityYIsPositive ? "green" : "red";
			string landingColor = _landing ? "green" : "red";
			string isLandDistanceColor = isAtLandDistance ? "green" : "red";
			Debug.Log($"<color=blue>forcing land" +
					$"\ndir magnitude < .1f?: <color={dirMagnitudeColor}>{directionMagnitudeIsNotZero}</color>, " +
					$"vel Y (should be >-.05f): <color={velocityColor}>{velocityYIsPositive}</color>, " +
					$"landing? (should be true): <color={landingColor}>{_landing}</color>, " +
					$"isAtLandDistance? (should be false): <color={isLandDistanceColor}>{isAtLandDistance}</color></color>");
			_landing = true;
			CoroutineRunner.StartCoroutine(CharacterHelper.AddForce(Model.rigidbody,
																	down * CharacterProperties.LandingForce,
																	ForceMode.Impulse));
		}
	}
}