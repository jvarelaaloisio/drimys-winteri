using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Abilities
{
	public abstract class Ability : ScriptableObject
	{
		[SerializeField]
		private string id;

		public string ID => id;

		public abstract bool CanRun(Dictionary<object, object> cache, CharacterModel model);
		public abstract void Run(Dictionary<object, object>  cache, CharacterModel model);
	}
}