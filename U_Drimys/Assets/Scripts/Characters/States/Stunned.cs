using System.Collections;
using Core.Helpers;
using UnityEngine;

namespace Characters.States
{
	public class Stunned<T> : CharacterState<T>
	{
		public override string GetName() => "Stunned";
		public float Duration { get; set; } = 0;
		
		public Stunned(CharacterModel model, ICoroutineRunner coroutineRunner) : base(model, coroutineRunner) { }
		
		public override void Awake()
		{
			CoroutineRunner.StartCoroutine(WaitStun(Duration));
			base.Awake();
		}
		private IEnumerator WaitStun(float duration)
		{
			yield return new WaitForSeconds(duration);
			Model.RecoverFromStun();
		}

		public override void MoveTowards(Vector2 direction) { }
	}
}