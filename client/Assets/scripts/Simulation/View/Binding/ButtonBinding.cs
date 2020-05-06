using System.Reflection;
using Simulation.Domain.Bindings;
using UnityEngine;
using UnityEngine.UI;

namespace Simulation.View.Binding
{
    /// <summary>
    /// Primitive binder for button's action
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ButtonBinding : MonoBehaviour
    {
        protected void Awake()
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(() =>
            {
                _targetMethod.Invoke(_target.Component, null);
            });
        }

        protected void Start()
        {
            _targetMethod = _target.GetMemberInfo();
        }

        [SerializeField]
        private BindableMethod _target = default;

        private MethodInfo _targetMethod;
        private Button _button;
    }
}