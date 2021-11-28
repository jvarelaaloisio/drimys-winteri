using Characters;
using Characters.Abilities;
using Core.Interactions.Throwables;
using Events.Channels;
using UnityEngine;

namespace Player
{
	public class PlayerView : ThrowerView
	{
		[SerializeField]
		private float meleeAnimationDuration;

		[SerializeField]
		private float meleeEffectDelay;

		[SerializeField]
		private Throwable meleeEffect;

		[SerializeField]
		private Transform meleeEffectSpawnPoint;
		
		protected new PlayerController Controller;

		private AbilityRunner _abilityRunner;

		[Space, Header("Channels Listened")]
		[SerializeField, Tooltip("Not Null")]
		private VoidChannelSo jump;
		[SerializeField, Tooltip("Not Null")]
		private VoidChannelSo lockOnTarget;
		[SerializeField, Tooltip("Not Null")]
		private VoidChannelSo melee;
		[SerializeField, Tooltip("Not Null")]
		private VoidChannelSo aim;
		[SerializeField, Tooltip("Not Null")]
		private VoidChannelSo shoot;
		[SerializeField, Tooltip("Not Null")]
		private VoidChannelSo ability1;
		[SerializeField, Tooltip("Not Null")]
		private Vector2ChannelSo move;
		protected override void Awake()
		{
			base.Awake();
			Controller = new PlayerController(Model);
			_abilityRunner = GetComponent<AbilityRunner>();
			
			move.Subscribe(Controller.Move);
			jump.Subscribe(Controller.Jump);
			lockOnTarget.Subscribe(Controller.Lock);
			melee.Subscribe(Melee);
			aim.Subscribe(Controller.StartAim);
			shoot.Subscribe(Controller.Shoot);
			ability1.Subscribe(() => Controller.RunAbility1(_abilityRunner));
		}

		private void Melee()
		{
			Controller.Melee(meleeAnimationDuration,
							meleeEffectDelay,
							meleeEffect,
							meleeEffectSpawnPoint);
		}
		
#if UNITY_EDITOR
		private void OnGUI()
		{
			Rect rect = new Rect(10, 50, 150, 250);
			GUILayout.BeginArea(rect, Texture2D.blackTexture);
			// GUILayout.Box(Texture2D.blackTexture, GUILayout.Height(150));
			GUI.skin.label.fontSize = 15;
			GUI.skin.label.normal.textColor = Color.white;
			GUILayout.Label($"State: <color=green>{Model.GetCurrentState.GetName()}</color>");
			// GUI.skin.label.normal.textColor = controller.Stamina.IsRefillingActive ? Color.green : Color.red;

			// GUILayout.Label("Stamina: " + controller.Stamina.FillState);
			GUILayout.EndArea();
		}
#endif
	}
}