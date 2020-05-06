using System;
using System.Collections.Generic;

namespace Networking.Framework.Utils
{
    /// <summary>
    /// Generic pool for temporary objects.
    /// </summary>
    public abstract class BasePool<T>
        where T : class
    {
        /// <summary>
        /// When disposed, releases stored object.
        /// </summary>
        public struct Scope : IDisposable
        {
            public T Value { get; private set; }

            public Scope(BasePool<T> pool, T value)
            {
                _pool = pool;
                Value = value;
            }

            public void Dispose()
            {
                if (Value != null)
                {
                    _pool.Release(Value);
                }
                Value = null;
                _pool = null;
            }

            private BasePool<T> _pool;
        }

        protected Scope CreateScope()
        {
            return new Scope(this, Pull());
        }

        protected abstract T CreateNew();

        protected abstract void Clear(T value);

        private readonly List<T> _pool = new List<T>();

        private T Pull()
        {
            lock (_pool)
            {
                if (_pool.Count == 0)
                {
                    return CreateNew();
                }

                var index = _pool.Count - 1;
                var list = _pool[index];
                _pool.RemoveAt(index);
                return list;
            }
        }

        private void Release(T value)
        {
            if (value == null)
            {
                throw new Exception("Null object released");
            }

            Clear(value);

            lock (_pool)
            {
                _pool.Add(value);
            }
        }
    }
}