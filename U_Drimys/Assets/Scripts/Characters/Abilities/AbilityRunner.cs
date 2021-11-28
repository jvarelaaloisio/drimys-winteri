using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.Abilities
{
	public class AbilityRunner : MonoBehaviour
	{
		public List<Ability> abilities;

		private readonly Dictionary<object, object> _cache = new Dictionary<object, object>();

		public void TryRunAbility(string id, CharacterModel model)
		{
			var ability = abilities.FirstOrDefault(ab => ab.ID == id);
			if (ability && ability.CanRun(_cache, model))
				ability.Run(_cache, model);
		}
		public void TryRunAbility(int index, CharacterModel model)
		{
			if (abilities.Count - 1< index)
				throw new IndexOutOfRangeException($"No ability at index {index} found. Max available index is {abilities.Count - 1}");
			var ability = abilities[index];
			if (ability && ability.CanRun(_cache, model))
				ability.Run(_cache, model);
		}
	}
}