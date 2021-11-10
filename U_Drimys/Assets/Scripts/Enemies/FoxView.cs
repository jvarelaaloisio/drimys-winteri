using System;
using System.Collections.Generic;
using Characters;
using IA.DecisionTree.Helpers;
using UnityEngine;
using Tree = IA.DecisionTree.Tree;

namespace Enemies
{
	public class FoxView : CharacterView
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
			//TODO:Make Static method
			TryCastProperties(out var properties);
			base.Awake();
			Model = (EnemyModel)base.Model;
			_tree = TreeHelper.LoadTree(decisionTree, GetModel);
			_tree.Callback = HandleTreeCallback;
		}

		private void TryCastProperties(out EnemyProperties properties)
		{
			properties = characterProperties as EnemyProperties;
			EnemyProperties = properties
								? properties
								: throw new ArgumentException("Properties field in a thrower class" +
															" should be of type ThrowerProperties");
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
								this,
								shouldLogTransitions);
		}

		private EnemyModel GetModel() => Model;

		protected override void Update()
		{
			_tree.RunTree();
			base.Update();
		}
		
		private void OnDrawGizmos()
		{
			Transform myTransform = transform;
			Vector3 position = myTransform.position + Vector3.down * debugOffset;

			#region LineOfSight

			TryCastProperties(out var properties);

			Vector3 forwardScaled = myTransform.forward * properties.ViewDistance;
			int points = 5;

			Mesh viewMesh = new Mesh();

			List<Vector3> vertices = new List<Vector3> { position };
			for (int i = -points; i <= points; i++)
			{
				vertices.Add(position
							+ Quaternion.AngleAxis(properties.FieldOfView / (points * 2) * i, Vector3.up)
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