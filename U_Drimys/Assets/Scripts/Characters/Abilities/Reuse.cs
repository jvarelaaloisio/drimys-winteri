﻿using System;
using System.Collections.Generic;
using Core.Interactions.Throwables;
using Events.UnityEvents;
using UnityEngine;

namespace Characters.Abilities
{
	[CreateAssetMenu(menuName = "Characters/Abilities/Reuse", fileName = "Reuse", order = 1)]
	public class Reuse : Ability
	{
		public enum Mode
		{
			Push,
			Heal,
			Stun
		}

		[SerializeField]
		private float pushForce;

		private readonly Type _throwableKey = typeof(Throwable);
		private readonly Type _modeKey = typeof(Mode);
		
		[SerializeField]
		private Vector3UnityEvent onPush;

		[SerializeField]
		private float healPoints;

		public override bool CanRun(Dictionary<object, object> cache, CharacterModel model)
		{
			if (!cache.ContainsKey(_throwableKey))
				cache.Add(_throwableKey, null);
			if(!cache.ContainsKey(_modeKey))
				cache.Add(_modeKey, Mode.Push);
			return cache[_throwableKey] != null;
		}

		public override void Run(Dictionary<object, object> cache, CharacterModel model)
		{
			var item = (Throwable)cache[_throwableKey];
			//TODO:This should be handled by the model?
			model.Flags.IsRunningAbility = false;
			switch (cache[_modeKey])
			{
				case Mode.Push:
					model.rigidbody.AddForce(Vector3.up * pushForce, ForceMode.Impulse);
					onPush.Invoke(item.transform.position);
					Destroy(item.gameObject);
					break;
				case Mode.Heal:
					model.transform.SendMessage("Heal", healPoints);
					Debug.Log("Heal");
					break;
				case Mode.Stun:
					//TODO:Stun Logic
					Debug.Log("Stun");
					break;
			}
		}
	}
}