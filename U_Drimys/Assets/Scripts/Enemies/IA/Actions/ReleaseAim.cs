using UnityEngine;

namespace Enemies.IA.Actions
{
	public class ReleaseAim : EnemyAction
	{
		protected override void DoActionInternal(EnemyModel model, Transform player)
		{
			model.ReleaseAimAndThrow();
		}
	}
}