using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SimpleGPUInstancingExample : MonoBehaviour
{
	public IEnumerable<MeshRenderer> leaves;
	public Material InstancedMaterial;

	void Awake()
	{
		leaves = (from renderer in GameObject.FindGameObjectsWithTag("smallLeaf").ToList() select renderer.GetComponent<MeshRenderer>());

		float range = 4f;

		for ( int i = 0; i < 1000; i++ )
		{
			//Transform newInstance = Instantiate( Prefab, new Vector3( Random.Range( -range, range ), range + Random.Range( -range, range ), Random.Range( -range, range ) ), Quaternion.identity ) as Transform;
			//MaterialPropertyBlock matpropertyBlock = new MaterialPropertyBlock();
			//Color newColor = new Color( Random.Range( 0.0f, 1.0f ), Random.Range( 0.0f, 1.0f ), Random.Range( 0.0f, 1.0f ) );
			//matpropertyBlock.SetColor( "_Color", newColor );
			//newInstance.GetComponent<MeshRenderer>().SetPropertyBlock( matpropertyBlock );
		} 
	}
}
