namespace MVC
{
	public class BaseController
	{
		protected readonly BaseModel Model;
		public BaseController(BaseModel model)
			=> Model = model;
	}
}
