using System;
using System.Reflection;
using Simulation.Domain.Bindings;
using UnityEngine;

namespace Simulation.View.Binding
{
    /// <summary>
    /// Set's active flag of self gameObject based on bounded bool property
    /// </summary>
    public class ToggleGameObject : MonoBehaviour
    {
        protected void Start()
        {
            _sourceProp = _source.GetMemberInfo();

            if ((_sourceProp.PropertyType != typeof(bool)))
            {
                throw new NotSupportedException($"Unexpected type of bound property: {_sourceProp.DeclaringType}, expected: {typeof(bool)}");
            }
        }

        protected void Update()
        {
            var value = (bool)_source.GetValue();
            value = _inverse ? !value : value;
            gameObject.SetActive(value);
        }

        [SerializeField]
        private BindableProperty _source = default;

        [SerializeField]
        private bool _inverse = false;

        private PropertyInfo _sourceProp;
    }
}