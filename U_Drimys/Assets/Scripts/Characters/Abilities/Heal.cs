using System.Collections.Generic;
using Core.Interactions.Throwables;
using UnityEngine;

namespace Characters.Abilities
{
	[CreateAssetMenu(menuName = "Abilities/Reuse Modes/Heal", fileName = "Heal", order = 0)]
	public class Heal : ReuseMode
	{
		[SerializeField]
		private float healPoints;

		[SerializeField]
		private GameObject effect;


		public override void Reuse(Dictionary<object, object> cache, CharacterModel model, Throwable item)
		{
			model.transform.SendMessage("Heal", healPoints);
			var itemTransform = item.transform;
			if (effect)
				Instantiate(effect, itemTransform.position, itemTransform.rotation, null);
			Destroy(item.gameObject);
		}
	}
}