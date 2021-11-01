using UnityEngine;

namespace Characters.States
{
	public class Jump<T> : IdleRun<T>
	{
		public Jump(CharacterModel model) : base(model) { }

		public override string GetName() => "Jump";

		public override void Awake()
		{
			base.Awake();

			StartCoroutine(CharacterHelper.AddForce(Model.rigidbody,
													Vector3.up * CharacterProperties.JumpForce,
													ForceMode.Impulse));
		}
	}
}