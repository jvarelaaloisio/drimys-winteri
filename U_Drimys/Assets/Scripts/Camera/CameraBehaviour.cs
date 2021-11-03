using Core.Helpers;
using UnityEngine;

namespace Camera
{
	public class CameraBehaviour : MonoBehaviour, ICoroutineRunner
	{
		[SerializeField]
		private Vector3 offsetFromPlayer;

		[SerializeField]
		private Transform player;

		[SerializeField]
		private Vector2 inputTurnSpeed;

		[SerializeField]
		private float automaticTurnSpeed;

		[SerializeField]
		private float minimumAngle;

		[SerializeField]
		private float maximumAngle;

		[SerializeField]
		private bool isInvertedVer;

		private Transform _transform;

		private float _deltaVerticalRotation = 0;
		private Vector2 _lastMoveInput;
		private Vector2 _lastCamInput;

		private void Awake()
		{
			_transform = transform;
			var playerRotation = player.rotation;
			_transform.rotation = playerRotation;
		}

		private void LateUpdate()
		{
			Vector3 nextEulerAngles = Vector3.zero;
			bool isManualMode = _lastCamInput.x != 0;
			if (isManualMode)
			{
				nextEulerAngles.y = _transform.localEulerAngles.y
								+ inputTurnSpeed.x * Time.deltaTime * _lastCamInput.x;
			}
			else
			{
				nextEulerAngles.y = _transform.localEulerAngles.y
									+ automaticTurnSpeed * Time.deltaTime * _lastMoveInput.x;
			}

			//TODO:Cam should rotate smoothly towards deltaRotation 0 when there's no input.
			nextEulerAngles.x = GetVerticalRotationFromInput();
			
			transform.localEulerAngles = nextEulerAngles;

			if (!player.hasChanged)
				return;
			_transform.position = player.position + _transform.TransformDirection(offsetFromPlayer);
		}

		public void HandleMoveInput(Vector2 input)
			=> _lastMoveInput = input;

		public void HandleCamInput(Vector2 input)
			=> _lastCamInput = input;

		private float GetVerticalRotationFromInput()
		{
			int direction = isInvertedVer ? -1 : 1;
			var rotationAngle = _lastCamInput.y * inputTurnSpeed.y * direction * Time.deltaTime;
			if (rotationAngle > 0 && _deltaVerticalRotation >= maximumAngle
				|| rotationAngle < 0 && _deltaVerticalRotation <= minimumAngle)
				return _deltaVerticalRotation;
			_deltaVerticalRotation += rotationAngle;
			return _deltaVerticalRotation;
		}
	}
}