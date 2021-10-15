namespace MVC
{
	public class BaseModel
	{
		protected readonly IView View;
		public BaseModel(IView view) => View = view;
	}
}
