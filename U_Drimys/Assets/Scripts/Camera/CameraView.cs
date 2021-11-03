using Core.Helpers;
using UnityEngine;

namespace Camera
{
	public class CameraView : MonoBehaviour, ICoroutineRunner
	{
		[SerializeField]
		private CameraProperties properties;

		[SerializeField]
		private Transform target;
		
		private CameraModel _model;
		
		[Header("Debug")]
		[SerializeField]
		private bool shouldLogFsmTransitions;

		private void Awake()
		{
			_model = new CameraModel(transform,
									properties,
									target,
									this,
									shouldLogFsmTransitions);
		}

		private void LateUpdate()
		{
			_model.LateUpdate(Time.deltaTime);
		}

		public void HandleMoveInput(Vector2 input)
			=> _model.HandleMoveInput(input);

		public void HandleCamInput(Vector2 input)
			=> _model.HandleCamInput(input);

		public void HandleLock()
		{
			_model.Lock();
		}
		
		public void HandleUnlock()
		{
			_model.Unlock();
		}
	}
}
