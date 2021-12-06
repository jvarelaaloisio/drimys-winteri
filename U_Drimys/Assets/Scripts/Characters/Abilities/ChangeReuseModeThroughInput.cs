using System;
using Events.Channels;
using Events.UnityEvents;
using UnityEngine;

namespace Characters.Abilities
{
	using Mode = Reuse.Mode;

	[RequireComponent(typeof(AbilityRunner))]
	public class ChangeReuseModeThroughInput : MonoBehaviour
	{
		public StringUnityEvent onChangedMode;

		[Header("Events Listened")]
		[SerializeField]
		[Tooltip("Not Null")]
		private IntChannelSo changeModeIndex;

		private readonly Mode[] _modes = { Mode.Push, Mode.Heal, Mode.Stun };
		private int _currentIndex;
		private AbilityRunner _abilityRunner;
		private readonly Type _modeKey = typeof(Mode);

		private void Awake()
		{
			_abilityRunner = GetComponent<AbilityRunner>();
			if (!_abilityRunner.Cache.ContainsKey(_modeKey))
				_abilityRunner.Cache.Add(_modeKey, _modes[0]);
			changeModeIndex.Subscribe(ChangeModeIndex);
		}

		private void ChangeModeIndex(int i)
		{
			_currentIndex += i;
			int modesQty = _modes.Length;
			if (_currentIndex < 0)
				_currentIndex += modesQty;
			else if (_currentIndex > modesQty - 1)
				_currentIndex -= modesQty;
			
			_abilityRunner.Cache[_modeKey] = _modes[_currentIndex];
			onChangedMode.Invoke(_modes[_currentIndex].ToString());
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