using Characters;
using UnityEngine;

namespace Enemies
{
	[CreateAssetMenu(menuName = "Properties/Enemy", fileName = "EnemyProperties", order = 2)]
	public class EnemyProperties : CharacterProperties
	{
		public float viewDistance;
		public float fieldOfView;
		public LayerMask wallLayer;

		public float ViewDistance => viewDistance;

		public float FieldOfView => fieldOfView;

		public LayerMask WallLayer => wallLayer;
	}
}
