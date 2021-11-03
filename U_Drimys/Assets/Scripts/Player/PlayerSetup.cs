using Characters;
using MVC;
using UnityEngine;

namespace Player
{
    //TODO:This should be done by the player view (when it's a base class)
    [RequireComponent(typeof(IView))]
    public class PlayerSetup : MonoBehaviour
    {
        [SerializeField]
        private GameplayInputHandler inputHandler;
        private IView _view;
        private PlayerController _controller;
        private void Start()
        {
            _view = GetComponent<IView>();
            _view.Setup(_controller);
            _controller = new PlayerController((CharacterModel) _view.Model);
        }
    }
}
