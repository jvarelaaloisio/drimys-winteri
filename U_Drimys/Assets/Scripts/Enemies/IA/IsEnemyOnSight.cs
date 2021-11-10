using IA;

namespace Enemies.IA
{
	public class IsEnemyOnSight : EnemyQuestion
	{
		protected override int CheckConditionInternal()
		{
			return LineOfSight.IsTargetOnSight(Model.transform,
												Player.position,
												Model.Properties.viewDistance,
												Model.Properties.FieldOfView,
												Model.Properties.WallLayer.value)
						? 0
						: 1;
		}
	}
}
