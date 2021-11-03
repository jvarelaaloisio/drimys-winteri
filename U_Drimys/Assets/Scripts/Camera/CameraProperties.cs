using UnityEngine;
using UnityEngine.Serialization;

namespace Camera
{
	[CreateAssetMenu(menuName = "Properties/Camera", fileName = "CameraProperties", order = 0)]
	public class CameraProperties : ScriptableObject
	{
		[Header("Positioning")]
		[SerializeField]
		private Vector3 offsetFromPlayer;

		[Header("Turning")]
		[SerializeField]
		private Vector2 inputTurnSpeed;

		[SerializeField]
		private Vector2 automaticTurnSpeed;

		[SerializeField]
		private float minimumPitchAngle;

		[SerializeField]
		private float maximumPitchAngle;

		[SerializeField]
		private float pitchResetSpeed;
		
		[SerializeField]
		private Vector2 yieldSpeed;

		[SerializeField]
		private float automaticYieldTime;

		[SerializeField]
		private Vector2 lockSpeed;
		
		[FormerlySerializedAs("lockTime")]
		[SerializeField]
		private float lockDuration;

		[SerializeField]
		private bool isInvertedPitch;

		public Vector3 OffsetFromPlayer => offsetFromPlayer;

		public Vector2 InputTurnSpeed => inputTurnSpeed;

		public Vector2 AutomaticTurnSpeed => automaticTurnSpeed;

		public float MinimumPitchAngle => minimumPitchAngle;

		public float MaximumPitchAngle => maximumPitchAngle;

		public float PitchResetSpeed => pitchResetSpeed;
		
		public float AutomaticYieldTime => automaticYieldTime;

		public Vector2 YieldSpeed => yieldSpeed;

		public Vector2 LockSpeed => lockSpeed;
		
		public bool IsInvertedPitch => isInvertedPitch;

		public float LockDuration => lockDuration;
	}
}