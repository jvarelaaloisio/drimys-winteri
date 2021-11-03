using Core.Helpers;
using UnityEngine;

namespace Characters.States
{
	public class Jump<T> : IdleRun<T>
	{
		public Jump(CharacterModel model,
					ICoroutineRunner coroutineRunner)
			: base(model,
					coroutineRunner) { }

		public override string GetName() => "Jump";

		public override void Awake()
		{
			CoroutineRunner.StartCoroutine(CharacterHelper.AddForce(Model.rigidbody,
													Vector3.up * CharacterProperties.JumpForce,
													ForceMode.Impulse));
			base.Awake();
		}
	}
}