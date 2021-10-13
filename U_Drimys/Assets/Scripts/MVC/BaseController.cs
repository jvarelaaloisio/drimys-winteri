namespace MVC
{
	public class BaseController
	{
		private readonly BaseModel _model;
		private readonly BaseView _view;
		public BaseController(BaseModel model, BaseView view)
		{
			_model = model;
			_view = view;
		}
	}
}
