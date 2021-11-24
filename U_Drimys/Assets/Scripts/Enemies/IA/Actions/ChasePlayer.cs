using UnityEngine;

namespace Enemies.IA.Actions
{
	public class ChasePlayer : EnemyAction
	{
		protected override void DoActionInternal(EnemyModel model, Transform player)
		{
			Vector3 direction = player.position - model.transform.position;
			Vector2 direction2D = new Vector2(direction.x, direction.z);
			model.MoveTowards(direction2D.normalized);
		}
	}
}
