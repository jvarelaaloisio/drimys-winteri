using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interactions.Throwables;
using UnityEngine;

namespace Characters.Abilities
{
	[CreateAssetMenu(menuName = "Characters/Abilities/Catch", fileName = "Catch", order = 0)]
	public class Catch : Ability
	{
		private readonly Type _throwableKey = typeof(Throwable);

		[SerializeField]
		private float flySpeed;

		[SerializeField]
		private float minimumHeight;
		
		public override bool CanRun(Dictionary<object, object> cache, CharacterModel model)
		{
			if (!cache.ContainsKey(_throwableKey))
				cache.Add(_throwableKey, null);
			return !model.Flags.IsRunningAbility && cache[_throwableKey] == null;
		}

		public override void Run(Dictionary<object, object> cache, CharacterModel model)
		{
			cache[_throwableKey] = TryCatch(model);
		}

		private Throwable TryCatch(CharacterModel model)
		{
			if (!FetchItem(model.transform,
							minimumHeight,
							out var item))
				return item;
			//TODO:This should be handled by the model?
			model.Flags.IsRunningAbility = true;
			item.StopDeath();
			var catchHelper = model.transform.Find("CatchHelper");
			item.FlyTargeted(catchHelper,
						flySpeed,
						() => item.transform.SetParent(catchHelper));
			item.Freeze();
			return item;
		}

		private static bool FetchItem(Transform user,
									float minimumHeight,
									out Throwable item)
		{
			Vector3 userPosition = user.position;
			Throwable[] items = FindObjectsOfType<Throwable>();
			items = items
					.OrderByDescending(throwable => Vector3.Distance(userPosition, throwable.transform.position))
					.ToArray();

			for (var i = 0; i < items.Length; i++)
			{
				float color = 1.0f * i / items.Length;
				Debug.DrawLine(userPosition, items[i].transform.position, new Color(color, 0, .35f), 2f);
			}

			return item = items.FirstOrDefault(nut => nut.transform.position.y >= userPosition.y + minimumHeight);
		}
	}
}