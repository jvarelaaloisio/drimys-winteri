using IA.DecisionTree.Helpers;

namespace IA.DecisionTree
{
	[SkippedInEditor]
	public class TreeStart : TreeNode
	{
		public readonly TreeNode FirstNode;
		public TreeStart(TreeNode firstNode)
		{
			FirstNode = firstNode;
		}
		public override void NodeFunction()
		{
			if(FirstNode != null)ChangeNode(FirstNode);
		}
	}
}