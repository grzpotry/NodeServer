using System;
using System.Reflection;

namespace Simulation.Domain.Bindings
{
    [Serializable]
    public class BindableMethod : BindableComponentMember<MethodInfo>
    {
    }
}