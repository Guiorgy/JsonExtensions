using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guiorgy.JsonExtensions
{
    public sealed class SingleOrArrayJsonConverter<TValue> : JsonConverterFactory
    {
        private JsonConverterFactory? _jsonConverterFactory;

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
