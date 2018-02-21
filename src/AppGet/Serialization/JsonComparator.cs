using System.Collections.Generic;

namespace AppGet.Serialization
{
    public class JsonComparator<T> : IEqualityComparer<T>
    {
        public static readonly IEqualityComparer<T> Comparator = new JsonComparator<T>();

        public bool Equals(T x, T y)
        {
            if (object.Equals(x, y))
            {
                return true;
            }

            return Json.Serialize(x) == Json.Serialize(y);
        }

        public int GetHashCode(T obj)
        {
            return Json.Serialize(obj).GetHashCode();
        }
    }
}