using Events.UnityEvents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player
{
	public class GameplayInputHandler : MonoBehaviour
	{
		public UnityEvent onJumpInput;
		public UnityEvent onLockInput;
		public Vector2UnityEvent onMoveInput;
		public Vector2UnityEvent onCameraInput;

		[SerializeField]
		private InputActionAsset input;

		[SerializeField]
		private string actionMapName;

		[SerializeField]
		private string jumpActionName;
		
		[SerializeField]
		private string lockActionName;

		[SerializeField]
		private string movementActionName;
		
		[SerializeField]
		private string cameraActionName;


		private InputActionMap _actionMap;
		private InputAction _movementInput;

		private void Awake()
		{
			_actionMap = input.FindActionMap(actionMapName, true);
			_movementInput = _actionMap.FindAction(movementActionName);
			_actionMap.FindAction(jumpActionName).performed += HandleJump;
			_actionMap.FindAction(lockActionName).performed += HandleLock;
			_actionMap.FindAction(cameraActionName).started += HandleCamera;
			_actionMap.FindAction(cameraActionName).performed += HandleCamera;
			_actionMap.FindAction(cameraActionName).canceled += HandleCamera;
		}

		private void HandleLock(InputAction.CallbackContext obj)
			=> onLockInput.Invoke();

		private void HandleCamera(InputAction.CallbackContext obj)
			=> onCameraInput.Invoke(obj.ReadValue<Vector2>());

		private void Update()
		{
			var movementInput = _movementInput.ReadValue<Vector2>();
			onMoveInput.Invoke(movementInput);
		}

		private void HandleJump(InputAction.CallbackContext context)
			=> onJumpInput.Invoke();
	}
}