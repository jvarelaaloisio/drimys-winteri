using Core.Helpers;

namespace Characters.States
{
	public class Fall<T> : IdleRun<T>
	{
		public Fall(CharacterModel model,
					ICoroutineRunner coroutineRunner)
			: base(model, coroutineRunner)
		{
			Speed = CharacterProperties.AirSpeed;
		}

		public override string GetName() => "Fall";
	}
}