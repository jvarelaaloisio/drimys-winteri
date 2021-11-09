using UnityEngine;

namespace Camera.States
{
	public class FollowAutomatic<T> : CameraState<T>
	{
		protected readonly CameraProperties Properties;
		protected readonly Transform transform;
		private Vector3 _nextEulerAngles;
		protected Vector2 ResetSpeed;
		private float yawAutoRotationStart;

		public FollowAutomatic(CameraModel model) : base(model)
		{
			transform = model.transform;
			Properties = model.Properties;
			ResetSpeed = Properties.YieldSpeed;
		}

		public override string GetName() => "Follow(Automatic)";

		public override void Awake()
		{
			yawAutoRotationStart = 0;
			_nextEulerAngles = transform.localEulerAngles;
			base.Awake();
		}

		public override void Update(float deltaTime)
		{
			CalculateNextEulerAngles(ref _nextEulerAngles, deltaTime);

			transform.localEulerAngles = _nextEulerAngles;

			if (!Model.Target.hasChanged)
				return;
			transform.position
				= Model.Target.position + transform.TransformDirection(Model.Properties.OffsetFromPlayer);
		}

		protected virtual void CalculateNextEulerAngles(ref Vector3 nextEulerAngles, float deltaTime)
		{
			var currentEulerAngles = transform.localEulerAngles;

			nextEulerAngles.x = GetNextPitch(nextEulerAngles.x, currentEulerAngles.x, deltaTime);
			nextEulerAngles.y = GetNextYaw(nextEulerAngles.y, currentEulerAngles.y, deltaTime);
		}

		protected virtual float GetNextPitch(float previewedPitch, float currentPitch, float deltaTime)
		{
			currentPitch -= (currentPitch > 180) ? 360 : 0;
			float targetPitch = Model.Target.localEulerAngles.x;
			targetPitch -= (targetPitch > 180) ? 360 : 0;
			float deltaPitch = currentPitch - targetPitch;
			if (Mathf.Abs(deltaPitch) < .1f)
				return previewedPitch;
			return previewedPitch - ResetSpeed.x * Mathf.Sign(deltaPitch) * deltaTime;
		}

		//TODO:Clean this mess without breaking it
		protected virtual float GetNextYaw(float previewedYaw, float currentYaw, float deltaTime)
		{
			if (LastMoveInput.x != 0)
			{
				yawAutoRotationStart += deltaTime;
				if (yawAutoRotationStart >= Properties.YawFollowDelay)
					return currentYaw + Model.Properties.AutomaticTurnSpeed.x * LastMoveInput.x * deltaTime;
			}
			else
			{
				yawAutoRotationStart = 0;
			}

			if (LastMoveInput.y != 0)
				return previewedYaw;
			currentYaw -= (currentYaw > 180) ? 360 : 0;
			float targetYaw = Model.Target.localEulerAngles.y;
			targetYaw -= (targetYaw > 180) ? 360 : 0;
			float deltaYaw = currentYaw - targetYaw;

			if (Mathf.Abs(deltaYaw) < 1f || Mathf.Abs(deltaYaw) > 179)
			{
				return previewedYaw;
			}
			return previewedYaw - ResetSpeed.y * Mathf.Sign(deltaYaw) * deltaTime;
		}
	}
}