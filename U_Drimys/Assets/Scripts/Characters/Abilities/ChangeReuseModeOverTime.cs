using System;
using System.Collections;
using System.Collections.Generic;
using Events.UnityEvents;
using UnityEngine;

namespace Characters.Abilities
{
	[RequireComponent(typeof(AbilityRunner))]
	public class ChangeReuseModeOverTime : MonoBehaviour
	{
		public StringUnityEvent onChangedMode;

		[SerializeField]
		private float rotationPeriod;

		[SerializeField]
		private ReuseMode[] modes;

		private int _currentIndex;
		private AbilityRunner _abilityRunner;
		private readonly Type _modeKey = typeof(ReuseMode);

		private void Awake()
		{
			_abilityRunner = GetComponent<AbilityRunner>();
			if (modes.Length < 1)
			{
				Debug.Log("No reuse modes were provided", gameObject);
				return;
			}
			if (_abilityRunner.Cache.ContainsKey(_modeKey))
				_abilityRunner.Cache.Add(_modeKey, modes[0]);
			StartCoroutine(RotateModes(_abilityRunner.Cache,
										_modeKey,
										modes,
										rotationPeriod,
										m => onChangedMode.Invoke(m.ToString())));
			Application.quitting += () => StopCoroutine(nameof(RotateModes));
		}

		private static IEnumerator RotateModes(Dictionary<object, object> cache,
												object key,
												ReuseMode[] modes,
												float period,
												Action<ReuseMode> onChanged = null)
		{
			var wait = new WaitForSeconds(period);
			while (true)
				foreach (var mode in modes)
				{
					cache[key] = mode;
					onChanged?.Invoke(mode);
					yield return wait;
				}
		}
#if UNITY_EDITOR
		private void OnGUI()
		{
			if (!Application.isPlaying || !_abilityRunner.Cache.ContainsKey(_modeKey))
				return;
			Rect rect = new Rect(10, 100, 150, 250);
			GUILayout.BeginArea(rect, Texture2D.blackTexture);
			GUI.skin.label.fontSize = 15;
			var mode = _abilityRunner.Cache[_modeKey];
			GUILayout.Label($"Reuse Mode: {mode}");
			GUILayout.EndArea();
		}
#endif
	}
}