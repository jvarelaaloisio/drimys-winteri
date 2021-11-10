using Characters;
using IA.DecisionTree;

namespace Enemies.IA.Actions
{
	public class GoIdle : TreeAction
	{
		public override void NodeFunction()
			=> callback(CharacterModel.IDLE_STATE);
	}
}
