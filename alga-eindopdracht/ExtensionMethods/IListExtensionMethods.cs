using System.Collections.Generic;

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
    }
}
