using System.Reflection;
using Simulation.Domain.Bindings;
using UnityEngine;

namespace Simulation.View.Binding
{
    /// <summary>
    /// Very primitive binding between source and target <see cref="BindableProperty"/>s just for prototype - only purposes"/>
    /// In real use-case bindings should be event - based instead of naive value pulling
    /// </summary>
    public class ComponentPropertyBinding : MonoBehaviour
    {
        protected void Start()
        {
            _sourceProp = _source.GetMemberInfo();
            _targetProp = _target.GetMemberInfo();
        }

        protected void Update()
        {
            _targetProp.SetValue(_target.Component, _source.GetValue());
        }

        [SerializeField]
        private BindableProperty _source = default;

        [SerializeField]
        private BindableProperty _target = default;

        private PropertyInfo _sourceProp;
        private PropertyInfo _targetProp;
    }
}