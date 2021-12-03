using System;
using System.Collections;
using System.Collections.Generic;
using Events.UnityEvents;
using UnityEngine;

namespace Characters.Abilities
{
	using Mode = Reuse.Mode;

	[RequireComponent(typeof(AbilityRunner))]
	public class ReuseModeHandler : MonoBehaviour
	{
		public StringUnityEvent onChangedMode;

		[SerializeField]
		private float rotationPeriod;

		private readonly Mode[] _modes = { Mode.Push, Mode.Heal, Mode.Stun };
		private AbilityRunner _abilityRunner;
		private readonly Type _modeKey = typeof(Mode);

		private void Awake()
		{
			_abilityRunner = GetComponent<AbilityRunner>();
			if (_abilityRunner.Cache.ContainsKey(_modeKey))
				_abilityRunner.Cache.Add(_modeKey, _modes[0]);
			StartCoroutine(RotateModes(_abilityRunner.Cache,
										_modeKey,
										_modes,
										rotationPeriod,
										m => onChangedMode.Invoke(m.ToString())));
			Application.quitting += () => StopCoroutine(nameof(RotateModes));
		}

		private static IEnumerator RotateModes(Dictionary<object, object> cache,
												object key,
												Mode[] modes,
												float period,
												Action<Mode> onChanged = null)
		{
			var wait = new WaitForSeconds(period);
			while (true)
				for (int i = 0; i < 3; i++)
				{
					cache[key] = modes[i];
					onChanged?.Invoke(modes[i]);
					yield return wait;
				}
		}
#if UNITY_EDITOR
		private void OnGUI()
		{
			if (!Application.isPlaying)
				return;
			Rect rect = new Rect(10, 100, 150, 250);
			GUILayout.BeginArea(rect, Texture2D.blackTexture);
			GUI.skin.label.fontSize = 15;
			var mode = _abilityRunner.Cache[_modeKey];
			Color color;
			switch (mode)
			{
				case Mode.Heal:
					color = Color.green;
					break;
				case Mode.Push:
					color = Color.blue;
					break;
				case Mode.Stun:
					color = Color.red;
					break;
				default:
					color = Color.black;
					break;
			}

			GUI.skin.label.normal.textColor = color;
			GUILayout.Label($"Reuse Mode: {mode}");
			GUILayout.EndArea();
		}
#endif
	}
}