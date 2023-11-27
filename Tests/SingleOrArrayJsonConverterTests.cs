using Guiorgy.JsonExtensions;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tests
{
    public sealed class SingleOrArrayJsonConverterTests
    {
        [TestClass]
        public sealed class GenericAttributeTests
        {
            public static readonly JsonSerializerOptions JsonOptionsIncludeFields = new()
            {
                IncludeFields = true
            };

            public sealed class ClassField
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter<string>))]
                public string[]? array;
            }

            public sealed class ClassProperty
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter<string>))]
                public string[]? Array { get; set; }
            }

            [method: JsonConstructor]
            public sealed class ClassConstructorField(string[] array)
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter<string>))]
                public string[] array = array;
            }

            [method: JsonConstructor]
            public sealed class ClassConstructorProperty(string[] array)
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter<string>))]
                public string[] Array { get; } = array;
            }

            [TestMethod]
            public void TestGenericSingleField()
            {
                const string json = """{"array": "single"}""";

                var deserialized = JsonSerializer.Deserialize<ClassField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(1, deserialized.array.Length);
                Assert.AreEqual("single", deserialized.array[0]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":["single"]}""", serialized);
            }

            [TestMethod]
            public void TestGenericSingleProperty()
            {
                const string json = """{"Array": "single"}""";

                var deserialized = JsonSerializer.Deserialize<ClassProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(1, deserialized.Array.Length);
                Assert.AreEqual("single", deserialized.Array[0]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":["single"]}""", serialized);
            }

            [TestMethod]
            public void TestGenericSingleConstructorField()
            {
                const string json = """{"array": "single"}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(1, deserialized.array.Length);
                Assert.AreEqual("single", deserialized.array[0]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":["single"]}""", serialized);
            }

            [TestMethod]
            public void TestGenericSingleConstructorProperty()
            {
                const string json = """{"Array": "single"}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(1, deserialized.Array.Length);
                Assert.AreEqual("single", deserialized.Array[0]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":["single"]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayField()
            {
                const string json = """{"array": ["first", "second"]}""";

                var deserialized = JsonSerializer.Deserialize<ClassField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(2, deserialized.array.Length);
                Assert.AreEqual("first", deserialized.array[0]);
                Assert.AreEqual("second", deserialized.array[1]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":["first","second"]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayProperty()
            {
                const string json = """{"Array": ["first", "second"]}""";

                var deserialized = JsonSerializer.Deserialize<ClassProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(2, deserialized.Array.Length);
                Assert.AreEqual("first", deserialized.Array[0]);
                Assert.AreEqual("second", deserialized.Array[1]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":["first","second"]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayConstructorField()
            {
                const string json = """{"array": ["first", "second"]}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(2, deserialized.array.Length);
                Assert.AreEqual("first", deserialized.array[0]);
                Assert.AreEqual("second", deserialized.array[1]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":["first","second"]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayConstructor()
            {
                const string json = """{"Array": ["first", "second"]}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(2, deserialized.Array.Length);
                Assert.AreEqual("first", deserialized.Array[0]);
                Assert.AreEqual("second", deserialized.Array[1]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":["first","second"]}""", serialized);
            }
        }

        [TestClass]
        public sealed class TypeDeductionTests
        {
            public static readonly JsonSerializerOptions JsonOptionsIncludeFields = new()
            {
                IncludeFields = true
            };

            public sealed class ClassField
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter))]
                public string[]? array;
            }

            public sealed class ClassProperty
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter))]
                public string[]? Array { get; set; }
            }

            [method: JsonConstructor]
            public sealed class ClassConstructorField(string[] array)
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter))]
                public string[] array = array;
            }

            [method: JsonConstructor]
            public sealed class ClassConstructorProperty(string[] array)
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter))]
                public string[] Array { get; } = array;
            }

            [TestMethod]
            public void TestGenericSingleField()
            {
                const string json = """{"array": "single"}""";

                var deserialized = JsonSerializer.Deserialize<ClassField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(1, deserialized.array.Length);
                Assert.AreEqual("single", deserialized.array[0]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":["single"]}""", serialized);
            }

            [TestMethod]
            public void TestGenericSingleProperty()
            {
                const string json = """{"Array": "single"}""";

                var deserialized = JsonSerializer.Deserialize<ClassProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(1, deserialized.Array.Length);
                Assert.AreEqual("single", deserialized.Array[0]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":["single"]}""", serialized);
            }

            [TestMethod]
            public void TestGenericSingleConstructorField()
            {
                const string json = """{"array": "single"}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(1, deserialized.array.Length);
                Assert.AreEqual("single", deserialized.array[0]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":["single"]}""", serialized);
            }

            [TestMethod]
            public void TestGenericSingleConstructorProperty()
            {
                const string json = """{"Array": "single"}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(1, deserialized.Array.Length);
                Assert.AreEqual("single", deserialized.Array[0]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":["single"]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayField()
            {
                const string json = """{"array": ["first", "second"]}""";

                var deserialized = JsonSerializer.Deserialize<ClassField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(2, deserialized.array.Length);
                Assert.AreEqual("first", deserialized.array[0]);
                Assert.AreEqual("second", deserialized.array[1]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":["first","second"]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayProperty()
            {
                const string json = """{"Array": ["first", "second"]}""";

                var deserialized = JsonSerializer.Deserialize<ClassProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(2, deserialized.Array.Length);
                Assert.AreEqual("first", deserialized.Array[0]);
                Assert.AreEqual("second", deserialized.Array[1]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":["first","second"]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayConstructorField()
            {
                const string json = """{"array": ["first", "second"]}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(2, deserialized.array.Length);
                Assert.AreEqual("first", deserialized.array[0]);
                Assert.AreEqual("second", deserialized.array[1]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":["first","second"]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayConstructor()
            {
                const string json = """{"Array": ["first", "second"]}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(2, deserialized.Array.Length);
                Assert.AreEqual("first", deserialized.Array[0]);
                Assert.AreEqual("second", deserialized.Array[1]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":["first","second"]}""", serialized);
            }
        }

        [TestClass]
        public sealed class CustomConverterTests
        {
            public sealed class Person(string firstName, string lastName)
            {
                public string FirstName { get; } = firstName;
                public string LastName { get; } = lastName;
            }

            public sealed class PersonJsonConverter : JsonConverter<string>
            {
                public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    var personConverter = (JsonConverter<Person>)options.GetConverter(typeof(Person));

                    Person? person = personConverter.Read(ref reader, typeof(Person), options);

                    return person != null ? $"{person.FirstName} {person.LastName}" : null;
                }

                public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
                {
                    var personConverter = (JsonConverter<Person>)options.GetConverter(typeof(Person));

                    var split = value.Split(' ');
                    var firstName = split[0];
                    var lastName = split[1];
                    var person = new Person(firstName, lastName);

                    personConverter.Write(writer, person, options);
                }
            }

            public static readonly JsonSerializerOptions JsonOptionsIncludeFields = new()
            {
                IncludeFields = true
            };

            public sealed class ClassField
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter<PersonJsonConverter>))]
                public string[]? array;
            }

            public sealed class ClassProperty
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter<PersonJsonConverter>))]
                public string[]? Array { get; set; }
            }

            [method: JsonConstructor]
            public sealed class ClassConstructorField(string[] array)
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter<PersonJsonConverter>))]
                public string[] array = array;
            }

            [method: JsonConstructor]
            public sealed class ClassConstructorProperty(string[] array)
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter<PersonJsonConverter>))]
                public string[] Array { get; } = array;
            }

            [TestMethod]
            public void TestGenericSingleField()
            {
                const string json = """{"array": {"FirstName": "John", "LastName": "Smith"}}""";

                var deserialized = JsonSerializer.Deserialize<ClassField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(1, deserialized.array.Length);
                Assert.AreEqual("John Smith", deserialized.array[0]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":[{"FirstName":"John","LastName":"Smith"}]}""", serialized);
            }

            [TestMethod]
            public void TestGenericSingleProperty()
            {
                const string json = """{"Array": {"FirstName": "John", "LastName": "Smith"}}""";

                var deserialized = JsonSerializer.Deserialize<ClassProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(1, deserialized.Array.Length);
                Assert.AreEqual("John Smith", deserialized.Array[0]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":[{"FirstName":"John","LastName":"Smith"}]}""", serialized);
            }

            [TestMethod]
            public void TestGenericSingleConstructorField()
            {
                const string json = """{"array": {"FirstName": "John", "LastName": "Smith"}}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(1, deserialized.array.Length);
                Assert.AreEqual("John Smith", deserialized.array[0]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":[{"FirstName":"John","LastName":"Smith"}]}""", serialized);
            }

            [TestMethod]
            public void TestGenericSingleConstructorProperty()
            {
                const string json = """{"Array": {"FirstName": "John", "LastName": "Smith"}}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(1, deserialized.Array.Length);
                Assert.AreEqual("John Smith", deserialized.Array[0]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":[{"FirstName":"John","LastName":"Smith"}]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayField()
            {
                const string json = """{"array": [{"FirstName": "John", "LastName": "Smith"}, {"FirstName": "John", "LastName": "Doe"}]}""";

                var deserialized = JsonSerializer.Deserialize<ClassField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(2, deserialized.array.Length);
                Assert.AreEqual("John Smith", deserialized.array[0]);
                Assert.AreEqual("John Doe", deserialized.array[1]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":[{"FirstName":"John","LastName":"Smith"},{"FirstName":"John","LastName":"Doe"}]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayProperty()
            {
                const string json = """{"Array": [{"FirstName": "John", "LastName": "Smith"}, {"FirstName": "John", "LastName": "Doe"}]}""";

                var deserialized = JsonSerializer.Deserialize<ClassProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(2, deserialized.Array.Length);
                Assert.AreEqual("John Smith", deserialized.Array[0]);
                Assert.AreEqual("John Doe", deserialized.Array[1]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":[{"FirstName":"John","LastName":"Smith"},{"FirstName":"John","LastName":"Doe"}]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayConstructorField()
            {
                const string json = """{"array": [{"FirstName": "John", "LastName": "Smith"}, {"FirstName": "John", "LastName": "Doe"}]}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(2, deserialized.array.Length);
                Assert.AreEqual("John Smith", deserialized.array[0]);
                Assert.AreEqual("John Doe", deserialized.array[1]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":[{"FirstName":"John","LastName":"Smith"},{"FirstName":"John","LastName":"Doe"}]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayConstructor()
            {
                const string json = """{"Array": [{"FirstName": "John", "LastName": "Smith"}, {"FirstName": "John", "LastName": "Doe"}]}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(2, deserialized.Array.Length);
                Assert.AreEqual("John Smith", deserialized.Array[0]);
                Assert.AreEqual("John Doe", deserialized.Array[1]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":[{"FirstName":"John","LastName":"Smith"},{"FirstName":"John","LastName":"Doe"}]}""", serialized);
            }
        }

        [TestClass]
        public sealed class GenericAttributeAndCustomConverterTests
        {
            public sealed class Person(string firstName, string lastName)
            {
                public string FirstName { get; } = firstName;
                public string LastName { get; } = lastName;
            }

            public sealed class PersonJsonConverter : JsonConverter<string>
            {
                public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    var personConverter = (JsonConverter<Person>)options.GetConverter(typeof(Person));

                    Person? person = personConverter.Read(ref reader, typeof(Person), options);

                    return person != null ? $"{person.FirstName} {person.LastName}" : null;
                }

                public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
                {
                    var personConverter = (JsonConverter<Person>)options.GetConverter(typeof(Person));

                    var split = value.Split(' ');
                    var firstName = split[0];
                    var lastName = split[1];
                    var person = new Person(firstName, lastName);

                    personConverter.Write(writer, person, options);
                }
            }

            public static readonly JsonSerializerOptions JsonOptionsIncludeFields = new()
            {
                IncludeFields = true
            };

            public sealed class ClassField
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter<string, PersonJsonConverter>))]
                public string[]? array;
            }

            public sealed class ClassProperty
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter<string, PersonJsonConverter>))]
                public string[]? Array { get; set; }
            }

            [method: JsonConstructor]
            public sealed class ClassConstructorField(string[] array)
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter<string, PersonJsonConverter>))]
                public string[] array = array;
            }

            [method: JsonConstructor]
            public sealed class ClassConstructorProperty(string[] array)
            {
                [JsonConverter(typeof(SingleOrArrayJsonConverter<string, PersonJsonConverter>))]
                public string[] Array { get; } = array;
            }

            [TestMethod]
            public void TestGenericSingleField()
            {
                const string json = """{"array": {"FirstName": "John", "LastName": "Smith"}}""";

                var deserialized = JsonSerializer.Deserialize<ClassField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(1, deserialized.array.Length);
                Assert.AreEqual("John Smith", deserialized.array[0]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":[{"FirstName":"John","LastName":"Smith"}]}""", serialized);
            }

            [TestMethod]
            public void TestGenericSingleProperty()
            {
                const string json = """{"Array": {"FirstName": "John", "LastName": "Smith"}}""";

                var deserialized = JsonSerializer.Deserialize<ClassProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(1, deserialized.Array.Length);
                Assert.AreEqual("John Smith", deserialized.Array[0]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":[{"FirstName":"John","LastName":"Smith"}]}""", serialized);
            }

            [TestMethod]
            public void TestGenericSingleConstructorField()
            {
                const string json = """{"array": {"FirstName": "John", "LastName": "Smith"}}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(1, deserialized.array.Length);
                Assert.AreEqual("John Smith", deserialized.array[0]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":[{"FirstName":"John","LastName":"Smith"}]}""", serialized);
            }

            [TestMethod]
            public void TestGenericSingleConstructorProperty()
            {
                const string json = """{"Array": {"FirstName": "John", "LastName": "Smith"}}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(1, deserialized.Array.Length);
                Assert.AreEqual("John Smith", deserialized.Array[0]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":[{"FirstName":"John","LastName":"Smith"}]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayField()
            {
                const string json = """{"array": [{"FirstName": "John", "LastName": "Smith"}, {"FirstName": "John", "LastName": "Doe"}]}""";

                var deserialized = JsonSerializer.Deserialize<ClassField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(2, deserialized.array.Length);
                Assert.AreEqual("John Smith", deserialized.array[0]);
                Assert.AreEqual("John Doe", deserialized.array[1]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":[{"FirstName":"John","LastName":"Smith"},{"FirstName":"John","LastName":"Doe"}]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayProperty()
            {
                const string json = """{"Array": [{"FirstName": "John", "LastName": "Smith"}, {"FirstName": "John", "LastName": "Doe"}]}""";

                var deserialized = JsonSerializer.Deserialize<ClassProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(2, deserialized.Array.Length);
                Assert.AreEqual("John Smith", deserialized.Array[0]);
                Assert.AreEqual("John Doe", deserialized.Array[1]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":[{"FirstName":"John","LastName":"Smith"},{"FirstName":"John","LastName":"Doe"}]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayConstructorField()
            {
                const string json = """{"array": [{"FirstName": "John", "LastName": "Smith"}, {"FirstName": "John", "LastName": "Doe"}]}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorField>(json, JsonOptionsIncludeFields);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.array);
                Assert.AreEqual(2, deserialized.array.Length);
                Assert.AreEqual("John Smith", deserialized.array[0]);
                Assert.AreEqual("John Doe", deserialized.array[1]);

                var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                Assert.AreEqual("""{"array":[{"FirstName":"John","LastName":"Smith"},{"FirstName":"John","LastName":"Doe"}]}""", serialized);
            }

            [TestMethod]
            public void TestGenericArrayConstructorProperty()
            {
                const string json = """{"Array": [{"FirstName": "John", "LastName": "Smith"}, {"FirstName": "John", "LastName": "Doe"}]}""";

                var deserialized = JsonSerializer.Deserialize<ClassConstructorProperty>(json);
                Assert.IsNotNull(deserialized);
                Assert.IsNotNull(deserialized.Array);
                Assert.AreEqual(2, deserialized.Array.Length);
                Assert.AreEqual("John Smith", deserialized.Array[0]);
                Assert.AreEqual("John Doe", deserialized.Array[1]);

                var serialized = JsonSerializer.Serialize(deserialized);
                Assert.AreEqual("""{"Array":[{"FirstName":"John","LastName":"Smith"},{"FirstName":"John","LastName":"Doe"}]}""", serialized);
            }
        }
    }
}