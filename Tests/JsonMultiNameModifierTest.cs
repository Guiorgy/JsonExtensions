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

        public static readonly JsonSerializerOptions JsonOptionsIncludeFields = new()
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            {
                Modifiers = { JsonMultiNameModifier }
            },
            IncludeFields = true
        };

        public sealed class AllowDuplicatesTests
        {
            [TestClass]
            public sealed class BasicTest
            {
                public sealed class UserField
                {
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public string? userName;
                }

                public sealed class UserProperty
                {
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public string? UserName { get; set; }
                }

                public sealed class UserConstructorField
                {
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public readonly string userName;

                    [JsonConstructor]
                    public UserConstructorField(string userName)
                    {
                        this.userName = userName;
                    }
                }

                public sealed class UserConstructorModifiedField
                {
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public readonly string userName;

                    [JsonConstructor]
                    public UserConstructorModifiedField(string? userName)
                    {
                        this.userName = userName?.ToLower() ?? "";
                    }
                }

                public sealed class UserConstructorPropertyNoSetter
                {
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public string UserName { get; }

                    [JsonConstructor]
                    public UserConstructorPropertyNoSetter(string userName)
                    {
                        UserName = userName;
                    }
                }

                public sealed class UserConstructorPropertyNoBackingField
                {
                    private readonly string userName;

                    [JsonPropertyNames("UserName", "User", "Name")]
                    public string UserName => userName + "";

                    [JsonConstructor]
                    public UserConstructorPropertyNoBackingField(string userName)
                    {
                        this.userName = userName;
                    }
                }

                [TestMethod]
                public void TestSingleKeyField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.userName);
                    Assert.AreEqual("JohnSmith", deserialized1.userName);

                    var deserialized2 = JsonSerializer.Deserialize<UserField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.userName);
                    Assert.AreEqual("JohnSmith", deserialized2.userName);

                    var deserialized3 = JsonSerializer.Deserialize<UserField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.userName);
                    Assert.AreEqual("JohnSmith", deserialized3.userName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyProperty()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserProperty>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<UserProperty>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<UserProperty>(json3, JsonOptions);
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
                public void TestSingleKeyConstructorField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.userName);
                    Assert.AreEqual("JohnSmith", deserialized1.userName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.userName);
                    Assert.AreEqual("JohnSmith", deserialized2.userName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.userName);
                    Assert.AreEqual("JohnSmith", deserialized3.userName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyConstructorModifiedField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.userName);
                    Assert.AreEqual("johnsmith", deserialized1.userName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.userName);
                    Assert.AreEqual("johnsmith", deserialized2.userName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.userName);
                    Assert.AreEqual("johnsmith", deserialized3.userName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"johnsmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyConstructorPropertyNoSetter()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json3, JsonOptions);
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
                public void TestSingleKeyConstructorPropertyNoBackingField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json3, JsonOptions);
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
                public void TestMultipleKeysField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserField>(json, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.userName);
                    Assert.AreEqual("JohnSmith3", deserialized.userName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysProperty()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserProperty>(json, JsonOptions);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.UserName);
                    Assert.AreEqual("JohnSmith3", deserialized.UserName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptions);
                    Assert.AreEqual("""{"UserName":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorField>(json, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.userName);
                    Assert.AreEqual("JohnSmith3", deserialized.userName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorModifiedField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorModifiedField>(json, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.userName);
                    Assert.AreEqual("johnsmith3", deserialized.userName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"johnsmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorPropertyNoSetter()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json, JsonOptions);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.UserName);
                    Assert.AreEqual("JohnSmith3", deserialized.UserName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptions);
                    Assert.AreEqual("""{"UserName":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorPropertyNoBackingField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json, JsonOptions);
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
                public sealed class UserField
                {
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public string? identifier;
                }

                public sealed class UserProperty
                {
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public string? Identifier { get; set; }
                }

                public sealed class UserConstructorField
                {
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public readonly string identifier;

                    [JsonConstructor]
                    public UserConstructorField(string identifier)
                    {
                        this.identifier = identifier;
                    }
                }

                public sealed class UserConstructorModifiedField
                {
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public readonly string identifier;

                    [JsonConstructor]
                    public UserConstructorModifiedField(string? identifier)
                    {
                        this.identifier = identifier?.ToLower() ?? "";
                    }
                }

                public sealed class UserConstructorPropertyNoSetter
                {
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public string Identifier { get; }

                    [JsonConstructor]
                    public UserConstructorPropertyNoSetter(string identifier)
                    {
                        Identifier = identifier;
                    }
                }

                public sealed class UserConstructorPropertyNoBackingField
                {
                    private readonly string identifier;

                    [JsonPropertyNames("UserName", "User", "Name")]
                    public string Identifier => identifier + "";

                    [JsonConstructor]
                    public UserConstructorPropertyNoBackingField(string identifier)
                    {
                        this.identifier = identifier;
                    }
                }

                [TestMethod]
                public void TestSingleKeyField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.identifier);
                    Assert.AreEqual("JohnSmith", deserialized1.identifier);

                    var deserialized2 = JsonSerializer.Deserialize<UserField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.identifier);
                    Assert.AreEqual("JohnSmith", deserialized2.identifier);

                    var deserialized3 = JsonSerializer.Deserialize<UserField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.identifier);
                    Assert.AreEqual("JohnSmith", deserialized3.identifier);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyProperty()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserProperty>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized1.Identifier);

                    var deserialized2 = JsonSerializer.Deserialize<UserProperty>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized2.Identifier);

                    var deserialized3 = JsonSerializer.Deserialize<UserProperty>(json3, JsonOptions);
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
                public void TestSingleKeyConstructorField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.identifier);
                    Assert.AreEqual("JohnSmith", deserialized1.identifier);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.identifier);
                    Assert.AreEqual("JohnSmith", deserialized2.identifier);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.identifier);
                    Assert.AreEqual("JohnSmith", deserialized3.identifier);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyConstructorModifiedField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.identifier);
                    Assert.AreEqual("johnsmith", deserialized1.identifier);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.identifier);
                    Assert.AreEqual("johnsmith", deserialized2.identifier);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.identifier);
                    Assert.AreEqual("johnsmith", deserialized3.identifier);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"johnsmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyConstructorPropertyNoSetter()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized1.Identifier);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized2.Identifier);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json3, JsonOptions);
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
                public void TestSingleKeyConstructorPropertyNoBackingField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized1.Identifier);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized2.Identifier);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json3, JsonOptions);
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
                public void TestMultipleKeysField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserField>(json, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.identifier);
                    Assert.AreEqual("JohnSmith3", deserialized.identifier);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysProperty()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserProperty>(json, JsonOptions);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.Identifier);
                    Assert.AreEqual("JohnSmith3", deserialized.Identifier);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptions);
                    Assert.AreEqual("""{"UserName":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorField>(json, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.identifier);
                    Assert.AreEqual("JohnSmith3", deserialized.identifier);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorModifiedField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorModifiedField>(json, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.identifier);
                    Assert.AreEqual("johnsmith3", deserialized.identifier);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"johnsmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorPropertyNoSetter()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json, JsonOptions);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.Identifier);
                    Assert.AreEqual("JohnSmith3", deserialized.Identifier);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptions);
                    Assert.AreEqual("""{"UserName":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorPropertyNoBackingField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json, JsonOptions);
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
                public sealed class UserField
                {
                    [JsonPropertyNames(serializationName: "Identifier", "UserName", "User", "Name")]
                    public string? userName;
                }

                public sealed class UserProperty
                {
                    [JsonPropertyNames(serializationName: "Identifier", "UserName", "User", "Name")]
                    public string? UserName { get; set; }
                }

                public sealed class UserConstructorField
                {
                    [JsonPropertyNames(serializationName: "Identifier", "UserName", "User", "Name")]
                    public readonly string userName;

                    [JsonConstructor]
                    public UserConstructorField(string userName)
                    {
                        this.userName = userName;
                    }
                }

                public sealed class UserConstructorModifiedField
                {
                    [JsonPropertyNames(serializationName: "Identifier", "UserName", "User", "Name")]
                    public readonly string userName;

                    [JsonConstructor]
                    public UserConstructorModifiedField(string? userName)
                    {
                        this.userName = userName?.ToLower() ?? "";
                    }
                }

                public sealed class UserConstructorPropertyNoSetter
                {
                    [JsonPropertyNames(serializationName: "Identifier", "UserName", "User", "Name")]
                    public string UserName { get; }

                    [JsonConstructor]
                    public UserConstructorPropertyNoSetter(string userName)
                    {
                        UserName = userName;
                    }
                }

                public sealed class UserConstructorPropertyNoBackingField
                {
                    private readonly string userName;

                    [JsonPropertyNames(serializationName: "Identifier", "UserName", "User", "Name")]
                    public string UserName => userName + "";

                    [JsonConstructor]
                    public UserConstructorPropertyNoBackingField(string userName)
                    {
                        this.userName = userName;
                    }
                }

                [TestMethod]
                public void TestSingleKeyField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.userName);
                    Assert.AreEqual("JohnSmith", deserialized1.userName);

                    var deserialized2 = JsonSerializer.Deserialize<UserField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.userName);
                    Assert.AreEqual("JohnSmith", deserialized2.userName);

                    var deserialized3 = JsonSerializer.Deserialize<UserField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.userName);
                    Assert.AreEqual("JohnSmith", deserialized3.userName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"Identifier":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyProperty()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserProperty>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<UserProperty>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<UserProperty>(json3, JsonOptions);
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
                public void TestSingleKeyConstructorField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.userName);
                    Assert.AreEqual("JohnSmith", deserialized1.userName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.userName);
                    Assert.AreEqual("JohnSmith", deserialized2.userName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.userName);
                    Assert.AreEqual("JohnSmith", deserialized3.userName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"Identifier":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyConstructorModifiedField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.userName);
                    Assert.AreEqual("johnsmith", deserialized1.userName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.userName);
                    Assert.AreEqual("johnsmith", deserialized2.userName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.userName);
                    Assert.AreEqual("johnsmith", deserialized3.userName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"Identifier":"johnsmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyConstructorPropertyNoSetter()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json3, JsonOptions);
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
                public void TestSingleKeyConstructorPropertyNoBackingField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json3, JsonOptions);
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
                public void TestMultipleKeysField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserField>(json, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.userName);
                    Assert.AreEqual("JohnSmith3", deserialized.userName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"Identifier":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysProperty()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserProperty>(json, JsonOptions);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.UserName);
                    Assert.AreEqual("JohnSmith3", deserialized.UserName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptions);
                    Assert.AreEqual("""{"Identifier":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorField>(json, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.userName);
                    Assert.AreEqual("JohnSmith3", deserialized.userName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"Identifier":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorModifiedField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorModifiedField>(json, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.userName);
                    Assert.AreEqual("johnsmith3", deserialized.userName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"Identifier":"johnsmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorPropertyNoSetter()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json, JsonOptions);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.UserName);
                    Assert.AreEqual("JohnSmith3", deserialized.UserName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptions);
                    Assert.AreEqual("""{"Identifier":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorPropertyNoBackingField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json, JsonOptions);
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
                public sealed class UserField
                {
                    [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
                    public string? userName;
                }

                public sealed class UserProperty
                {
                    [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
                    public string? UserName { get; set; }
                }

                public sealed class UserConstructorField
                {
                    [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
                    public readonly string userName;

                    [JsonConstructor]
                    public UserConstructorField(string userName)
                    {
                        this.userName = userName;
                    }
                }

                public sealed class UserConstructorModifiedField
                {
                    [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
                    public readonly string userName;

                    [JsonConstructor]
                    public UserConstructorModifiedField(string? userName)
                    {
                        this.userName = userName?.ToLower() ?? "";
                    }
                }

                public sealed class UserConstructorPropertyNoSetter
                {
                    [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
                    public string UserName { get; }

                    [JsonConstructor]
                    public UserConstructorPropertyNoSetter(string userName)
                    {
                        UserName = userName;
                    }
                }

                public sealed class UserConstructorPropertyNoBackingField
                {
                    private readonly string userName;

                    [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
                    public string UserName => userName + "";

                    [JsonConstructor]
                    public UserConstructorPropertyNoBackingField(string userName)
                    {
                        this.userName = userName;
                    }
                }

                [TestMethod]
                public void TestSingleKeyField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.userName);
                    Assert.AreEqual("JohnSmith", deserialized1.userName);

                    var deserialized2 = JsonSerializer.Deserialize<UserField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.userName);
                    Assert.AreEqual("JohnSmith", deserialized2.userName);

                    var deserialized3 = JsonSerializer.Deserialize<UserField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.userName);
                    Assert.AreEqual("JohnSmith", deserialized3.userName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyProperty()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserProperty>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<UserProperty>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<UserProperty>(json3, JsonOptions);
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
                public void TestSingleKeyConstructorField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.userName);
                    Assert.AreEqual("JohnSmith", deserialized1.userName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.userName);
                    Assert.AreEqual("JohnSmith", deserialized2.userName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.userName);
                    Assert.AreEqual("JohnSmith", deserialized3.userName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyConstructorModifiedField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.userName);
                    Assert.AreEqual("johnsmith", deserialized1.userName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.userName);
                    Assert.AreEqual("johnsmith", deserialized2.userName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.userName);
                    Assert.AreEqual("johnsmith", deserialized3.userName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"johnsmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyConstructorPropertyNoSetter()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json3, JsonOptions);
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
                public void TestSingleKeyConstructorPropertyNoBackingField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json3, JsonOptions);
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
                public void TestMultipleKeysField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserField>(json, JsonOptionsIncludeFields));
                }

                [TestMethod]
                public void TestMultipleKeysProperty()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserProperty>(json, JsonOptions));
                }

                [TestMethod]
                public void TestMultipleKeysConstructorField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserConstructorField>(json, JsonOptionsIncludeFields));
                }

                [TestMethod]
                public void TestMultipleKeysConstructorModifiedField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserConstructorModifiedField>(json, JsonOptionsIncludeFields));
                }

                [TestMethod]
                public void TestMultipleKeysConstructorPropertyNoSetter()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json, JsonOptions));
                }

                [TestMethod]
                public void TestMultipleKeysConstructorPropertyNoBackingField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json, JsonOptions));
                }
            }

            [TestClass]
            public sealed class FirstNameSerializationTest
            {
                public sealed class UserField
                {
                    [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
                    public string? identifier;
                }

                public sealed class UserProperty
                {
                    [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
                    public string? Identifier { get; set; }
                }

                public sealed class UserConstructorField
                {
                    [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
                    public readonly string identifier;

                    [JsonConstructor]
                    public UserConstructorField(string identifier)
                    {
                        this.identifier = identifier;
                    }
                }

                public sealed class UserConstructorModifiedField
                {
                    [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
                    public readonly string identifier;

                    [JsonConstructor]
                    public UserConstructorModifiedField(string? identifier)
                    {
                        this.identifier = identifier?.ToLower() ?? "";
                    }
                }

                public sealed class UserConstructorPropertyNoSetter
                {
                    [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
                    public string Identifier { get; }

                    [JsonConstructor]
                    public UserConstructorPropertyNoSetter(string identifier)
                    {
                        Identifier = identifier;
                    }
                }

                public sealed class UserConstructorPropertyNoBackingField
                {
                    private readonly string identifier;

                    [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
                    public string Identifier => identifier + "";

                    [JsonConstructor]
                    public UserConstructorPropertyNoBackingField(string identifier)
                    {
                        this.identifier = identifier;
                    }
                }

                [TestMethod]
                public void TestSingleKeyField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.identifier);
                    Assert.AreEqual("JohnSmith", deserialized1.identifier);

                    var deserialized2 = JsonSerializer.Deserialize<UserField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.identifier);
                    Assert.AreEqual("JohnSmith", deserialized2.identifier);

                    var deserialized3 = JsonSerializer.Deserialize<UserField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.identifier);
                    Assert.AreEqual("JohnSmith", deserialized3.identifier);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyProperty()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserProperty>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized1.Identifier);

                    var deserialized2 = JsonSerializer.Deserialize<UserProperty>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized2.Identifier);

                    var deserialized3 = JsonSerializer.Deserialize<UserProperty>(json3, JsonOptions);
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
                public void TestSingleKeyConstructorField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.identifier);
                    Assert.AreEqual("JohnSmith", deserialized1.identifier);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.identifier);
                    Assert.AreEqual("JohnSmith", deserialized2.identifier);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.identifier);
                    Assert.AreEqual("JohnSmith", deserialized3.identifier);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyConstructorModifiedField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.identifier);
                    Assert.AreEqual("johnsmith", deserialized1.identifier);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.identifier);
                    Assert.AreEqual("johnsmith", deserialized2.identifier);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.identifier);
                    Assert.AreEqual("johnsmith", deserialized3.identifier);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"johnsmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyConstructorPropertyNoSetter()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized1.Identifier);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized2.Identifier);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json3, JsonOptions);
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
                public void TestSingleKeyConstructorPropertyNoBackingField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized1.Identifier);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.Identifier);
                    Assert.AreEqual("JohnSmith", deserialized2.Identifier);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json3, JsonOptions);
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
                public void TestMultipleKeysField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserField>(json, JsonOptionsIncludeFields));
                }

                [TestMethod]
                public void TestMultipleKeysProperty()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserProperty>(json, JsonOptions));
                }

                [TestMethod]
                public void TestMultipleKeysConstructorField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserConstructorField>(json, JsonOptionsIncludeFields));
                }

                [TestMethod]
                public void TestMultipleKeysConstructorModifiedField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserConstructorModifiedField>(json, JsonOptionsIncludeFields));
                }

                [TestMethod]
                public void TestMultipleKeysConstructorPropertyNoSetter()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json, JsonOptions));
                }

                [TestMethod]
                public void TestMultipleKeysConstructorPropertyNoBackingField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json, JsonOptions));
                }
            }

            [TestClass]
            public sealed class DifferentNameSerializationTest
            {
                public sealed class UserField
                {
                    [JsonPropertyNames(throwOnDuplicate: true, serializationName: "Identifier", "UserName", "User", "Name")]
                    public string? userName;
                }

                public sealed class UserProperty
                {
                    [JsonPropertyNames(throwOnDuplicate: true, serializationName: "Identifier", "UserName", "User", "Name")]
                    public string? UserName { get; set; }
                }

                public sealed class UserConstructorField
                {
                    [JsonPropertyNames(throwOnDuplicate: true, serializationName: "Identifier", "UserName", "User", "Name")]
                    public readonly string userName;

                    [JsonConstructor]
                    public UserConstructorField(string userName)
                    {
                        this.userName = userName;
                    }
                }

                public sealed class UserConstructorModifiedField
                {
                    [JsonPropertyNames(throwOnDuplicate: true, serializationName: "Identifier", "UserName", "User", "Name")]
                    public readonly string userName;

                    [JsonConstructor]
                    public UserConstructorModifiedField(string? userName)
                    {
                        this.userName = userName?.ToLower() ?? "";
                    }
                }

                public sealed class UserConstructorPropertyNoSetter
                {
                    [JsonPropertyNames(throwOnDuplicate: true, serializationName: "Identifier", "UserName", "User", "Name")]
                    public string UserName { get; }

                    [JsonConstructor]
                    public UserConstructorPropertyNoSetter(string userName)
                    {
                        UserName = userName;
                    }
                }

                public sealed class UserConstructorPropertyNoBackingField
                {
                    private readonly string userName;

                    [JsonPropertyNames(throwOnDuplicate: true, serializationName: "Identifier", "UserName", "User", "Name")]
                    public string UserName => userName + "";

                    [JsonConstructor]
                    public UserConstructorPropertyNoBackingField(string userName)
                    {
                        this.userName = userName;
                    }
                }

                [TestMethod]
                public void TestSingleKeyField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.userName);
                    Assert.AreEqual("JohnSmith", deserialized1.userName);

                    var deserialized2 = JsonSerializer.Deserialize<UserField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.userName);
                    Assert.AreEqual("JohnSmith", deserialized2.userName);

                    var deserialized3 = JsonSerializer.Deserialize<UserField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.userName);
                    Assert.AreEqual("JohnSmith", deserialized3.userName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"Identifier":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyProperty()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserProperty>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<UserProperty>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<UserProperty>(json3, JsonOptions);
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
                public void TestSingleKeyConstructorField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.userName);
                    Assert.AreEqual("JohnSmith", deserialized1.userName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.userName);
                    Assert.AreEqual("JohnSmith", deserialized2.userName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.userName);
                    Assert.AreEqual("JohnSmith", deserialized3.userName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"Identifier":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyConstructorModifiedField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.userName);
                    Assert.AreEqual("johnsmith", deserialized1.userName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.userName);
                    Assert.AreEqual("johnsmith", deserialized2.userName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.userName);
                    Assert.AreEqual("johnsmith", deserialized3.userName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"Identifier":"johnsmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyConstructorPropertyNoSetter()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json3, JsonOptions);
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
                public void TestSingleKeyConstructorPropertyNoBackingField()
                {
                    const string json1 = """{"UserName": "JohnSmith"}""";
                    const string json2 = """{"User": "JohnSmith"}""";
                    const string json3 = """{"Name": "JohnSmith"}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json3, JsonOptions);
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
                public void TestMultipleKeysField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserField>(json, JsonOptionsIncludeFields));
                }

                [TestMethod]
                public void TestMultipleKeysProperty()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserProperty>(json, JsonOptions));
                }

                [TestMethod]
                public void TestMultipleKeysConstructorField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserConstructorField>(json, JsonOptionsIncludeFields));
                }

                [TestMethod]
                public void TestMultipleKeysConstructorModifiedField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserConstructorModifiedField>(json, JsonOptionsIncludeFields));
                }

                [TestMethod]
                public void TestMultipleKeysConstructorPropertyNoSetter()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json, JsonOptions));
                }

                [TestMethod]
                public void TestMultipleKeysConstructorPropertyNoBackingField()
                {
                    const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";

                    Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json, JsonOptions));
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

        public sealed class CustomConverterTests
        {
            public sealed class TrimJsonConverter : JsonConverter<string>
            {
                public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                {
                    var converter = (JsonConverter<string>)options.GetConverter(typeof(string));

                    string? str = converter.Read(ref reader, typeof(string), options);

                    return str?.Trim();
                }

                public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
                {
                    var converter = (JsonConverter<string>)options.GetConverter(typeof(string));

                    converter.Write(writer, value, options);
                }
            }

            [TestClass]
            public sealed class BasicTest
            {
                public sealed class UserField
                {
                    [JsonConverter(typeof(TrimJsonConverter))]
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public string? userName;
                }

                public sealed class UserProperty
                {
                    [JsonConverter(typeof(TrimJsonConverter))]
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public string? UserName { get; set; }
                }

                public sealed class UserConstructorField
                {
                    [JsonConverter(typeof(TrimJsonConverter))]
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public readonly string userName;

                    [JsonConstructor]
                    public UserConstructorField(string userName)
                    {
                        this.userName = userName;
                    }
                }

                public sealed class UserConstructorModifiedField
                {
                    [JsonConverter(typeof(TrimJsonConverter))]
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public readonly string userName;

                    [JsonConstructor]
                    public UserConstructorModifiedField(string? userName)
                    {
                        this.userName = userName?.ToLower() ?? "";
                    }
                }

                public sealed class UserConstructorPropertyNoSetter
                {
                    [JsonConverter(typeof(TrimJsonConverter))]
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public string UserName { get; }

                    [JsonConstructor]
                    public UserConstructorPropertyNoSetter(string userName)
                    {
                        UserName = userName;
                    }
                }

                public sealed class UserConstructorPropertyNoBackingField
                {
                    private readonly string userName;

                    [JsonConverter(typeof(TrimJsonConverter))]
                    [JsonPropertyNames("UserName", "User", "Name")]
                    public string UserName => userName + "";

                    [JsonConstructor]
                    public UserConstructorPropertyNoBackingField(string userName)
                    {
                        this.userName = userName;
                    }
                }

                [TestMethod]
                public void TestSingleKeyField()
                {
                    const string json1 = """{"UserName": "  JohnSmith  "}""";
                    const string json2 = """{"User": "  JohnSmith  "}""";
                    const string json3 = """{"Name": "  JohnSmith  "}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.userName);
                    Assert.AreEqual("JohnSmith", deserialized1.userName);

                    var deserialized2 = JsonSerializer.Deserialize<UserField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.userName);
                    Assert.AreEqual("JohnSmith", deserialized2.userName);

                    var deserialized3 = JsonSerializer.Deserialize<UserField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.userName);
                    Assert.AreEqual("JohnSmith", deserialized3.userName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyProperty()
                {
                    const string json1 = """{"UserName": "  JohnSmith  "}""";
                    const string json2 = """{"User": "  JohnSmith  "}""";
                    const string json3 = """{"Name": "  JohnSmith  "}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserProperty>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<UserProperty>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<UserProperty>(json3, JsonOptions);
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
                public void TestSingleKeyConstructorField()
                {
                    const string json1 = """{"UserName": "  JohnSmith  "}""";
                    const string json2 = """{"User": "  JohnSmith  "}""";
                    const string json3 = """{"Name": "  JohnSmith  "}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.userName);
                    Assert.AreEqual("JohnSmith", deserialized1.userName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.userName);
                    Assert.AreEqual("JohnSmith", deserialized2.userName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.userName);
                    Assert.AreEqual("JohnSmith", deserialized3.userName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyConstructorModifiedField()
                {
                    const string json1 = """{"UserName": "  JohnSmith  "}""";
                    const string json2 = """{"User": "  JohnSmith  "}""";
                    const string json3 = """{"Name": "  JohnSmith  "}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json1, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.userName);
                    Assert.AreEqual("johnsmith", deserialized1.userName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json2, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.userName);
                    Assert.AreEqual("johnsmith", deserialized2.userName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorModifiedField>(json3, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized3);
                    Assert.IsNotNull(deserialized3.userName);
                    Assert.AreEqual("johnsmith", deserialized3.userName);

                    var serialized1 = JsonSerializer.Serialize(deserialized1, JsonOptionsIncludeFields);
                    var serialized2 = JsonSerializer.Serialize(deserialized2, JsonOptionsIncludeFields);
                    var serialized3 = JsonSerializer.Serialize(deserialized3, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"johnsmith"}""", serialized1);
                    Assert.AreEqual(serialized1, serialized2);
                    Assert.AreEqual(serialized2, serialized3);
                }

                [TestMethod]
                public void TestSingleKeyConstructorPropertyNoSetter()
                {
                    const string json1 = """{"UserName": "  JohnSmith  "}""";
                    const string json2 = """{"User": "  JohnSmith  "}""";
                    const string json3 = """{"Name": "  JohnSmith  "}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json3, JsonOptions);
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
                public void TestSingleKeyConstructorPropertyNoBackingField()
                {
                    const string json1 = """{"UserName": "  JohnSmith  "}""";
                    const string json2 = """{"User": "  JohnSmith  "}""";
                    const string json3 = """{"Name": "  JohnSmith  "}""";

                    var deserialized1 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json1, JsonOptions);
                    Assert.IsNotNull(deserialized1);
                    Assert.IsNotNull(deserialized1.UserName);
                    Assert.AreEqual("JohnSmith", deserialized1.UserName);

                    var deserialized2 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json2, JsonOptions);
                    Assert.IsNotNull(deserialized2);
                    Assert.IsNotNull(deserialized2.UserName);
                    Assert.AreEqual("JohnSmith", deserialized2.UserName);

                    var deserialized3 = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json3, JsonOptions);
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
                public void TestMultipleKeysField()
                {
                    const string json = """{"UserName": "  JohnSmith1  ", "User": "  JohnSmith2  ", "Name": "  JohnSmith3  "}""";

                    var deserialized = JsonSerializer.Deserialize<UserField>(json, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.userName);
                    Assert.AreEqual("JohnSmith3", deserialized.userName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysProperty()
                {
                    const string json = """{"UserName": "  JohnSmith1  ", "User": "  JohnSmith2  ", "Name": "  JohnSmith3  "}""";

                    var deserialized = JsonSerializer.Deserialize<UserProperty>(json, JsonOptions);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.UserName);
                    Assert.AreEqual("JohnSmith3", deserialized.UserName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptions);
                    Assert.AreEqual("""{"UserName":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorField()
                {
                    const string json = """{"UserName": "  JohnSmith1  ", "User": "  JohnSmith2  ", "Name": "  JohnSmith3  "}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorField>(json, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.userName);
                    Assert.AreEqual("JohnSmith3", deserialized.userName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorModifiedField()
                {
                    const string json = """{"UserName": "  JohnSmith1  ", "User": "  JohnSmith2  ", "Name": "  JohnSmith3  "}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorModifiedField>(json, JsonOptionsIncludeFields);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.userName);
                    Assert.AreEqual("johnsmith3", deserialized.userName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptionsIncludeFields);
                    Assert.AreEqual("""{"UserName":"johnsmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorPropertyNoSetter()
                {
                    const string json = """{"UserName": "  JohnSmith1  ", "User": "  JohnSmith2  ", "Name": "  JohnSmith3  "}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorPropertyNoSetter>(json, JsonOptions);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.UserName);
                    Assert.AreEqual("JohnSmith3", deserialized.UserName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptions);
                    Assert.AreEqual("""{"UserName":"JohnSmith3"}""", serialized);
                }

                [TestMethod]
                public void TestMultipleKeysConstructorPropertyNoBackingField()
                {
                    const string json = """{"UserName": "  JohnSmith1  ", "User": "  JohnSmith2  ", "Name": "  JohnSmith3  "}""";

                    var deserialized = JsonSerializer.Deserialize<UserConstructorPropertyNoBackingField>(json, JsonOptions);
                    Assert.IsNotNull(deserialized);
                    Assert.IsNotNull(deserialized.UserName);
                    Assert.AreEqual("JohnSmith3", deserialized.UserName);

                    var serialized = JsonSerializer.Serialize(deserialized, JsonOptions);
                    Assert.AreEqual("""{"UserName":"JohnSmith3"}""", serialized);
                }
            }
        }
    }
}
