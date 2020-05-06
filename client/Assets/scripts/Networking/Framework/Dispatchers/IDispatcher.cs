using System;

namespace Networking.Framework.Dispatchers
{
    /// <summary>
    /// Dispatches callbacks
    /// </summary>
    public interface IDispatcher
    {
        void Invoke(Action callback);
    }
}