using IA.DecisionTree;
using IA.DecisionTree.Helpers;
using UnityEngine;

namespace Enemies.IA
{
	[SkippedInEditor]
	public abstract class EnemyAction : TreeAction
	{
		/// <summary>
		/// Action behaviour
		/// </summary>
		/// <param name="model"></param>
		/// <param name="player"></param>
		protected abstract void DoActionInternal(EnemyModel model, Transform player);

		public override void NodeFunction()
		{
			EnemyModel model = (EnemyModel)getData();
			Transform player = GameObject.FindGameObjectWithTag("Player").transform;
			DoActionInternal(model, player);
		}
	}
}