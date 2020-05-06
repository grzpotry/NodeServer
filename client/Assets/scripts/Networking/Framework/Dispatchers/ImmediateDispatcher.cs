using System;

namespace Networking.Framework.Dispatchers
{
    /// <summary>
    /// Invokes callback immediately
    /// </summary>
    public class ImmediateDispatcher : IDispatcher
    {
        public void Invoke(Action callback)
        {
            callback();
        }
    }
}