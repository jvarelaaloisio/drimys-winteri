using Characters;
using UnityEngine;

namespace Enemies
{
	[CreateAssetMenu(menuName = "Properties/Enemy", fileName = "EnemyProperties", order = 2)]
	public class EnemyProperties : ThrowerProperties
	{
		[SerializeField]
		private float viewDistance;

		[SerializeField]
		private float attackDistance;

		[SerializeField]
		private float minThrowDistance;

		[SerializeField]
		private float maxThrowDistance;

		[SerializeField]
		private float throwPeriod;

		[SerializeField]
		private float fieldOfView;

		[SerializeField]
		private LayerMask wallLayer;

		[SerializeField]
		private float meleeDamage;

		public float ViewDistance => viewDistance;
		public float AttackDistance => attackDistance;

		public float MinThrowDistance => minThrowDistance;

		public float MaxThrowDistance => maxThrowDistance;

		public float FieldOfView => fieldOfView;

		public float ThrowPeriod => throwPeriod;

		public LayerMask WallLayer => wallLayer;
		
		public float MeleeDamage => meleeDamage;
	}
}