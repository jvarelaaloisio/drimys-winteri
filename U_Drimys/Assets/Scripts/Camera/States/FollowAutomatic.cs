using UnityEngine;

namespace Camera.States
{
	public class FollowAutomatic<T> : CameraState<T>
	{
		protected readonly Transform transform;
		private Vector3 _nextEulerAngles;
		private float _yawAutoRotationStart;

		public FollowAutomatic(CameraModel model) : base(model)
		{
			transform = model.transform;
		}

		public override string GetName() => "Follow(Automatic)";

		public override void Awake()
		{
			_yawAutoRotationStart = 0;
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
				= Model.Target.position + transform.TransformDirection(Model.GetPositioning());
		}

		protected virtual void CalculateNextEulerAngles(ref Vector3 nextEulerAngles, float deltaTime)
		{
			var currentEulerAngles = transform.localEulerAngles;

			nextEulerAngles.x = GetNextPitch(nextEulerAngles.x, currentEulerAngles.x, deltaTime);
			nextEulerAngles.y = GetNextYaw(nextEulerAngles.y, currentEulerAngles.y, deltaTime);
		}

		/// <summary>
		/// Pitch is in the X axis
		/// </summary>
		/// <param name="previewedPitch"></param>
		/// <param name="currentPitch"></param>
		/// <param name="deltaTime"></param>
		/// <returns></returns>
		protected virtual float GetNextPitch(float previewedPitch, float currentPitch, float deltaTime)
		{
			currentPitch -= (currentPitch > 180) ? 360 : 0;
			float targetPitch = Model.Target.localEulerAngles.x;
			targetPitch -= (targetPitch > 180) ? 360 : 0;
			float deltaPitch = currentPitch - targetPitch;
			if (Mathf.Abs(deltaPitch) < .1f)
				return previewedPitch;
			return previewedPitch - Model.Properties.YieldSpeed.y * Mathf.Sign(deltaPitch) * deltaTime;
		}

		//TODO:Clean this mess without breaking it
		/// <summary>
		/// Yaw is in the Y axis
		/// </summary>
		/// <param name="previewedYaw"></param>
		/// <param name="currentYaw"></param>
		/// <param name="deltaTime"></param>
		/// <returns></returns>
		protected virtual float GetNextYaw(float previewedYaw, float currentYaw, float deltaTime)
		{
			if (LastMoveInput.x != 0)
			{
				_yawAutoRotationStart += deltaTime;
				if (_yawAutoRotationStart >= Model.Properties.YawFollowDelay)
					return currentYaw + Model.Properties.AutomaticTurnSpeed.x * LastMoveInput.x * deltaTime;
			}
			else
			{
				_yawAutoRotationStart = 0;
			}

			// if (LastMoveInput.y != 0)
			// 	return previewedYaw;
			currentYaw -= (currentYaw > 180) ? 360 : 0;
			float targetYaw = Model.Target.localEulerAngles.y;
			targetYaw -= (targetYaw > 180) ? 360 : 0;
			float deltaYaw = currentYaw - targetYaw;

			//BUG:When the player walks backwards and then stops, the camera doesn't reset.
			// if (Mathf.Abs(deltaYaw) < 2f || Mathf.Abs(deltaYaw) > 178)
			if (Mathf.Abs(deltaYaw) < 2f)
			{
				return previewedYaw;
			}
			return previewedYaw - Model.Properties.YieldSpeed.x * Mathf.Sign(deltaYaw) * deltaTime;
		}
	}
}