using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guiorgy.JsonExtensions
{
    /// <summary>
    /// A JsonConverter that deserializes both a single JSON object and a JSON array as a C# array.
    /// </summary>
    ///
    /// <typeparamref  name="TValue">Either the array element type, or a custom JsonConverter for that type.</typeparamref>
    ///
    /// <remarks>
    /// The type of the field/property must be an array, e.g. <c>MyType[]</c>
    /// </remarks>
    ///
    /// <seealso cref="SingleOrArrayJsonConverter"/>
    /// <seealso cref="SingleOrArrayJsonConverter{TValue, TJsonConverter}"/>
    ///
    /// <example>
    /// <code>
    /// public sealed class Example
    /// {
    ///     // [JsonConverter(typeof(SingleOrArrayJsonConverter&lt;CustomJsonConverter&gt;))]
    ///     [JsonConverter(typeof(SingleOrArrayJsonConverter&lt;string&gt;))]
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
    public sealed class SingleOrArrayJsonConverter<TValue> : JsonConverterFactory
    {
        private JsonConverterFactory? _jsonConverterFactory;

        /// <summary>
        /// Determines whether the specified type can be converted.
        /// </summary>
        /// <param name="typeToConvert">The type to compare against.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeof(TValue).IsSubclassOf(typeof(JsonConverter<>)) || typeof(TValue).IsSubclassOf(typeof(JsonConverter)))
            {
                if (typeToConvert.IsArray)
                {
                    Type valueType = typeToConvert.GetElementType()!;

                    _jsonConverterFactory =
                        (JsonConverterFactory)Activator.CreateInstance(
                            typeof(SingleOrArrayJsonConverter<,>).MakeGenericType(valueType, typeof(TValue)),
                            BindingFlags.Instance | BindingFlags.Public,
                            binder: null,
                            args: null,
                            culture: null)!;

                    return _jsonConverterFactory.CanConvert(typeToConvert);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return typeToConvert == typeof(TValue[]);
            }
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
            return _jsonConverterFactory?.CreateConverter(typeToConvert, options)
                ?? (JsonConverter)Activator.CreateInstance(
                    typeof(SingleOrArrayJsonConverterInner),
                    BindingFlags.Instance | BindingFlags.Public,
                    binder: null,
                    args: new object[] { options },
                    culture: null)!;
        }

        private sealed class SingleOrArrayJsonConverterInner : JsonConverter<TValue[]>
        {
            private readonly Type _singleType;
            private readonly JsonConverter<TValue> _singleConverter;
            private readonly Type _arrayType;
            private readonly JsonConverter<TValue[]> _arrayConverter;

            [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression")]
            [SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "Used by reflection")]
            public SingleOrArrayJsonConverterInner(JsonSerializerOptions options)
            {
                _singleType = typeof(TValue);
                _singleConverter = (JsonConverter<TValue>)options.GetConverter(_singleType);

                _arrayType = typeof(TValue[]);
                _arrayConverter = (JsonConverter<TValue[]>)options.GetConverter(_arrayType);
            }

            public override TValue[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    return _arrayConverter.Read(ref reader, _arrayType, options);
                }
                else
                {
                    var value = _singleConverter.Read(ref reader, _singleType, options);
                    return value != null ? new TValue[1] { value } : Array.Empty<TValue>();
                }
            }

            public override void Write(Utf8JsonWriter writer, TValue[] values, JsonSerializerOptions options)
            {
                _arrayConverter.Write(writer, values, options);
            }
        }
    }
}
