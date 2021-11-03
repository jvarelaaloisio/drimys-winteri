﻿using Core.Helpers;
using IA.FSM;
using UnityEngine;

namespace MVC
{
	public class BaseModel
	{
		/// <summary>
		/// Build in the constructor
		/// </summary>
		protected FSM<string> StateMachine;
		protected readonly ICoroutineRunner CoroutineRunner;

		public BaseModel(Transform transform,
						ICoroutineRunner coroutineRunner)
		{
			this.transform = transform;
			CoroutineRunner = coroutineRunner;
		}
		public Transform transform { get; }
	}
}
