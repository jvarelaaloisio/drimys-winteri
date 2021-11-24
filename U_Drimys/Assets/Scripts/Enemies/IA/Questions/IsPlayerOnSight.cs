using Core.Extensions;
using IA;
using UnityEngine;

namespace Enemies.IA.Questions
{
	public class IsPlayerOnSight : EnemyQuestion
	{
		protected override int CheckConditionInternal(EnemyModel model, Transform player)
		{
			return LineOfSight.IsTargetOnSight(model.transform,
												player.position,
												model.Properties.ViewDistance,
												model.Properties.FieldOfView,
												model.Properties.WallLayer.value)
							.TrueIs0FalseIs1();
		}
	}
}