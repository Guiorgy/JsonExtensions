using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            List<JsonPropertyInfo> propertiesToAdd = [];
            List<JsonPropertyInfo> propertiesToRemove = [];

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

                        WeakReference? Object = null;
                        bool propertySet = false;
#pragma warning disable IDE0039 // Use local function (test fails)
                        var propertySetter = (object obj, object? value) =>
                        {
                            Object ??= new WeakReference(obj);

                            if (Object.Target != obj) propertySet = false;

                            if (propertySet && attribute.ThrowOnDuplicate)
                            {
                                throw new JsonException($"Duplicate key detected for the \"{property.Name}\" property for class {typeInfo.Type.FullName}");
                            }

                            propertySet = true;

                            if (property.Set != null)
                            {
                                property.Set(obj, value);
                            } else
                            {
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
                                var setter = typeInfo.Type.GetProperty(property.Name)?.GetSetMethod(nonPublic: true);

                                if (setter != null)
                                {
                                    setter.Invoke(obj, [value]);
                                }
                                else
                                {
                                    var backingField =
                                        typeInfo.Type.GetField(
                                            $"<{property.Name}>k__BackingField",
                                            BindingFlags.Instance | BindingFlags.NonPublic
                                        );

                                    if (backingField != null)
                                    {
                                        backingField.SetValue(obj, value);
                                    }
                                    else
                                    {
                                        var field = typeInfo.Type.GetField(property.Name)
                                            ?? throw new JsonException($"Can't set \"{property.Name}\" property for \"{typeInfo.Type.FullName}\" type");

                                        field.SetValue(obj, value);
                                    }
                                }
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
                            }
                        };
#pragma warning restore IDE0039 // Use local function

                        foreach (var name in attribute.Names)
                        {
                            if (name != property.Name && propertyNames.Contains(name))
                                throw new ArgumentException($"{typeInfo.Type.FullName} already contains a property named \"{name}\"");

                            var newProperty = typeInfo.CreateJsonPropertyInfo(property.PropertyType, name);
                            newProperty.AttributeProvider = property.AttributeProvider;
                            newProperty.CustomConverter = property.CustomConverter;
                            newProperty.Order = property.Order;
                            newProperty.NumberHandling = property.NumberHandling;
                            newProperty.Set = propertySetter;
                            if (name == attribute.SerializationName)
                            {
                                newProperty.Get = property.Get;
                                newProperty.ShouldSerialize = property.ShouldSerialize;
                            }
                            propertiesToAdd.Add(newProperty);
                        }

                        if (attribute.Names.Select(name => name.ToLower()).Contains(property.Name.ToLower()))
                        {
                            propertiesToRemove.Add(property);
                        }
                        else
                        {
                            var constructors = typeInfo.Type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

                            var jsonConstructor = Array.Find(constructors, constructor => constructor.GetCustomAttribute<JsonConstructorAttribute>() != null);
                            if (jsonConstructor != null) constructors = [jsonConstructor];

                            var propertyName = property.Name.ToLower();
                            if (constructors.Any(constructor => constructor.GetParameters().Select(p => p.Name?.ToLower()).Contains(propertyName)))
                            {
                                property.ShouldSerialize = (object _, object? __) => false;
                            }
                            else
                            {
                                propertiesToRemove.Add(property);
                            }
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
