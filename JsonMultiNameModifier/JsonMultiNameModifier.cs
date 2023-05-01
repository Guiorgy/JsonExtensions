using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Guiorgy.JsonExtensions
{
    /// <summary>
    /// A static class that holds <see cref="System.Text.Json"/> modifiers
    /// </summary>
    ///
    /// <example>
    /// <code>
    /// JsonSerializerOptions jsonOptions = new()
    /// {
    ///     TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    ///     {
    ///         Modifiers = { Modifiers.JsonMultiNameModifier }
    ///     }
    /// }
    /// </code>
    /// </example>
public static class Modifiers
    {
        /// <summary>
        /// A <see cref="System.Text.Json"/> modifier that allows mapping of multiple JSON keys to one C# property.
        /// </summary>
        ///
        /// <param name="typeInfo">A <see cref="JsonTypeInfo"/> defining a reflection-derived JSON contract.</param>
        ///
        /// <remarks>
        /// Should be used along <see cref="JsonPropertyNamesAttribute"/> attribute.
        /// <br/><br/>
        /// Doesn't currently work with non-public fields/properties and public constructors!
        /// </remarks>
        ///
        /// <example>
        /// <code>
        /// public sealed class User
        /// {
        ///     [JsonPropertyNames("UserName", "User", "Name")]
        ///     public string UserName { get; set; }
        /// }
        ///
        /// JsonSerializerOptions jsonOptions = new()
        /// {
        ///     TypeInfoResolver = new DefaultJsonTypeInfoResolver()
        ///     {
        ///         Modifiers = { Modifiers.JsonMultiNameModifier }
        ///     }
        /// }
        ///
        /// const string json1 = """{"UserName": "JohnSmith1"}""";
        /// const string json2 = """{"User": "JohnSmith2"}""";
        /// const string json3 = """{"Name": "JohnSmith3"}""";
        /// var deserialized = JsonSerializer.Deserialize&lt;User&gt;(json1, jsonOptions);
        /// Console.WriteLine(deserialized.UserName);
        /// // Output: "JohnSmith1"
        /// deserialized = JsonSerializer.Deserialize&lt;User&gt;(json2, jsonOptions);
        /// Console.WriteLine(deserialized.UserName);
        /// // Output: "JohnSmith2"
        /// deserialized = JsonSerializer.Deserialize&lt;User&gt;(json3, jsonOptions);
        /// Console.WriteLine(deserialized.UserName);
        /// // Output: "JohnSmith3"
        /// </code>
        /// </example>
        public static void JsonMultiNameModifier(JsonTypeInfo typeInfo)
        {
            var propertyNames = typeInfo.Properties.Select(p => p.Name).ToArray();

            List<JsonPropertyInfo> propertiesToAdd = new();
            List<JsonPropertyInfo> propertiesToRemove = new();

            foreach (var property in typeInfo.Properties)
            {
                if (property.AttributeProvider != null)
                {
                    var attribute =
                        (JsonPropertyNamesAttribute?)property.AttributeProvider.GetCustomAttributes(
                            typeof(JsonPropertyNamesAttribute),
                            inherit: true
                        ).FirstOrDefault();

                    if (attribute != null)
                    {
                        var jsonPropertyNameAttribute =
                            (JsonPropertyNameAttribute?)property.AttributeProvider.GetCustomAttributes(
                                typeof(JsonPropertyNameAttribute),
                                inherit: true
                            ).FirstOrDefault();
                        if (jsonPropertyNameAttribute != null)
                        {
                            throw new NotImplementedException("The behaviour of System.Text.Json.Serialization.JsonPropertyNameAttribute "
                                + "with Guiorgy.JsonExtensions.JsonPropertyNamesAttribute is undefined");
                        }

                        propertiesToRemove.Add(property);

                        WeakReference? Object = null;
                        bool propertySet = false;
                        var propertySetter = (object obj, object? value) =>
                        {
                            Object ??= new WeakReference(obj);

                            if (Object.Target != obj) propertySet = false;

                            if (propertySet && attribute.ThrowOnDuplicate)
                            {
                                throw new JsonException($"Duplicate key detected for the \"{property.Name}\" property for class {typeInfo.Type.FullName}");
                            }

                            propertySet = true;

                            property.Set?.Invoke(obj, value);
                        };

                        foreach (var name in attribute.Names)
                        {
                            if (name != property.Name && propertyNames.Contains(name))
                                throw new ArgumentException($"{typeInfo.Type.FullName} already contains a property named \"{name}\"");

                            var newProperty = typeInfo.CreateJsonPropertyInfo(property.PropertyType, name);
                            newProperty.Set = propertySetter;
                            if (name == attribute.SerializationName)
                            {
                                newProperty.Get = property.Get;
                                newProperty.ShouldSerialize = property.ShouldSerialize;
                            }
                            propertiesToAdd.Add(newProperty);
                        }
                    }
                }
            }

            foreach (var property in propertiesToRemove)
            {
                typeInfo.Properties.Remove(property);
            }

            foreach (var property in propertiesToAdd)
            {
                typeInfo.Properties.Add(property);
            }
        }
    }
}
