using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace Guiorgy.JsonExtensions
{
    /// <summary>
    /// Specifies the property names that are present in the JSON when serializing and deserializing.
    /// This overrides any naming policy specified by <see cref="System.Text.Json.JsonNamingPolicy"/>.
    /// </summary>
    ///
    /// <remarks>
    /// Should be used along <see cref="Modifiers.JsonMultiNameModifier(System.Text.Json.Serialization.Metadata.JsonTypeInfo)"/> modifier.
    /// </remarks>
    ///
    /// <seealso cref="Modifiers.JsonMultiNameModifier(System.Text.Json.Serialization.Metadata.JsonTypeInfo)"/>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class JsonPropertyNamesAttribute : JsonAttribute
    {
        /// <summary>
        /// The name of the property when serializing.
        /// </summary>
        public string SerializationName { get; }
        /// <summary>
        /// The names of the property.
        /// </summary>
        public string[] Names { get; }
        /// <summary>
        /// If <c>true</c>, a <see cref="System.Text.Json.JsonException"/> will be thrown during deserialization
        /// if more than one JSON key matches any of the <see cref="Names"/>.
        /// </summary>
        public bool ThrowOnDuplicate { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="JsonPropertyNamesAttribute"/> with the specified property names.
        /// </summary>
        /// <param name="names">The names of the property.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="names"/> is empty.
        /// </exception>
        public JsonPropertyNamesAttribute(params string[] names)
            : this(false, names.Length != 0 ? names[0] : "", names)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="JsonPropertyNamesAttribute"/> with the specified property names.
        /// </summary>
        /// <param name="serializationName">The name of the property when serializing.</param>
        /// <param name="names">The names of the property.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="names"/> is empty.
        /// </exception>
        public JsonPropertyNamesAttribute(string serializationName, params string[] names)
            : this(false, serializationName, names)
        {
        }


        /// <summary>
        /// Initializes a new instance of <see cref="JsonPropertyNamesAttribute"/> with the specified property names.
        /// </summary>
        /// <param name="throwOnDuplicate">
        /// If <c>true</c>, a <see cref="System.Text.Json.JsonException"/> will be thrown during deserialization
        /// if more than one JSON key matches any of the <paramref name="names"/>.
        /// </param>
        /// <param name="serializationName">The name of the property when serializing.</param>
        /// <param name="names">The names of the property.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="names"/> is empty.
        /// </exception>
        public JsonPropertyNamesAttribute(bool throwOnDuplicate, string serializationName, params string[] names)
        {
            if (names.Length == 0) throw new ArgumentException("Argument can't be empty");

            SerializationName = serializationName;
            Names = names.Contains(serializationName) ? names : [..names, serializationName];
            ThrowOnDuplicate = throwOnDuplicate;
        }
    }
}
