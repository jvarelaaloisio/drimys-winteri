using System;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
	public class GameplayInputHandler : MonoBehaviour
	{
		[SerializeField]
		private InputActionAsset input;

		[SerializeField]
		private string actionMapName;

		[SerializeField]
		private string jumpActionName;

		[SerializeField]
		private string movementActionName;


		private PlayerController _controller;
		private InputActionMap _actionMap;
		private InputAction _movementInput;

		//TODO:Add Camera Events
		public void SetupController(PlayerController controller)
		{
			_controller = controller;
			_actionMap = input.FindActionMap(actionMapName, true);
			_movementInput = _actionMap.FindAction(movementActionName);
			// movementInput.started += HandleMovement;
			// movementInput.performed += HandleMovement;
			// movementInput.canceled += HandleMovement;
			_actionMap.FindAction(jumpActionName).performed += HandleJump;
		}

		private void Update()
		{
			_controller.Move(_movementInput.ReadValue<Vector2>());
		}

		private void HandleJump(InputAction.CallbackContext context)
			=> _controller.Jump();

		private void HandleMovement(InputAction.CallbackContext context)
		{
			_controller.Move(context.ReadValue<Vector2>());
		}
	}
}