using IA.DecisionTree;
using IA.DecisionTree.Helpers;
using UnityEngine;

namespace Enemies.IA
{
	[SkippedInEditor]
	public abstract class EnemyQuestion : TreeQuestion
	{
		public override int CheckCondition()
		{
			EnemyModel model = (EnemyModel)getData();
			Transform player = GameObject.FindGameObjectWithTag("Player").transform;
			return CheckConditionInternal(model, player);
		}

		/// <summary>
		/// This method should return 0 if the condition is true
		/// </summary>
		/// <param name="model"></param>
		/// <param name="player"></param>
		/// <returns></returns>
		protected abstract int CheckConditionInternal(EnemyModel model, Transform player);
	}
}