using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guiorgy.JsonExtensions
{
    public sealed class SingleOrArrayJsonConverter<TValue, TJsonConverter> : JsonConverterFactory where TJsonConverter : JsonConverter<TValue>
	{
		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert == typeof(TValue[]);
		}

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
