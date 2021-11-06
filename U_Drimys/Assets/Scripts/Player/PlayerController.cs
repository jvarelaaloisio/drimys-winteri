using System;
using Characters;
using Core.Extensions;
using MVC;
using UnityEngine;

namespace Player
{
	public class PlayerController : BaseController
	{
		protected new readonly ThrowerModel Model;
		private readonly Transform _mainCameraTransform;

		public PlayerController(ThrowerModel model)
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
			Model.Aim(Model.LockTargetTransform);
		}

		public void Shoot()
		{
			Model.Throw();
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