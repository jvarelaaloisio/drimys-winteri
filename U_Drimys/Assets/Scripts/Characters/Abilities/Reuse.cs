using System;
using System.Collections.Generic;
using Core.Interactions.Throwables;
using UnityEngine;

namespace Characters.Abilities
{
	[CreateAssetMenu(menuName = "Abilities/Reuse", fileName = "Reuse", order = 1)]
	public class Reuse : Ability
	{
		[SerializeField]
		private ReuseMode defaultMode;

		private readonly Type _throwableKey = typeof(Throwable);
		private readonly Type _modeKey = typeof(ReuseMode);

		public override bool CanRun(Dictionary<object, object> cache, CharacterModel model)
		{
			if (!cache.ContainsKey(_throwableKey))
				cache.Add(_throwableKey, null);
			if (!cache.ContainsKey(_modeKey))
				cache.Add(_modeKey, defaultMode);
			return cache[_throwableKey] != null;
		}

		public override void Run(Dictionary<object, object> cache, CharacterModel model)
		{
			var item = (Throwable)cache[_throwableKey];
			var mode = (ReuseMode)cache[_modeKey];
			mode.Reuse(cache, model, item);
			ResetCacheItem(cache);
			model.Flags.IsRunningAbility = false;
		}

		private void DestroyItem(Dictionary<object, object> cache, Throwable item)
		{
			Destroy(item.gameObject);
		}

		private void ResetCacheItem(Dictionary<object, object> cache)
		{
			cache[_throwableKey] = null;
		}
	}
}