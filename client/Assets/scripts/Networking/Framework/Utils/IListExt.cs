using System;
using System.Collections.Generic;

namespace Networking.Framework.Utils
{
    /// <summary>
    /// Extensions for <see cref="IList{T}"/>
    /// </summary>
    public static class IListExt
    {
        public static void ReplaceWith<T, TSource>(this IList<T> self, IEnumerable<TSource> source, Func<TSource, T> create, Action<T> onRemoved = null)
        {
            var index = 0;
            foreach (var item in source)
            {
                if (self.Count > index)
                {
                    self[index] = create(item);
                }
                else
                {
                    self.Add(create(item));
                }

                index += 1;
            }
            self.TrimAboveCount(index);
        }

        public static void TrimAboveCount<T>(this IList<T> list, int count, Action<T> onRemoved = null)
        {
            while (list.Count > count)
            {
                var obj = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                onRemoved?.Invoke(obj);
            }
        }
    }
}