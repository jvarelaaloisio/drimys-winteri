namespace MVC
{
	public class BaseController
	{
		protected readonly BaseModel Model;
		protected readonly IView View;
		public BaseController(BaseModel model, IView view)
		{
			Model = model;
			View = view;
		}
	}
}
