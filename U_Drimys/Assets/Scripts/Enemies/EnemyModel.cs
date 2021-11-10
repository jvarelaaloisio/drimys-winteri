using Characters;
using Core.Helpers;
using UnityEngine;

namespace Enemies
{
	public class EnemyModel : CharacterModel
	{
		public new EnemyProperties Properties;
		public EnemyModel(Transform transform,
						Rigidbody rigidbody,
						EnemyProperties properties,
						ICoroutineRunner coroutineRunner,
						bool shouldLogFsmTransitions = false)
			: base(transform,
					rigidbody,
					properties,
					coroutineRunner,
					shouldLogFsmTransitions)
		{
			Properties = properties;
		}
	}
}