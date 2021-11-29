using UnityEngine;

namespace Enemies.IA.Actions
{
	public class Unlock : EnemyAction
	{
		protected override void DoActionInternal(EnemyModel model, Transform player)
		{
			model.Unlock();
		}
	}
}