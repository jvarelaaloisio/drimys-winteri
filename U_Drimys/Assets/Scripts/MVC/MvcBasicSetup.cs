using System;
using UnityEngine;

namespace MVC
{
    [RequireComponent(typeof(IView))]
    public class MvcBasicSetup : MonoBehaviour
    {
        private IView _view;
        private BaseModel _model;
        private BaseController _controller;
        private void Start()
        {
            _view = GetComponent<IView>();
            _model = new BaseModel(_view);
            _controller = new BaseController(_model, _view);
        }
    }
}
