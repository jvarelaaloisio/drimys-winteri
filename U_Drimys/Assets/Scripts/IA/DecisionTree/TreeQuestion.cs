using IA.DecisionTree.Helpers;

namespace IA.DecisionTree
{
	[SkippedInEditor]
	public abstract class TreeQuestion : TreeNode
	{
		public TreeNode[] Outcomes { get; set; }

		public virtual int GetOutcomesQty() { return 2; }
		public override void NodeFunction()
		{
			int outcomeIndex = CheckCondition();
			if (outcomeIndex >= Outcomes.Length) return;
			ChangeNode?.Invoke(Outcomes[outcomeIndex]);
		}

		public abstract int CheckCondition();
	}
}