using System;
using Core.Helpers;
using IA.FSM;
using UnityEngine;

namespace Characters.States
{
	public abstract class CharacterState<T> : State<T>
	{
		protected readonly CharacterModel Model;
		protected readonly ICoroutineRunner CoroutineRunner;
		public Action<Vector3> onMove;

		public CharacterState(CharacterModel model,
							ICoroutineRunner coroutineRunner)
		{
			Model = model;
			CoroutineRunner = coroutineRunner;
		}

		public abstract void MoveTowards(Vector2 direction);
	}
}