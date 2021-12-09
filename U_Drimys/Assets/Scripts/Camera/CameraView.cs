using Core.Helpers;
using Events.Channels;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Camera
{
	public class CameraView : MonoBehaviour, ICoroutineRunner
	{
		[SerializeField]
		private CameraProperties properties;
		
		[SerializeField]
		private CameraPositioning positioning;

		[SerializeField]
		private Transform target;

		private CameraModel _model;

		[SerializeField]
		private int closeUpFOV;

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

		private float _originalFOV;

		public CameraPositioning Positioning => positioning;
		// private UnityEngine.Camera _cameraComponent;

		private void Awake()
		{
			// _cameraComponent = GetComponent<UnityEngine.Camera>();
			// _originalFOV = _cameraComponent.fieldOfView;
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
									Positioning,
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
			//TODO:The camera must be a child of this object and it should move closer to the player
			// _cameraComponent.fieldOfView = Mathf.Lerp(_originalFOV, closeUpFOV,
			// 										Mathf.Abs(transform.rotation.eulerAngles.x) /
			// 										properties.MaximumPitchAngle);
		}

		public void HandleMoveInput(Vector2 input)
			=> _model.HandleMoveInput(input);

		public void HandleCamInput(Vector2 input)
			=> _model.HandleCamInput(input);

		public void HandleLock()
			=> _model.Lock();

		public void HandleUnlock()
			=> _model.Unlock();

		public void ChangePositioning(CameraPositioning newPositioning,
									float duration,
									AnimationCurve curve)
			=> _model.ChangePositioning(newPositioning, duration, curve.Evaluate);
	}
}