using System.Collections.Generic;

namespace AppGet.Manifest.Serialization
{
    public class JsonComparator<T> : IEqualityComparer<T>
    {
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