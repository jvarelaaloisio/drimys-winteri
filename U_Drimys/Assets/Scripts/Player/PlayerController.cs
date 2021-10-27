using Characters;
using MVC;
using UnityEngine;

namespace Player
{
	public class PlayerController : BaseController
	{
		protected new CharacterModel Model;
		public PlayerController(CharacterModel model, IView view)
			: base(model, view)
		{
		}

		public void Jump()
		{
			Model.Jump();
		}

		public void Move(Vector2 input) => Model.Move(input);

		public void StartAim()
		{
		}

		public void Shoot()
		{
		}

		public void Melee()
		{
		}
	}
}