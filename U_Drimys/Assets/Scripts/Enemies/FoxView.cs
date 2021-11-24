using System.Collections.Generic;
using Characters;
using Core.DataManipulation;
using IA.DecisionTree.Helpers;
using UnityEngine;
using Tree = IA.DecisionTree.Tree;

namespace Enemies
{
	public class FoxView : ThrowerView
	{
		[SerializeField]
		private TextAsset decisionTree;

		protected new EnemyModel Model;
		protected EnemyProperties EnemyProperties;
		private Tree _tree;

		[SerializeField]
		[Range(-1, 2)]
		private float debugOffset = .95f;

		protected override void Awake()
		{
			Caster.TryCast(characterProperties, out  EnemyProperties);
			base.Awake();
			Model = (EnemyModel)base.Model;
			_tree = TreeHelper.LoadTree(decisionTree, GetModel);
			_tree.Callback = HandleTreeCallback;
		}
		
		private void HandleTreeCallback(object[] args)
		{
			Model.ForceTransition((string)args[0]);
		}

		protected override CharacterModel BuildModel()
		{
			return new EnemyModel(transform,
								Rigidbody,
								EnemyProperties,
								throwablePrefab,
								hand,
								this,
								shouldLogTransitions);
		}

		private EnemyModel GetModel() => Model;

		protected override void Update()
		{
			_tree.RunTree();
			base.Update();
		}

		protected override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			Transform myTransform = transform;
			Vector3 position = myTransform.position + Vector3.down * debugOffset;

			#region LineOfSight

			Caster.TryCast(characterProperties, out  EnemyProperties);

			Vector3 forwardScaled = myTransform.forward * EnemyProperties.ViewDistance;
			int points = 5;

			Mesh viewMesh = new Mesh();

			List<Vector3> vertices = new List<Vector3> { position };
			for (int i = -points; i <= points; i++)
			{
				vertices.Add(position
							+ Quaternion.AngleAxis(EnemyProperties.FieldOfView / (points * 2) * i, Vector3.up)
							* forwardScaled);
			}

			viewMesh.SetVertices(vertices);

			int[] triangles = new int[3 * 2 * points];
			int firstValue = 1;
			int secondValue = 2;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				triangles[i] = 0;
				triangles[i + 1] = firstValue++;
				triangles[i + 2] = secondValue++;
			}

			viewMesh.triangles = triangles;

			viewMesh.RecalculateNormals();
			Gizmos.color = new Color(0, 1, .75f, .5f);
			Gizmos.DrawMesh(viewMesh);

			#endregion
		}
	}
}