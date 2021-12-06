using Core.Helpers;
using Events.Channels;
using JetBrains.Annotations;
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

		[Space, Header("Channels Listened")]
		[SerializeField, Tooltip("Can Be Null")]
		private Vector2ChannelSo cameraMovement;

		[SerializeField, Tooltip("Can Be Null")]
		[CanBeNull]
		private Vector2ChannelSo playerMovement;

		[SerializeField, Tooltip("Not Null")]
		private TransformChannelSo playerStarts;

		private void Awake()
		{
			cameraMovement.SubscribeSafely(HandleCamInput);
			playerMovement.SubscribeSafely(HandleMoveInput);
			if (!target)
			{
				playerStarts.Subscribe(AssignTarget);
				enabled = false;
				return;
			}

			SetupModel();
		}

		private void SetupModel()
		{
			transform.SetPositionAndRotation(target.position, target.rotation);
			_model = new CameraModel(transform,
									properties,
									target,
									this,
									shouldLogFsmTransitions);
		}

		private void AssignTarget(Transform target)
		{
			this.target = target;
			enabled = true;
			SetupModel();
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
			=> _model.Lock();

		public void HandleUnlock()
			=> _model.Unlock();
	}
}