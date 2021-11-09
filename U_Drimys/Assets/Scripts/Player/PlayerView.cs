using Characters;
using Core.Interactions.Throwables;
using UnityEngine;

namespace Player
{
	public class PlayerView : ThrowerView
	{
		[SerializeField]
		private GameplayInputHandler inputHandler;

		[SerializeField]
		private float meleeAnimationDuration;
		
		[SerializeField]
		private float meleeEffectDelay;
		
		[SerializeField]
		private Throwable meleeEffect;
		
		[SerializeField]
		private Transform meleeEffectSpawnPoint;
		
		protected new PlayerController Controller;

		protected override void Awake()
		{
			base.Awake();
			Controller = new PlayerController(Model);
			inputHandler.onJumpInput.AddListener(Controller.Jump);
			inputHandler.onLockInput.AddListener(Controller.Lock);
			inputHandler.onMeleeInput.AddListener(() => Controller.Melee(meleeAnimationDuration,
																		meleeEffectDelay,
																		meleeEffect,
																		meleeEffectSpawnPoint));
			inputHandler.onAimInput.AddListener(Controller.StartAim);
			inputHandler.onShootInput.AddListener(Controller.Shoot);
			inputHandler.onMoveInput.AddListener(HandleMove);
		}

		private void HandleMove(Vector2 input)
			=> Controller.Move(input);
	}
}