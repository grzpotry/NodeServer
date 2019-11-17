using System;

namespace Networking.Framework.Dispatchers
{
    /// <summary>
    /// Provides hook to main unity thread
    /// </summary>
    public interface IUnityThreadListener
    {
        event Action Updated;
    }
}