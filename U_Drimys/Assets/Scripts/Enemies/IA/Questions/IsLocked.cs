using Core.Extensions;
using UnityEngine;

namespace Enemies.IA.Questions
{
	public class IsLocked : EnemyQuestion
	{
		protected override int CheckConditionInternal(EnemyModel model, Transform player)
		{
			return model.Flags.IsLocked.TrueIs0FalseIs1();
		}
	}
}