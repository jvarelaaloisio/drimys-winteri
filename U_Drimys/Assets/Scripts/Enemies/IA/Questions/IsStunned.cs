using Core.Extensions;
using UnityEngine;

namespace Enemies.IA.Questions
{
	public class IsStunned : EnemyQuestion
	{
		protected override int CheckConditionInternal(EnemyModel model, Transform player)
		{
			return model.Flags.IsStunned.TrueIs0FalseIs1();
		}
	}
}