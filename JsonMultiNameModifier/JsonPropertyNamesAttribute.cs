using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace Guiorgy.JsonExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class JsonPropertyNamesAttribute : JsonAttribute
    {
        public string SerializationName { get; }
        public string[] Names { get; }
        public bool ThrowOnDuplicate { get; }

        public JsonPropertyNamesAttribute(params string[] names)
            : this(false, names.Length != 0 ? names[0] : "", names)
        {
        }

        public JsonPropertyNamesAttribute(string serializationName, params string[] names)
            : this(false, serializationName, names)
        {
        }

        public JsonPropertyNamesAttribute(bool throwOnDuplicate, string serializationName, params string[] names)
        {
            if (names.Length == 0) throw new ArgumentException("Argument can't be empty");

            SerializationName = serializationName;
            Names = names.Contains(serializationName) ? names : names.Concat(new [] { serializationName }).ToArray();
            ThrowOnDuplicate = throwOnDuplicate;
        }
    }
}
