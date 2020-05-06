using System.Collections.Generic;

namespace Networking.Framework.Utils
{
    /// <summary>
    /// Pool for generic list
    /// </summary>
    public class ListPool<T> : BasePool<List<T>>
    {
        public static Scope Create()
        {
            return _pool.CreateScope();
        }

        protected override List<T> CreateNew() => new List<T>();

        protected override void Clear(List<T> value)
        {
            value.Clear();
        }

        private static readonly ListPool<T> _pool = new ListPool<T>();
    }
}