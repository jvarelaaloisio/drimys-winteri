using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayInputHandler : MonoBehaviour
{
	[SerializeField]
	private InputActionAsset input;

	[SerializeField]
	private string actionMapName;

	[SerializeField]
	private string jumpAction;

	//TODO:Create character controller

	private void Awake()
	{
		var actionMap = input.FindActionMap(actionMapName);
		actionMap.FindAction(jumpAction).performed += HandleJump;
	}

	//TODO:Create controller setup
	// public void SetupController()

	private void HandleJump(InputAction.CallbackContext context)
	{
		//TODO:Call Jump in controller
	}
}