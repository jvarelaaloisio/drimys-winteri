using UnityEngine;
using UnityEngine.Serialization;

namespace Camera
{
	[CreateAssetMenu(menuName = "Properties/Camera/Properties", fileName = "CameraProperties", order = 0)]
	public class CameraProperties : ScriptableObject
	{
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

		[SerializeField]
		private float yawFollowDelay;

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
		public float YawFollowDelay => yawFollowDelay;
	}
}