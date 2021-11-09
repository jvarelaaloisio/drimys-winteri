using System.Collections;
using Characters;
using Core.Extensions;
using Core.Interactions.Throwables;
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
			//BUG:Character walks in an angle instead of forward bc it's taking the camera forward directly.
			//Maybe there could be a validation with the dot product between the char's forward and the camera's forward
			var processedDirection = _mainCameraTransform.TransformDirection(input.HorizontalPlaneToVector3());
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
			Model.ReleaseAimAndThrow();
		}

		public void Melee(float meleeDuration, float effectSpawnDelay, Throwable meleeEffect,
						Transform effectSpawnPoint)
		{
			Model.ThrowAttack(null,
							effectSpawnPoint,
							effectSpawnDelay,
							meleeDuration,
							meleeEffect);
		}

		public void Lock()
		{
			Model.TryLock("Enemy");
		}

		private IEnumerator Attack(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
		}
	}
}