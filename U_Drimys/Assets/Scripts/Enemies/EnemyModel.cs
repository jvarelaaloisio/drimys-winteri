using Characters;
using Core.Helpers;
using Core.Interactions.Throwables;
using UnityEngine;

namespace Enemies
{
	public class EnemyModel : ThrowerModel
	{
		public new readonly EnemyProperties Properties;

		public float LastThrow { get; protected set; }
		public EnemyModel(Transform transform,
						Rigidbody rigidbody,
						EnemyProperties properties,
						Throwable throwablePrefab,
						Transform hand,
						ICoroutineRunner coroutineRunner,
						bool shouldLogFsmTransitions = false)
			: base(transform,
					rigidbody,
					properties,
					throwablePrefab,
					hand,
					coroutineRunner,
					shouldLogFsmTransitions)
		{
			Properties = properties;
		}

		public override void ReleaseAimAndThrow()
		{
			base.ReleaseAimAndThrow();
			LastThrow = Time.time;
		}
	}
}