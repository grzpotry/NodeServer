using Networking.Framework.Dispatchers;

namespace Networking.Framework
{
    /// <summary>
    /// Peer which handles server messages always in main unity thread
    /// </summary>
    public abstract class ThreadSafePeer : BasePeer
    {
        protected ThreadSafePeer(IUnityThreadListener unityListener, UserData userData)
            : base(new UnityDispatcher(unityListener), userData)
        {
        }
    }
}