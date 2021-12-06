using System.Collections.Generic;
using Core.Interactions.Throwables;
using UnityEngine;

namespace Characters.Abilities
{
	[CreateAssetMenu(menuName = "Abilities/Reuse Modes/Stun", fileName = "Stun", order = 5)]
	public class Stun : ReuseMode
	{
		[SerializeField]
		private float flySpeed;

		public override void Reuse(Dictionary<object, object> cache, CharacterModel model, Throwable item)
		{
			if (!model.Flags.IsLocked) return;
			var target = model.LockTargetTransform;
			item.transform.SetParent(null);
			item.FlyTargeted(target, flySpeed, item.Stun);
		}
	}
}