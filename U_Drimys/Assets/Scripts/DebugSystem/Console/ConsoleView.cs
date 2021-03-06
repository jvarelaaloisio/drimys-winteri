using System.Collections.Generic;
using DS.DebugConsole;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Plugins.DebugSystem.Console
{
	public class ConsoleView : MonoBehaviour
	{
		[SerializeField] private Text consoleBody;
		[SerializeField] private InputField inputField;
		[SerializeField] private ConsoleControllerSO consoleController;
		private IDebugConsole<string> _debugConsole;
		private List<ICommand<string>> _commands;
		public void ReadInput(string input)
		{
			if(!Input.GetButtonDown("Submit"))
				return;
			_ = consoleController.TryUseInput(input);
		}

		private void Start()
		{
			consoleController.onFeedback += WriteFeedback;
		}

		private void WriteFeedback(string newFeedBack)
		{
			consoleBody.text += "\n" + newFeedBack;
			inputField.ActivateInputField();
		}
	}
}