using System;
using System.Reflection;

namespace Simulation.Domain.Bindings
{
    [Serializable]
    public class BindableProperty : BindableComponentMember<PropertyInfo>
    {
        public object GetValue()
        {
            _propertyInfo = _propertyInfo ?? GetMemberInfo();
            return _propertyInfo.GetValue(Component);
        }

        private PropertyInfo _propertyInfo;
    }
}