using System.Collections;
using Core.Helpers;
using UnityEngine;

namespace Camera.States
{
	public class Lock<T> : CameraState<T>
	{
		private Quaternion _originRotation;
		private bool _isResetting;
		private readonly Transform _transform;
		private readonly ICoroutineRunner _coroutineRunner;
		private readonly Transform _modelTarget;

		public Lock(CameraModel model,
					ICoroutineRunner coroutineRunner)
			: base(model)
		{
			_coroutineRunner = coroutineRunner;
			_transform = model.transform;
			_modelTarget = Model.Target;
		}

		public override void Awake()
		{
			_originRotation = _transform.rotation;
			_isResetting = true;
			_coroutineRunner.StartCoroutine(ResetRotation());
			base.Awake();
		}

		public override string GetName() => "Lock";

		public override void Update(float deltaTime)
		{
			if (!_isResetting)
				_transform.rotation = _modelTarget.rotation;

			if (Model.Target.hasChanged)
				_transform.position
					= Model.Target.position + _transform.TransformDirection(Model.GetPositioning());
			base.Update(deltaTime);
		}

		public override void Sleep()
		{
			_coroutineRunner.StopCoroutine(ResetRotation());
			base.Sleep();
		}

		private IEnumerator ResetRotation()
		{
			const float iterationPeriod = 1.0f / 45;
			float lockDuration = Model.Properties.LockDuration;
			var waitIteration = new WaitForSeconds(iterationPeriod);
			for (float i = 0; i < Model.Properties.LockDuration; i += iterationPeriod)
			{
				_transform.rotation = Quaternion.Slerp(_originRotation, _modelTarget.rotation, i / lockDuration);
				yield return waitIteration;
			}

			_isResetting = false;
		}
	}
}