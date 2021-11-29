using UnityEngine;

namespace Enemies.IA.Actions
{
	public class LockOnPlayer : EnemyAction
	{
		protected override void DoActionInternal(EnemyModel model, Transform player)
		{
			model.TryLock("Player");
		}
	}
}