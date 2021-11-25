using Core.Extensions;
using UnityEngine;

namespace Enemies.IA.Questions
{
	public class CanThrow : EnemyQuestion
	{
		protected override int CheckConditionInternal(EnemyModel model, Transform player)
		{
			return model.throwerFlags.CanThrow.TrueIs0FalseIs1();
		}
	}
}