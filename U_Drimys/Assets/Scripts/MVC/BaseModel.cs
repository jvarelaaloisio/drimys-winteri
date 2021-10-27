using IA.FSM;

namespace MVC
{
	public class BaseModel
	{
		public BaseModel(IView view)
		{
			View = view;
			StateMachine = FSM<string>.Builder.BuildAnFSM(view.Transform.name).Build();
		}

		protected readonly IView View;
		protected readonly FSM<string> StateMachine;

		public void Update(float deltaTime)
		{
			StateMachine.Update(deltaTime);
		}
	}
}
