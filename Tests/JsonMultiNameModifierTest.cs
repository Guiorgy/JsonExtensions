using Guiorgy.JsonExtensions;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

using static Guiorgy.JsonExtensions.Modifiers;

namespace Tests
{
    public sealed class JsonMultiNameModifierTests
    {
        public static readonly JsonSerializerOptions JsonOptions = new()
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            {
                Modifiers = { JsonMultiNameModifier }
            }
        };

        public sealed class AllowDuplicatesTests
        {
            [TestClass]
            public sealed class BasicTest
            {
                public sealed class User
                {
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public string? UserName { get; set; }
                }

                [TestMethod]
                public void TestSingleKey()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<User>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<User>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<User>(json3, JsonOptions);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.UserName);
                    Assert.AreEqual("JohnSmith", deserialized3.UserName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptions);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptions);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptions);
                    Assert.AreEqual("""{"UserName":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestMultipleKeys()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<User>(json, JsonOptions);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.UserName);
                    Assert.AreEqual("JohnSmith3", deserialized.UserName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptions);
                    Assert.AreEqual("""{"UserName":"JohnSmith3"}""", serialized);
                }
            }

            [TestClass]
            public sealed class FirstNameSerializationTest
            {
                public sealed class User
                {
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public string? Identifier { get; set; }
                }

                [TestMethod]
                public void TestSingleKey()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<User>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized1.Identifier);

                    var deserialized2 = JsonSerializer.Deserialize<User>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized2.Identifier);

                    var deserialized3 = JsonSerializer.Deserialize<User>(json3, JsonOptions);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized3.Identifier);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptions);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptions);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptions);
                    Assert.AreEqual("""{"UserName":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestMultipleKeys()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<User>(json, JsonOptions);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.Identifier);
                    Assert.AreEqual("JohnSmith3", deserialized.Identifier);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptions);
                    Assert.AreEqual("""{"UserName":"JohnSmith3"}""", serialized);
                }
            }

            [TestClass]
            public sealed class DifferentNameSerializationTest
            {
                public sealed class User
                {
                    [JsonPropertyNames(serializationName: "Identifier", "UserName", "User", "Name")]
                    public string? UserName { get; set; }
                }

                [TestMethod]
                public void TestSingleKey()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<User>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<User>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<User>(json3, JsonOptions);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.UserName);
                    Assert.AreEqual("JohnSmith", deserialized3.UserName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptions);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptions);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptions);
                    Assert.AreEqual("""{"Identifier":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestMultipleKeys()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<User>(json, JsonOptions);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.UserName);
                    Assert.AreEqual("JohnSmith3", deserialized.UserName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptions);
                    Assert.AreEqual("""{"Identifier":"JohnSmith3"}""", serialized);
                }
            }
        }

        public sealed class ThrowOnDuplicatesTests
        {
            [TestClass]
            public sealed class BasicTest
            {
                public sealed class User
                {
                    [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
                    public string? UserName { get; set; }
                }

                [TestMethod]
                public void TestSingleKey()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<User>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<User>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<User>(json3, JsonOptions);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.UserName);
                    Assert.AreEqual("JohnSmith", deserialized3.UserName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptions);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptions);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptions);
                    Assert.AreEqual("""{"UserName":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestMultipleKeys()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<User>(json, JsonOptions));
                }
            }

            [TestClass]
            public sealed class FirstNameSerializationTest
            {
                public sealed class User
                {
                    [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
                    public string? Identifier { get; set; }
                }

                [TestMethod]
                public void TestSingleKey()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<User>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized1.Identifier);

                    var deserialized2 = JsonSerializer.Deserialize<User>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized2.Identifier);

                    var deserialized3 = JsonSerializer.Deserialize<User>(json3, JsonOptions);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized3.Identifier);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptions);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptions);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptions);
                    Assert.AreEqual("""{"UserName":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestMultipleKeys()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<User>(json, JsonOptions));
                }
            }

            [TestClass]
            public sealed class DifferentNameSerializationTest
            {
                public sealed class User
                {
                    [JsonPropertyNames(throwOnDuplicate: true, serializationName: "Identifier", "UserName", "User", "Name")]
                    public string? UserName { get; set; }
                }

                [TestMethod]
                public void TestSingleKey()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<User>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<User>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<User>(json3, JsonOptions);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.UserName);
                    Assert.AreEqual("JohnSmith", deserialized3.UserName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptions);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptions);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptions);
                    Assert.AreEqual("""{"Identifier":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestMultipleKeys()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<User>(json, JsonOptions));
                }
            }
        }

        public sealed class JsonPropertyNameAttributeTests
        {
            [TestClass]
            public sealed class BasicTest
            {
                public sealed class User
                {
                    [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
                    [JsonPropertyName("UserName")]
                    public string? UserName { get; set; }
                }

                [TestMethod]
                public void TestThatItThrows()
                {
                    const string json = """{"UserName": "JohnSmith"}""";

                    Assert.ThrowsException<NotImplementedException>(() => JsonSerializer.Deserialize<User>(json, JsonOptions));
                }
            }
        }
    }
}
