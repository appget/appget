using System;
using System.Collections.Generic;
using System.Linq;
using AppGet.Manifest.Serialization;

namespace AppGet.Installers
{
    public class DiffGenerator<T>
    {
        private readonly Dictionary<string, T> _before;
        private readonly Dictionary<string, T> _after;
        public DiffGenerator(IEnumerable<T> before, IEnumerable<T> after, Func<T, string> keyFunction)
        {
            var comparator = new JsonComparator<T>();
            _before = before.Except(after, comparator).ToDictionary(keyFunction, c => c);
            _after = after.Except(before, comparator).ToDictionary(keyFunction, c => c);
        }

        public IEnumerable<T> Added()
        {
            return _after.Where(c => !_before.ContainsKey(c.Key)).Select(c => c.Value);
        }

        public IEnumerable<T> Updated()
        {
            return _after.Where(c => _before.ContainsKey(c.Key)).Select(c => c.Value);
        }

        public IEnumerable<T> Removed()
        {
            return _before.Where(c => !_after.ContainsKey(c.Key)).Select(c => c.Value);
        }
    }
}