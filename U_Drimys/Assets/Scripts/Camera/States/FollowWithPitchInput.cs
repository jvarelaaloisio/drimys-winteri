using System;
using UnityEngine;

namespace Camera.States
{
	public class FollowWithPitchInput<T> : FollowAutomatic<T>
	{
		protected float DeltaPitch = 0;

		private float _lastCamInputTime;

		public FollowWithPitchInput(CameraModel model) : base(model) { }

		/// <summary>
		/// Event risen when sufficient time has passed since the state read cam input.
		/// </summary>
		public event Action onYield = delegate { };

		public override string GetName() => "Follow(Pitch Input)";

		public override void Awake()
		{
			DeltaPitch = transform.localEulerAngles.x;
			DeltaPitch -= (DeltaPitch > 180) ? 360 : 0;
			_lastCamInputTime = Time.time;
			base.Awake();
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);
			if (LastCamInput.magnitude != 0)
				_lastCamInputTime = Time.time;
			else if (_lastCamInputTime + Model.Properties.AutomaticYieldTime <= Time.time)
				onYield();
		}

		protected override void CalculateNextEulerAngles(ref Vector3 nextEulerAngles, float deltaTime)
		{
			base.CalculateNextEulerAngles(ref nextEulerAngles, deltaTime);
			nextEulerAngles.x = GetPitchFromInput(deltaTime);
		}

		protected float GetPitchFromInput(float deltaTime)
		{
			int direction = Model.Properties.IsInvertedPitch ? -1 : 1;
			var rotationAngle = LastCamInput.y * Model.Properties.InputTurnSpeed.y * direction * deltaTime;
			if (rotationAngle > 0 && DeltaPitch >= Model.Properties.MaximumPitchAngle
				|| rotationAngle < 0 && DeltaPitch <= Model.Properties.MinimumPitchAngle)
				return DeltaPitch;
			DeltaPitch += rotationAngle;
			return DeltaPitch;
		}
	}
}