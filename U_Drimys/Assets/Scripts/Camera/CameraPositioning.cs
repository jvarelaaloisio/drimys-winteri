using UnityEngine;

namespace Camera
{
	[CreateAssetMenu(menuName = "Properties/Camera/Positioning", fileName = "CameraPositioning", order = 1)]
	public class CameraPositioning : ScriptableObject
	{
		[SerializeField]
		private Vector3 offsetFromTarget;

		public Vector3 OffsetFromTarget => offsetFromTarget;
	}
}