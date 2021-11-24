using UnityEngine;

namespace Enemies.IA.Actions
{
	public class RetreatFromPlayer : EnemyAction
	{
		protected override void DoActionInternal(EnemyModel model, Transform player)
		{
			//TODO: Eto no anda
			Vector3 direction = (model.transform.position - player.position).normalized;
			Vector2 direction2D = new Vector2(direction.x, direction.z);
			model.MoveTowards(direction2D);
		}
	}
}