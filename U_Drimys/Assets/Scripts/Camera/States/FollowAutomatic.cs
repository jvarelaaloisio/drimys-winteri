using UnityEngine;

namespace Camera.States
{
	public class FollowAutomatic<T> : CameraState<T>
	{
		protected readonly CameraProperties Properties;
		protected readonly Transform transform;
		private Vector3 _nextEulerAngles;
		protected Vector2 ResetSpeed;

		public FollowAutomatic(CameraModel model) : base(model)
		{
			transform = model.transform;
			Properties = model.Properties;
			ResetSpeed = Properties.YieldSpeed;
		}

		public override string GetName() => "Follow(Automatic)";

		public override void Awake()
		{
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

			// CalculatePitch(ref nextEulerAngles, deltaTime, ref currentEulerAngles);
			//
			// CalculateYaw(ref nextEulerAngles, deltaTime, currentEulerAngles);
		}

		protected virtual void CalculatePitch(ref Vector3 nextEulerAngles, float deltaTime,
											ref Vector3 currentEulerAngles)
		{
			currentEulerAngles.x -= (currentEulerAngles.x > 180) ? 360 : 0;
			float targetPitch = Model.Target.localEulerAngles.x;
			targetPitch -= (targetPitch > 180) ? 360 : 0;
			float deltaPitch = currentEulerAngles.x - targetPitch;
			if (Mathf.Abs(deltaPitch) >= .1f)
				nextEulerAngles.x -= ResetSpeed.x * Mathf.Sign(deltaPitch) * deltaTime;
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

		protected virtual void CalculateYaw(ref Vector3 nextEulerAngles, float deltaTime, Vector3 currentEulerAngles)
		{
			if (LastMoveInput.x != 0)
			{
				nextEulerAngles.y = currentEulerAngles.y
									+ Model.Properties.AutomaticTurnSpeed.x * LastMoveInput.x * deltaTime;
			}
			else
			{
				currentEulerAngles.y -= (currentEulerAngles.y > 180) ? 360 : 0;
				float targetYaw = Model.Target.localEulerAngles.y;
				targetYaw -= (targetYaw > 180) ? 360 : 0;
				float deltaYaw = currentEulerAngles.y - targetYaw;
				if (Mathf.Abs(deltaYaw) >= .1f)
					nextEulerAngles.y -= ResetSpeed.y * Mathf.Sign(deltaYaw) * deltaTime;
			}
		}

		protected virtual float GetNextYaw(float previewedYaw, float currentYaw, float deltaTime)
		{
			if (LastMoveInput.x != 0)
			{
				return currentYaw + Model.Properties.AutomaticTurnSpeed.x * LastMoveInput.x * deltaTime;
			}

			currentYaw -= (currentYaw > 180) ? 360 : 0;
			float targetYaw = Model.Target.localEulerAngles.y;
			targetYaw -= (targetYaw > 180) ? 360 : 0;
			float deltaYaw = currentYaw - targetYaw;
			if (Mathf.Abs(deltaYaw) < .1f)
				return previewedYaw;
			return previewedYaw - ResetSpeed.y * Mathf.Sign(deltaYaw) * deltaTime;
		}
	}
}