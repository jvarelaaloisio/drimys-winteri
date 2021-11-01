using Characters;
using Input;
using MVC;
using UnityEngine;

namespace Player
{
    //TODO:This should be done by the player view (when it's a base class)
    [RequireComponent(typeof(IView))]
    [RequireComponent(typeof(GameplayInputHandler))]
    public class PlayerSetup : MonoBehaviour
    {
        private IView _view;
        private PlayerController _controller;
        private GameplayInputHandler _inputHandler;
        private void Start()
        {
            _view = GetComponent<IView>();
            _view.Setup(_controller);
            _controller = new PlayerController((CharacterModel) _view.Model, _view);
            _inputHandler = GetComponent<GameplayInputHandler>();
            _inputHandler.SetupController(_controller);
        }
    }
}
