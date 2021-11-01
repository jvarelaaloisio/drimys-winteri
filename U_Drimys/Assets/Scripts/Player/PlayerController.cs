using Characters;
using Core.Extensions;
using MVC;
using UnityEngine;

namespace Player
{
	public class PlayerController : BaseController
	{
		protected new readonly CharacterModel Model;
		private readonly Transform _mainCameraTransform;

		public PlayerController(CharacterModel model, IView view)
			: base(model, view)
		{
			Model = model;
			_mainCameraTransform = Camera.main.transform;
		}

		public void Jump()
		{
			Model.Jump();
		}

		public void Move(Vector2 input)
		{
			var processedDirection 
				= _mainCameraTransform.TransformDirection(input.HorizontalPlaneToVector3());
			var movement = new Vector2(processedDirection.x, processedDirection.z);
			Model.MoveTowards(movement);
		}

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