using System;
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

		public PlayerController(CharacterModel model)
			: base(model)
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
			processedDirection.y = 0;
			processedDirection.Normalize();

			var movement = new Vector2(processedDirection.x, processedDirection.z);
			Model.MoveTowards(movement);
		}

		public void StartAim()
		{
			throw new NotImplementedException();
		}

		public void Shoot()
		{
			throw new NotImplementedException();
		}

		public void Melee()
		{
			throw new NotImplementedException();
		}

		public void Lock()
		{
			Model.TryLock("Enemy");
		}
	}
}