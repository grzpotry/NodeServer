using System;
using System.Collections.Concurrent;

namespace Networking.Framework.Dispatchers
{
    /// <summary>
    /// Dispatches callbacks in unity main thread
    /// </summary>
    public class UnityDispatcher : IDispatcher, IDisposable
    {
        public UnityDispatcher(IUnityThreadListener listener)
        {
            _unityListener = listener ?? throw new ArgumentNullException(nameof(listener));
            _unityListener.Updated += OnUnityUpdate;
        }

        public void Dispose()
        {
            _unityListener.Updated -= OnUnityUpdate;
        }

        public void Invoke(Action callback)
        {
            _callbacksQueue.Enqueue(callback);
        }

        private readonly IUnityThreadListener _unityListener;
        private readonly ConcurrentQueue<Action> _callbacksQueue = new ConcurrentQueue<Action>();

        private void OnUnityUpdate()
        {
            Flush();
        }

        private void Flush()
        {
            while (_callbacksQueue.TryDequeue(out var callback))
            {
                callback.Invoke();
            }
        }
    }
}