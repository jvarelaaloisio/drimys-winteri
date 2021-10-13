namespace MVC
{
	public class BaseModel
	{
		private readonly BaseView _view;
		public BaseModel(BaseView view) => _view = view;
	}
}
