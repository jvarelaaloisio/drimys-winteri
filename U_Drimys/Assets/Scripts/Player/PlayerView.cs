using Characters;
using UnityEngine;

namespace Player
{
	public class PlayerView : AnimatedCharacterView
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
			inputHandler.onMoveInput.AddListener(HandleMove);
		}

		private void HandleMove(Vector2 input)
			=> Controller.Move(input);
	}
}