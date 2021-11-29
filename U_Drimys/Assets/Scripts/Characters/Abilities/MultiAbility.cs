using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.Abilities
{
	[CreateAssetMenu(menuName = "Characters/Abilities/Multi Ability", fileName = "MultiAbility", order = 99)]
	public class MultiAbility : Ability
	{
		[SerializeField]
		private List<Ability> abilities;

		private Ability lastSelection;
		public override bool CanRun(Dictionary<object, object> cache, CharacterModel model)
		{
			lastSelection = abilities.FirstOrDefault(a => a.CanRun(cache, model));
			return lastSelection != null;
		}

		public override void Run(Dictionary<object, object> cache, CharacterModel model)
		{
			lastSelection.Run(cache, model);
		}
	}
}