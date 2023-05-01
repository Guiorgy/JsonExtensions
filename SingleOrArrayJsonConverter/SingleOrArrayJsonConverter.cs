using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guiorgy.JsonExtensions
{
    /// <summary>
    /// A JsonConverter that deserializes both a single JSON object and a JSON array as a C# array.
    /// </summary>
    ///
    /// <remarks>
    /// The type of the field/property must be an array, e.g. <c>MyType[]</c>
    /// </remarks>
    ///
    /// <seealso cref="SingleOrArrayJsonConverter{TValue}"/>
    /// <seealso cref="SingleOrArrayJsonConverter{TValue, TJsonConverter}"/>
    ///
    /// <example>
    /// <code>
    /// public sealed class Example
    /// {
    ///     [JsonConverter(typeof(SingleOrArrayJsonConverter))]
    ///     public string[]? Strings { get; set; }
    /// }
    ///
    /// const string jsonSingle = """{"Strings": "single"}""";
    /// var deserializedSingle = JsonSerializer.Deserialize&lt;Example&gt;(jsonSingle);
    /// Console.WriteLine($"{deserializedSingle.Strings.Length}: {deserializedSingle.Strings[0]}");
    /// // Output: "1: single"
    ///
    /// const string jsonArray = """{"Strings": ["first", "second"]}""";
    /// var deserializedArray = JsonSerializer.Deserialize&lt;Example&gt;(jsonArray);
    /// Console.WriteLine($"{deserializedArray.Strings.Length}: {deserializedArray.Strings[0]}, {deserializedArray.Strings[1]}");
    /// // Output: "2: first, second"
    /// </code>
    /// </example>
    public sealed class SingleOrArrayJsonConverter : JsonConverterFactory
    {
        private JsonConverterFactory? _jsonConverterFactory;

        private JsonConverterFactory GetJsonConverterFactory(Type typeToConvert)
        {
            if (_jsonConverterFactory == null)
            {
                if (!typeToConvert.IsArray) throw new ArgumentException($"{nameof(typeToConvert)} must be an Array!");

                Type valueType = typeToConvert.GetElementType()!;

                _jsonConverterFactory =
                    (JsonConverterFactory)Activator.CreateInstance(
                        typeof(SingleOrArrayJsonConverter<>).MakeGenericType(valueType),
                        BindingFlags.Instance | BindingFlags.Public,
                        binder: null,
                        args: null,
                        culture: null)!;
            }
            return _jsonConverterFactory;
        }

        /// <summary>
        /// Determines whether the specified type can be converted.
        /// </summary>
        /// <param name="typeToConvert">The type to compare against.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.IsArray)
                return GetJsonConverterFactory(typeToConvert).CanConvert(typeToConvert);
            else
                return false;
        }

        /// <summary>
        /// Create a converter for the provided <see cref="Type"/>.
        /// </summary>
        /// <param name="typeToConvert">The <see cref="Type"/> being converted.</param>
        /// <param name="options">The <see cref="JsonSerializerOptions"/> being used.</param>
        /// <returns>
        /// An instance of a <see cref="JsonConverter{T}"/> where T is compatible with <paramref name="typeToConvert"/>.
        /// If <see langword="null"/> is returned, a <see cref="NotSupportedException"/> will be thrown.
        /// </returns>
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return GetJsonConverterFactory(typeToConvert).CreateConverter(typeToConvert, options)!;
        }
    }
}
