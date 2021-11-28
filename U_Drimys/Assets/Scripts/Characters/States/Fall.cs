using Core.Helpers;
using UnityEngine;

namespace Characters.States
{
	public class Fall<T> : IdleRun<T>
	{
		private float _start;
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
			_start = Time.time;
		}

		public override void MoveTowards(Vector2 direction)
		{
			base.MoveTowards(direction);
			var down = -transform.up;
			if (_landing
				|| direction.magnitude < .1f
				|| Model.rigidbody.velocity.y > .15f
				|| !Physics.Raycast(transform.position,
									down,
									CharacterProperties.LandDistance,
									CharacterProperties.FloorLayer))
				return;
			_landing = true;
			Debug.Log($"land");
			CoroutineRunner.StartCoroutine(CharacterHelper.AddForce(Model.rigidbody,
																	down * CharacterProperties.LandingForce,
																	ForceMode.Impulse));
		}
	}
}