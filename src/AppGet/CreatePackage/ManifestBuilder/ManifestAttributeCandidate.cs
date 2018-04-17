using System.Diagnostics;

namespace AppGet.CreatePackage.ManifestBuilder
{
    [DebuggerDisplay("{Value} [{Source}:{Confidence}]")]
    public class ManifestAttributeCandidate<T>
    {
        public T Value { get; }
        public Confidence Confidence { get; }
        public string Source { get; }

        public ManifestAttributeCandidate(T value, Confidence confidence, object source)
        {
            if (value is string)
            {
                var str = value.ToString().Trim();

                if (string.IsNullOrWhiteSpace(str))
                {
                    confidence = Confidence.None;
                    str = null;
                }

                value = (dynamic)str;
            }

            Source = source is string ? source.ToString() : source.GetType().Name.Replace("Extractor", "");

            Value = value;
            Confidence = confidence;
        }
    }
}