using System;
using UnityEngine;

namespace MVC
{
    [RequireComponent(typeof(BaseView))]
    public class MvcBasicSetup : MonoBehaviour
    {
        private BaseView _view;
        private BaseModel _model;
        private BaseController _controller;
        private void Start()
        {
            _view = GetComponent<BaseView>();
            _model = new BaseModel(_view);
            _controller = new BaseController(_model, _view);
        }
    }
}
