using System.Collections;
using UnityEngine;

namespace Enemies.IA.Actions
{
	public class MeleePlayer : EnemyAction
	{
		protected override void DoActionInternal(EnemyModel model, Transform player)
		{
			model.TryLock("Player");
			model.Melee(AttackBehaviour(model, player),player.transform);
		}

		private static IEnumerator AttackBehaviour(EnemyModel model, Transform player)
		{
			yield return new WaitForSeconds(1.3f);
			if(Vector3.Distance(player.position, model.transform.position) <= model.Properties.AttackDistance)
				player.SendMessage("TakeDamage",model.Properties.MeleeDamage);
		}
	}
}