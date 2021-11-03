using System;
using System.Collections;
using UnityEngine;

namespace MVC
{
	[Obsolete("This Interface should be removed once the model stops using it")]
	public interface IView
	{
		Transform Transform { get; }
		Rigidbody Rigidbody { get; }
		BaseModel Model { get; }
		void Setup(BaseController controller);
	}
}