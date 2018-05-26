using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace AppGet.Manifest.Builder
{
    [DebuggerDisplay("{Value} [{Values.Count}]")]
    public class ManifestAttribute<T>
    {
        private readonly Func<T, object> _secondarySort;
        public List<ManifestAttributeCandidate<T>> Values { get; }

        [JsonIgnore]
        public T Value
        {
            get
            {
                var topAttr = GetTop();
                return topAttr == null ? default(T) : topAttr.Value;
            }
        }

        [JsonIgnore]
        public bool HasValue => HasConfidence(Confidence.LastEffort);

        public ManifestAttribute(Func<T, object> secondarySort = null)
        {
            if (secondarySort == null)
            {
                secondarySort = candidate => null;
            }

            _secondarySort = secondarySort;

            Values = new List<ManifestAttributeCandidate<T>>();
        }

        public T Add(T value, Confidence confidence, object source)
        {
            var attr = new ManifestAttributeCandidate<T>(value, confidence, source);

            if (attr.Confidence != Confidence.None && value != null)
            {
                Values.Add(attr);
            }
            return attr.Value;
        }

        public ManifestAttributeCandidate<T> GetTop()
        {
            return Values
                .AsEnumerable()
                .Reverse()
                .OrderByDescending(c => c.Confidence)
                .ThenBy(c => c.Value == null)
                .ThenByDescending(c => _secondarySort(c.Value))
                .FirstOrDefault(c => c.Confidence > Confidence.None);
        }

        public bool HasConfidence(Confidence confidence)
        {
            return GetTop()?.Confidence >= confidence;
        }

        public override string ToString()
        {
            var top = GetTop();
            return top?.Value.ToString() ?? "";
        }
    }
}