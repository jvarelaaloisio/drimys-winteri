using System;
using IA.FSM;
using UnityEngine;

namespace Characters.States
{
	public abstract class CharacterState<T> : State<T>
	{
		protected readonly CharacterModel Model;
		public Action<Vector3> onMove;

		public CharacterState(CharacterModel model)
		{
			Model = model;
		}

		public abstract void MoveTowards(Vector2 direction);
	}
}