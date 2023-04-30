using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Guiorgy.JsonExtensions
{
    public sealed class SingleOrArrayJsonConverter : JsonConverterFactory
	{
		private JsonConverterFactory? _jsonConverterFactory;

        private JsonConverterFactory GetJsonConverterFactory(Type typeToConvert)
		{
			if (_jsonConverterFactory == null)
			{
				ArgumentNullException.ThrowIfNull(typeToConvert);
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

		public override bool CanConvert(Type typeToConvert)
		{
			if (typeToConvert.IsArray)
				return GetJsonConverterFactory(typeToConvert).CanConvert(typeToConvert);
			else
				return false;
		}

		public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
		{
			return GetJsonConverterFactory(typeToConvert).CreateConverter(typeToConvert, options)!;
		}
	}
}
