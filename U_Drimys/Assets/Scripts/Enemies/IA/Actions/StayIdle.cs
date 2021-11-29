using Characters;
using IA.DecisionTree;
using UnityEngine;

namespace Enemies.IA.Actions
{
	public class StayIdle : EnemyAction
	{
		protected override void DoActionInternal(EnemyModel model, Transform player)
		{
			model.Unlock();
			callback(CharacterModel.IDLE_STATE);
		}
	}
}
