using UnityEngine;

namespace Camera.States
{
	public class FollowFullInput<T> : FollowWithPitchInput<T>
	{
		public FollowFullInput(CameraModel model) : base(model)
		{
		}

		public override string GetName() => "Follow(Full Input)";

		// protected override void CalculateNextEulerAngles(ref Vector3 nextEulerAngles, float deltaTime)
		// {
		// 	nextEulerAngles.x = GetPitchFromInput(deltaTime);
		// 	nextEulerAngles.y = transform.localEulerAngles.y
		// 						+ Properties.InputTurnSpeed.x * LastCamInput.x * deltaTime;
		// }

		protected override float GetNextPitch(float previewedPitch, float currentPitch, float deltaTime)
		{
			return GetPitchFromInput(deltaTime);
		}

		protected override float GetNextYaw(float previewedYaw, float currentYaw, float deltaTime)
		{
			return transform.localEulerAngles.y + Properties.InputTurnSpeed.x * LastCamInput.x * deltaTime;
		}
	}
}