using IA.FSM;
using UnityEngine;

namespace Camera.States
{
	public abstract class CameraState<T> : State<T>
	{
		public Vector2 LastMoveInput { protected get; set; }
		public Vector2 LastCamInput { protected get; set; }
		protected CameraModel Model;

		public CameraState(CameraModel model)
		{
			Model = model;
		}
	}
}