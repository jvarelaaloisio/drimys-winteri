using IA.FSM;

namespace MVC
{
	public class BaseModel
	{
		public BaseModel(IView view)
		{
			View = view;
			//TODO:Arreglar esto
		}

		public IView View { get; }
	}
}
