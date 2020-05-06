using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Simulation.Domain.Bindings
{
    /// <summary>
    /// Wrapper for <see cref="Component"/> member binding
    /// </summary>
    [Serializable]
    public abstract class BindableComponentMember<T>
        where T : MemberInfo
    {
        public Component Component => _component;
        public string MemberName => _memberName;

        public T GetMemberInfo()
        {
            var member = Component.GetType().GetMember(MemberName).OfType<T>().SingleOrDefault();

            if (member == null)
            {
                Debug.LogError($"Member {MemberName} not found on component", _component);
            }

            return member;
        }

        [SerializeField]
        private Component _component = default;

        [SerializeField]
        private string _memberName = default;
    }
}