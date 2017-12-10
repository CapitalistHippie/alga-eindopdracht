using System;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.ExtensionMethods
{
    public static class IListExtensionMethods
    {
        public static void AddIfNotNull<T>(this IList<T> list, T entry)
        {
            if (entry != null)
            {
                list.Add(entry);
            }
        }

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
