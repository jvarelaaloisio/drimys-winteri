using UnityEngine;

namespace Camera
{
	public class CameraView : MonoBehaviour
	{
		[SerializeField]
		private Vector3 offsetFromPlayer;

		[SerializeField]
		private Transform player;

		[SerializeField]
		private Vector2 turnSpeed;

		[SerializeField]
		private float minimumAngle;

		[SerializeField]
		private float maximumAngle;

		[SerializeField]
		private bool isInvertedVer;

		private Transform _transform;

		private float _deltaVerticalRotation = 0;
		private Vector3 _nextEulerAngles = new Vector3();
		
		private void Awake()
		{
			_transform = transform;
			var playerRotation = player.rotation;
			_transform.rotation = playerRotation;
		}

		private void LateUpdate()
		{
			_transform.localEulerAngles = _nextEulerAngles;
			if (!player.hasChanged)
				return;
			_transform.position = player.position + _transform.TransformDirection(offsetFromPlayer);
		}

		public void HandlePlayerMovement(Vector2 input)
		{
			if (input.x == 0)
				return;
			// _transform.Rotate(Vector3.up, turnSpeed.x * Time.deltaTime * input.x);
			_nextEulerAngles.y = _transform.localEulerAngles.y + turnSpeed.x * Time.deltaTime * input.x;
		}

		public void HandleMouseMovement(Vector2 input)
		{
			int direction = isInvertedVer ? -1 : 1;
			var rotationAngle = input.y * turnSpeed.y * direction * Time.deltaTime;
			if (rotationAngle > 0 && _deltaVerticalRotation  >= maximumAngle
				|| rotationAngle < 0 && _deltaVerticalRotation <= minimumAngle)
				return;
			_deltaVerticalRotation += rotationAngle;
			//TODO:I'm getting a weird rotation in the Z axis
			// _transform.Rotate(Vector3.right, rotationAngle);
			// _nextEulerAngles.x = _transform.localEulerAngles.x + rotationAngle;
			_nextEulerAngles.x = _deltaVerticalRotation;
		}
	}
}