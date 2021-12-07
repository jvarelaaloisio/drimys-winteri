using System;
using System.Collections;
using Core.Extensions;
using UnityEditor;
using UnityEngine;

namespace Extras
{
	public class Spawner : MonoBehaviour
	{
		[SerializeField]
		private GameObject prefab;

		[SerializeField]
		private Transform spawnPoint;

		[SerializeField]
		private float spawnPeriod;

		[SerializeField]
		private float activeRadius;

		[SerializeField]
		private bool radiusIs2d;

		[SerializeField]
		private string playerTag = "Player";

		private Transform _player;
		private bool _isSpawning;

		private void Awake()
		{
			_player = GameObject.FindGameObjectWithTag(playerTag).transform;
		}

		private void OnEnable()
		{
			bool ShouldSpawn() => (radiusIs2d &&
									Vector3.Distance(transform.position.XZtoXY(), _player.position.XZtoXY())
									< activeRadius)
								|| Vector3.Distance(transform.position, _player.position) < activeRadius;

			StartCoroutine(Spawn(prefab,
								spawnPoint.position,
								spawnPoint.rotation,
								spawnPeriod,
								ShouldSpawn));
		}

		private void OnDisable()
		{
			StopCoroutine(nameof(Spawn));
		}

		private static IEnumerator Spawn(GameObject prefab,
										Vector3 position,
										Quaternion rotation,
										float period,
										Func<bool> shouldSpawn)
		{
			WaitForSeconds waitForPeriod = new WaitForSeconds(period);
			while (true)
			{
				if (shouldSpawn())
					Instantiate(prefab, position, rotation);
				yield return waitForPeriod;
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = new Color(1, .5f, .5f);
			Gizmos.DrawSphere(transform.position, .25f);
			Gizmos.DrawWireSphere(transform.position, activeRadius);
		}
	}
}