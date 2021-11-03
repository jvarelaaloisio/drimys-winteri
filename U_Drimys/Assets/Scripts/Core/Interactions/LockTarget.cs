using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Interactions
{
	public class LockTarget : MonoBehaviour
	{
		public UnityEvent onDisabling;

		private void OnDisable()
		{
			onDisabling.Invoke();
			onDisabling.RemoveAllListeners();
		}
	}
}
