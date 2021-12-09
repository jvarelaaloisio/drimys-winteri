using System;
using Events.Channels;
using Events.UnityEvents;
using UnityEngine;

namespace Characters.Abilities
{
	[RequireComponent(typeof(AbilityRunner))]
	public class ChangeReuseModeThroughInput : MonoBehaviour
	{
		public StringUnityEvent onChangedMode;

		[SerializeField]
		private ReuseMode[] modes;

		[Header("Events Listened")]
		[SerializeField]
		[Tooltip("Not Null")]
		private IntChannelSo changeModeIndex;

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
			if (!_abilityRunner.Cache.ContainsKey(_modeKey))
				_abilityRunner.Cache.Add(_modeKey, modes[0]);
			changeModeIndex.Subscribe(ChangeModeIndex);
		}

		private void Start()
		{
			ChangeModeIndex(0);
		}

		private void ChangeModeIndex(int i)
		{
			_currentIndex += i;
			int modesQty = modes.Length;
			if (_currentIndex < 0)
				_currentIndex += modesQty;
			else if (_currentIndex > modesQty - 1)
				_currentIndex -= modesQty;
			
			_abilityRunner.Cache[_modeKey] = modes[_currentIndex];
			onChangedMode.Invoke(modes[_currentIndex].ToString());
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