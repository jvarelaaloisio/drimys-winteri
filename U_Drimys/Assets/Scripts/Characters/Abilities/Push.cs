using System.Collections.Generic;
using Core.Interactions.Throwables;
using UnityEngine;

namespace Characters.Abilities
{
	[CreateAssetMenu(menuName = "Abilities/Reuse Modes/Push", fileName = "Push", order = 10)]
	public class Push : ReuseMode
	{
		[SerializeField]
		private Vector3 force;

		[SerializeField]
		private ForceMode forceMode = ForceMode.Impulse;

		public override void Reuse(Dictionary<object, object> cache, CharacterModel model, Throwable item)
		{
			model.KnockBack(force, forceMode);
			Destroy(item.gameObject);
		}
	}
}