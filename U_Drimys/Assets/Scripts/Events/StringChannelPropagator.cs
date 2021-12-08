using System;
using Events.Channels;
using Events.UnityEvents;
using UnityEngine;

namespace Events
{
	public class StringChannelPropagator : MonoBehaviour 
	{
		[SerializeField, Tooltip("Not Null")]
		private StringChannelSo channel;
		
		public StringUnityEvent onEvent;

		private void Awake()
		{
			try
			{
				channel.Subscribe(onEvent.Invoke);
			}
			catch (NullReferenceException)
			{
				throw new NullReferenceException($"No channel was provided for the propagation" +
												$" in the {gameObject.name} GameObject");
			}
		}
	}
}