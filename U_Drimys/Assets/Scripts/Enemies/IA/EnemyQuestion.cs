using IA.DecisionTree;
using IA.DecisionTree.Helpers;
using UnityEngine;

namespace Enemies.IA
{
	[SkippedInEditor]
	public abstract class EnemyQuestion : TreeQuestion
	{
		protected Transform Player;
		protected EnemyModel Model;

		public override int CheckCondition()
		{
			Model = (EnemyModel)getData();
			Player = GameObject.FindGameObjectWithTag("Player").transform;
			return CheckConditionInternal();
		}

		protected abstract int CheckConditionInternal();
	}
}