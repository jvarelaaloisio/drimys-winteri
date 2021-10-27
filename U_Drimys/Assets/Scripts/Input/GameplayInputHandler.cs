﻿using Player;
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

		private InputAction _movementInput;
		private Vector2 _lastMovementInput;

		private PlayerController _controller;
		private InputActionMap _actionMap;

		//TODO:Add Camera Events
		public void SetupController(PlayerController controller)
		{
			_controller = controller;
			_actionMap = input.FindActionMap(actionMapName);
			_movementInput = _actionMap.FindAction(movementActionName);
			_actionMap.FindAction(jumpActionName).performed += HandleJump;
		}

		private void Update()
		{
			ReadMovement();
		}

		private void HandleJump(InputAction.CallbackContext context)
			=> _controller.Jump();

		private void ReadMovement()
		{
			var value = _movementInput.ReadValue<Vector2>();
			if (value.Equals(_lastMovementInput))
				return;
			_lastMovementInput = value;
			_controller.Move(value);
		}
	}
}