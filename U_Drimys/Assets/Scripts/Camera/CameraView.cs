using Core.Helpers;
using Player;
using UnityEngine;

namespace Camera
{
	public class CameraView : MonoBehaviour, ICoroutineRunner
	{
		[SerializeField]
		private GameplayInputHandler inputHandler;
		
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
			if (!inputHandler)
				inputHandler = FindObjectOfType<GameplayInputHandler>();
			inputHandler.onCameraInput.AddListener(HandleCamInput);
			inputHandler.onMoveInput.AddListener(HandleMoveInput);
			if (!target)
				target = GameObject.FindGameObjectWithTag("Player").transform;
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
