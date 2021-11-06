using Characters;
using UnityEngine;

namespace Player
{
	public class PlayerView : ThrowerView
	{
		[SerializeField]
		private GameplayInputHandler inputHandler;

		protected new PlayerController Controller;
		protected override void Awake()
		{
			base.Awake();
			Controller = new PlayerController(Model);
			inputHandler.onJumpInput.AddListener(Controller.Jump);
			inputHandler.onLockInput.AddListener(Controller.Lock);
			inputHandler.onAimInput.AddListener(() => Controller.StartAim());
			inputHandler.onShootInput.AddListener(Controller.Shoot);
			inputHandler.onMoveInput.AddListener(HandleMove);
		}

		private void HandleMove(Vector2 input)
			=> Controller.Move(input);
	}
}