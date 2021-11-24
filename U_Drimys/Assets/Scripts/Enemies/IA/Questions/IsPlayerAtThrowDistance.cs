using UnityEngine;

namespace Enemies.IA.Questions
{
	public class IsPlayerAtThrowDistance : EnemyQuestion
	{
		public override int GetOutcomesQty() => 3;

		protected override int CheckConditionInternal(EnemyModel model, Transform player)
		{
			float distanceFromPlayer = (player.position - model.transform.position).magnitude;
			return distanceFromPlayer < model.Properties.MinThrowDistance
						? 0
						: distanceFromPlayer > model.Properties.MaxThrowDistance
						? 2
						: 1;
		}
	}
}