using Events.UnityEvents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
	public class GameplayInputHandler : MonoBehaviour
	{
		public UnityEvent onJumpInput;
		public UnityEvent onLockInput;
		public UnityEvent onMeleeInput;
		public UnityEvent onAbility1Input;
		[FormerlySerializedAs("onAim")]
		public UnityEvent onAimInput;
		[FormerlySerializedAs("onShoot")]
		public UnityEvent onShootInput;
		public Vector2UnityEvent onMoveInput;
		public Vector2UnityEvent onCameraInput;

		[SerializeField]
		private InputActionAsset input;

		[SerializeField]
		private string actionMapName;

		[SerializeField]
		private string jumpActionName = "Jump";
		
		[SerializeField]
		private string lockActionName = "Lock";
		
		[SerializeField]
		private string meleeActionName = "Melee";
		
		[SerializeField]
		private string shootActionName = "Shoot";
		
		[SerializeField]
		private string ability1ActionName = "Ability1";

		[SerializeField]
		private string movementActionName = "Movement";
		
		[SerializeField]
		private string cameraActionName = "Camera";


		private InputActionMap _actionMap;
		private InputAction _movementInput;

		private void Awake()
		{
			_actionMap = input.FindActionMap(actionMapName, true);
			_movementInput = _actionMap.FindAction(movementActionName);
			_actionMap.FindAction(jumpActionName).performed += HandleJump;
			_actionMap.FindAction(lockActionName).performed += HandleLock;
			_actionMap.FindAction(meleeActionName).started += _ => onMeleeInput.Invoke();
			_actionMap.FindAction(shootActionName).started += _ => onAimInput.Invoke();
			_actionMap.FindAction(shootActionName).canceled += _ => onShootInput.Invoke();
			_actionMap.FindAction(ability1ActionName).started += _ => onAbility1Input.Invoke();
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