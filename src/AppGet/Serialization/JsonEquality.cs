namespace AppGet.Serialization
{
    public static class JsonEquality
    {
        public static bool Equal(object obj1, object obj2)
        {
            if (Equals(obj1, obj2))
            {
                return true;
            }

            return Json.Serialize(obj1) == Json.Serialize(obj2);
        }
    }
}