using System.Collections.Generic;
using Core.Interactions.Throwables;
using UnityEngine;

namespace Characters.Abilities
{
	public abstract class ReuseMode : ScriptableObject
	{
		public abstract void Reuse(Dictionary<object, object> cache, CharacterModel model, Throwable item);
	}
}