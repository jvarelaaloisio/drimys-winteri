﻿using UnityEngine;

namespace IA.DecisionTree.Helpers
{
	public struct NodeData
	{
		public NodeKind Kind;
		public string ClassType;
		public string[] OutcomeClassNames;
		public Vector2 PositionInEditor;
	}
}