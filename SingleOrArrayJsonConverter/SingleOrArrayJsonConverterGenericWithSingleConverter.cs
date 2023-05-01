using System;
using System.Collections.Generic;
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
    /// <typeparamref  name="TValue">Either the array element type.</typeparamref>
    /// <typeparamref  name="TJsonConverter">A custom JsonConverter for <typeparamref name="TValue"/>.</typeparamref>
    ///
    /// <remarks>
    /// The type of the field/property must be an array, e.g. <c>MyType[]</c>
    /// </remarks>
    ///
    /// <seealso cref="SingleOrArrayJsonConverter"/>
    /// <seealso cref="SingleOrArrayJsonConverter{TValue}"/>
    ///
    /// <example>
    /// <code>
    /// public sealed class DoubleJsonConverter : JsonConverter&lt;string&gt;
    /// {
    ///     public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    ///     {
    ///         var value = reader.GetString();
    ///         return value + value;
    ///     }
    ///
    ///     public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    ///     {
    ///         writer.WriteStringValue(value.Substring(value.Length / 2));
    ///     }
    /// }
    ///
    /// public sealed class Example
    /// {
    ///     [JsonConverter(typeof(SingleOrArrayJsonConverter&lt;DoubleJsonConverter&gt;))]
    ///     public string[]? Strings { get; set; }
    /// }
    ///
    /// const string jsonSingle = """{"Strings": "single"}""";
    /// var deserializedSingle = JsonSerializer.Deserialize&lt;Example&gt;(jsonSingle);
    /// Console.WriteLine($"{deserializedSingle.Strings.Length}: {deserializedSingle.Strings[0]}");
    /// // Output: "1: singlesingle"
    ///
    /// const string jsonArray = """{"Strings": ["first", "second"]}""";
    /// var deserializedArray = JsonSerializer.Deserialize&lt;Example&gt;(jsonArray);
    /// Console.WriteLine($"{deserializedArray.Strings.Length}: {deserializedArray.Strings[0]}, {deserializedArray.Strings[1]}");
    /// // Output: "2: firstfirst, secondsecond"
    /// </code>
    /// </example>
    public sealed class SingleOrArrayJsonConverter<TValue, TJsonConverter> : JsonConverterFactory where TJsonConverter : JsonConverter<TValue>
    {
        /// <summary>
        /// Determines whether the specified type can be converted.
        /// </summary>
        /// <param name="typeToConvert">The type to compare against.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(TValue[]);
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
            return (JsonConverter)Activator.CreateInstance(
                typeof(SingleOrArrayJsonConverterInner),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
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
            public SingleOrArrayJsonConverterInner()
            {
                _singleType = typeof(TValue);
                _singleConverter =
                    (JsonConverter<TValue>)Activator.CreateInstance(
                        typeof(TJsonConverter),
                        BindingFlags.Instance | BindingFlags.Public,
                        binder: null,
                        args: null,
                        culture: null)!;
                _arrayType = typeof(TValue[]);
                _arrayConverter = new ArrayJsonConverter(_singleConverter);
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

            public sealed class ArrayJsonConverter : JsonConverter<TValue[]>
            {
                private readonly Type _valueType;
                private readonly JsonConverter<TValue> _valueConverter;

                public ArrayJsonConverter(JsonConverter<TValue> valueConverter)
                {
                    _valueType = typeof(TValue);
                    _valueConverter = valueConverter;
                }

                public override TValue[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();

                    List<TValue> values = new();
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray) break;

                        var value = _valueConverter.Read(ref reader, _valueType, options);
                        if (value != null) values.Add(value);
                    }
                    return values.ToArray();
                }

                public override void Write(Utf8JsonWriter writer, TValue[] values, JsonSerializerOptions options)
                {
                    writer.WriteStartArray();

                    foreach (TValue value in values)
                        _valueConverter.Write(writer, value, options);

                    writer.WriteEndArray();
                }
            }
        }
    }
}
