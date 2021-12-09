using System;
using UnityEngine;

namespace Camera
{
	[RequireComponent(typeof(BoxCollider))]
	public class CameraPositioningVolume : MonoBehaviour
	{
		[SerializeField]
		private CameraPositioning positioning;

		[SerializeField]
		private float transitionDuration = 1;
		
		[SerializeField]
		private AnimationCurve transitionCurve = AnimationCurve.Constant(0, 1, 1);

		private CameraPositioning _oldPositioning;
		private BoxCollider _boxCollider;

		private void OnValidate()
		{
			if(!positioning)
				Debug.LogError("No positioning was assigned", this);
			if(!_boxCollider)
				_boxCollider = GetComponent<BoxCollider>();
			_boxCollider.isTrigger = true;
		}

		private void OnTriggerEnter(Collider other)
		{
			if(!UnityEngine.Camera.main.TryGetComponent(out CameraView view))
				return;
			_oldPositioning = view.Positioning;
			view.ChangePositioning(positioning, transitionDuration, transitionCurve);
			Debug.Log($"Changed positioning on {view.name} from {_oldPositioning} to {positioning.name}", this);
		}

		private void OnTriggerExit(Collider other)
		{
			Transform otherTransform = other.transform;
			if(!UnityEngine.Camera.main.TryGetComponent(out CameraView view))
				return;
			view.ChangePositioning(_oldPositioning, transitionDuration, transitionCurve);
			Debug.Log($"Changed positioning on {view.name} from {positioning.name} to {_oldPositioning}", this);
		}
		
		private void OnDrawGizmos()
		{
			Gizmos.color = new Color(.5f, 1, .5f, .5f);
			Vector3 center = transform.position + _boxCollider.center;
			Vector3 size = Vector3.Scale(_boxCollider.size, transform.lossyScale);
			Gizmos.DrawWireCube(center, size);
		}
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(.5f, 1, .5f, .5f);
			Vector3 center = transform.position + _boxCollider.center;
			Vector3 size = Vector3.Scale(_boxCollider.size, transform.lossyScale);
			Gizmos.DrawCube(center, size);
		}
	}
}
