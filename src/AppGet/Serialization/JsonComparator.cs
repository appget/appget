using System.Collections.Generic;

namespace AppGet.Serialization
{
    public class JsonComparator<T> : IEqualityComparer<T>
    {
        public static readonly IEqualityComparer<T> Comparator = new JsonComparator<T>();

        public bool Equals(T x, T y)
        {
            return JsonEquality.Equal(x, y);
        }

        public int GetHashCode(T obj)
        {
            return Json.Serialize(obj).GetHashCode();
        }
    }
}