using Core.Extensions;
using UnityEngine;

namespace Enemies.IA.Questions
{
	public class IsPlayerAtMeleeDistance : EnemyQuestion
	{
		protected override int CheckConditionInternal(EnemyModel model, Transform player)
		{
			return ((player.position - model.transform.position).magnitude <= model.Properties.AttackDistance)
				.TrueIs0FalseIs1();
		}
	}
}