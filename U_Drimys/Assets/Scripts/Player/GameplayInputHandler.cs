using Events.UnityEvents;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Input
{
	public class GameplayInputHandler : MonoBehaviour
	{
		public UnityEvent onJumpInput;
		public Vector2UnityEvent onMoveInput;
		public Vector2UnityEvent onCameraInput;

		[SerializeField]
		private InputActionAsset input;

		[SerializeField]
		private string actionMapName;

		[SerializeField]
		private string jumpActionName;

		[SerializeField]
		private string movementActionName;
		
		[SerializeField]
		private string cameraActionName;


		private PlayerController _controller;
		private InputActionMap _actionMap;
		private InputAction _movementInput;

		public void SetupController(PlayerController controller)
		{
			_controller = controller;
			_actionMap = input.FindActionMap(actionMapName, true);
			_movementInput = _actionMap.FindAction(movementActionName);
			_actionMap.FindAction(jumpActionName).performed += HandleJump;
			_actionMap.FindAction(cameraActionName).started += HandleCamera;
			_actionMap.FindAction(cameraActionName).performed += HandleCamera;
			_actionMap.FindAction(cameraActionName).canceled += HandleCamera;
		}

		private void HandleCamera(InputAction.CallbackContext obj)
			=> onCameraInput.Invoke(obj.ReadValue<Vector2>());

		private void Update()
		{
			var movementInput = _movementInput.ReadValue<Vector2>();
			_controller.Move(movementInput);
			onMoveInput.Invoke(movementInput);
			// onCameraInput.Invoke(_actionMap.FindAction(cameraActionName).ReadValue<Vector2>());
		}

		private void HandleJump(InputAction.CallbackContext context)
			=> _controller.Jump();
	}
}