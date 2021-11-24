using Core.Extensions;
using UnityEngine;

namespace Enemies.IA.Questions
{
	public class CanAimAgain : EnemyQuestion
	{
		protected override int CheckConditionInternal(EnemyModel model, Transform player)
		{
			return (Time.time >= model.LastThrow + model.Properties.ThrowPeriod).TrueIs0FalseIs1();
		}
	}
}