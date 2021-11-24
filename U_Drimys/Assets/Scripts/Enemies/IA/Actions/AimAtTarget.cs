using UnityEngine;

namespace Enemies.IA.Actions
{
	public class AimAtTarget : EnemyAction
	{
		protected override void DoActionInternal(EnemyModel model, Transform player)
		{
			model.Aim();
		}
	}
}