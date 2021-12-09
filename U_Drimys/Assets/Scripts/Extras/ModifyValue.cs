using Events.UnityEvents;
using UnityEngine;

namespace Extras
{
    public class ModifyValue : MonoBehaviour
    {
        public FloatUnityEvent onModify;

        [SerializeField]
        private int divider;

        public void DivideBy(float original) => onModify.Invoke(original / divider);
        public void DivideBy(int original) => DivideBy((float)original);
    }
}