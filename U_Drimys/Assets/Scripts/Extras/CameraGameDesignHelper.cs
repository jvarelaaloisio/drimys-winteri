using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraGameDesignHelper : MonoBehaviour
{
	[SerializeField]
	private bool controlPosition;

	[SerializeField]
	private float yOffset;

	
	
	private void Update()
	{
		var transformPosition = transform.position;
		if (controlPosition
			&& transform.hasChanged
			&& Physics.Raycast(transformPosition,
													Vector3.down,
													out RaycastHit hit,
													10))
		{
			transform.position = new Vector3(transformPosition.x,
											hit.point.y + yOffset,
											transformPosition.z);
		}
	}
}