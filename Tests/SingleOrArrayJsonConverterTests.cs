using Guiorgy.JsonExtensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tests
{
    [TestClass]
    public sealed class SingleOrArrayJsonConverterGenericTests
    {
        public sealed class ClassProperty
        {
            [JsonConverter(typeof(SingleOrArrayJsonConverter<string>))]
            public string[]? Array { get; set; }
        }

        public sealed class ClassConstructor
        {
            [JsonConverter(typeof(SingleOrArrayJsonConverter<string>))]
            public string[] Array { get; }

            [JsonConstructor]
            public ClassConstructor(string[] array)
            {
                Array = array;
            }
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
        public void TestGenericSingleConstructor()
        {
            const string json = """{"Array": "single"}""";

            var deserialized = JsonSerializer.Deserialize<ClassConstructor>(json);
            Assert.IsNotNull(deserialized);
            Assert.IsNotNull(deserialized.Array);
            Assert.AreEqual(1, deserialized.Array.Length);
            Assert.AreEqual("single", deserialized.Array[0]);

            var serialized = JsonSerializer.Serialize(deserialized);
            Assert.AreEqual("""{"Array":["single"]}""", serialized);
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
        public void TestGenericArrayConstructor()
        {
            const string json = """{"Array": ["first", "second"]}""";

            var deserialized = JsonSerializer.Deserialize<ClassConstructor>(json);
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
    public sealed class SingleOrArrayJsonConverterDeductionTests
    {
        public sealed class ClassProperty
        {
            [JsonConverter(typeof(SingleOrArrayJsonConverter))]
            public string[]? Array { get; set; }
        }

        public sealed class ClassConstructor
        {
            [JsonConverter(typeof(SingleOrArrayJsonConverter))]
            public string[] Array { get; }

            [JsonConstructor]
            public ClassConstructor(string[] array)
            {
                Array = array;
            }
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
        public void TestGenericSingleConstructor()
        {
            const string json = """{"Array": "single"}""";

            var deserialized = JsonSerializer.Deserialize<ClassConstructor>(json);
            Assert.IsNotNull(deserialized);
            Assert.IsNotNull(deserialized.Array);
            Assert.AreEqual(1, deserialized.Array.Length);
            Assert.AreEqual("single", deserialized.Array[0]);

            var serialized = JsonSerializer.Serialize(deserialized);
            Assert.AreEqual("""{"Array":["single"]}""", serialized);
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
        public void TestGenericArrayConstructor()
        {
            const string json = """{"Array": ["first", "second"]}""";

            var deserialized = JsonSerializer.Deserialize<ClassConstructor>(json);
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
    public sealed class SingleOrArrayJsonConverterCustomTests
    {
        public sealed class Person
        {
            public string FirstName { get; }
            public string LastName { get; }

            public Person(string firstName, string lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }
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

        public sealed class ClassProperty
        {
            [JsonConverter(typeof(SingleOrArrayJsonConverter<string, PersonJsonConverter>))]
            public string[]? Array { get; set; }
        }

        public sealed class ClassConstructor
        {
            [JsonConverter(typeof(SingleOrArrayJsonConverter<string, PersonJsonConverter>))]
            public string[] Array { get; }

            [JsonConstructor]
            public ClassConstructor(string[] array)
            {
                Array = array;
            }
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
        public void TestGenericSingleConstructor()
        {
            const string json = """{"Array": {"FirstName": "John", "LastName": "Smith"}}""";

            var deserialized = JsonSerializer.Deserialize<ClassConstructor>(json);
            Assert.IsNotNull(deserialized);
            Assert.IsNotNull(deserialized.Array);
            Assert.AreEqual(1, deserialized.Array.Length);
            Assert.AreEqual("John Smith", deserialized.Array[0]);

            var serialized = JsonSerializer.Serialize(deserialized);
            Assert.AreEqual("""{"Array":[{"FirstName":"John","LastName":"Smith"}]}""", serialized);
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
        public void TestGenericArrayConstructor()
        {
            const string json = """{"Array": [{"FirstName": "John", "LastName": "Smith"}, {"FirstName": "John", "LastName": "Doe"}]}""";

            var deserialized = JsonSerializer.Deserialize<ClassConstructor>(json);
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