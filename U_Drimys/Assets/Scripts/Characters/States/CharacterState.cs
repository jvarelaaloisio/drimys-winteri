using IA.FSM;
using UnityEngine;

namespace Characters.States
{
	public abstract class CharacterState<T> : State<T>
	{
		protected readonly CharacterModel Model;

		public CharacterState(CharacterModel model)
		{
			Model = model;
		}

		public abstract void HandleMoveInput(Vector2 direction);
	}
}