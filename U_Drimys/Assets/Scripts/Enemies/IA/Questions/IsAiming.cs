using Core.Extensions;
using UnityEngine;

namespace Enemies.IA.Questions
{
	public class IsAiming : EnemyQuestion
	{
		protected override int CheckConditionInternal(EnemyModel model, Transform player)
		{
			return model.Flags.IsAiming.TrueIs0FalseIs1();
		}
	}
}