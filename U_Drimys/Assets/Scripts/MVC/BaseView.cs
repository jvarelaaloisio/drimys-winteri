using UnityEngine;

namespace MVC
{
	[RequireComponent(typeof(Rigidbody))]
	public class BaseView : MonoBehaviour
	{
		public BaseController Controller;
	}
}
