# JsonMultiNameModifier

[![NuGet Badge](https://buildstats.info/nuget/JsonMultiNameModifier)](https://www.nuget.org/packages/JsonMultiNameModifier/)

A System.Text.Json JsonPropertyNames attribute and JsonMultiNameModifier that allows mapping of multiple JSON keys to one C# property.

## Usage

- Allowing duplicate keys

```cs
public sealed class User
{
  [JsonPropertyNames("UserName", "User", "Name")]
  public string UserName { get; set; }
}
```

```cs
JsonSerializerOptions jsonOptions = new()
{
  TypeInfoResolver = new DefaultJsonTypeInfoResolver()
  {
    Modifiers = { Modifiers.JsonMultiNameModifier }
  }
}

const string json1 = """{"UserName": "JohnSmith1"}""";
const string json2 = """{"User": "JohnSmith2"}""";
const string json3 = """{"Name": "JohnSmith3"}""";
var deserialized = JsonSerializer.Deserialize<User>(json1, jsonOptions);
Console.WriteLine(deserialized.UserName);
// Output: "JohnSmith1"
deserialized = JsonSerializer.Deserialize<User>(json2, jsonOptions);
Console.WriteLine(deserialized.UserName);
// Output: "JohnSmith2"
deserialized = JsonSerializer.Deserialize<User>(json3, jsonOptions);
Console.WriteLine(deserialized.UserName);
// Output: "JohnSmith3"

string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";
deserialized = JsonSerializer.Deserialize<User>(json, jsonOptions);
Console.WriteLine(deserialized.UserName);
// Output: "JohnSmith3"

json = JsonSerializer.Serialize(deserialized, jsonOptions);
Console.WriteLine(json);
// Output: '{"UserName":"JohnSmith3"}'
```

- Disallowing duplicate keys

```cs
public sealed class User
{
  [JsonPropertyNames(throwOnDuplicate: true, "UserName", "User", "Name")]
  public string UserName { get; set; }
}
```

```cs
JsonSerializerOptions jsonOptions = new()
{
  TypeInfoResolver = new DefaultJsonTypeInfoResolver()
  {
    Modifiers = { Modifiers.JsonMultiNameModifier }
  }
}

const string json1 = """{"UserName": "JohnSmith1"}""";
const string json2 = """{"User": "JohnSmith2"}""";
const string json3 = """{"Name": "JohnSmith3"}""";
var deserialized = JsonSerializer.Deserialize<User>(json1, jsonOptions);
Console.WriteLine(deserialized.UserName);
// Output: "JohnSmith1"
deserialized = JsonSerializer.Deserialize<User>(json2, jsonOptions);
Console.WriteLine(deserialized.UserName);
// Output: "JohnSmith2"
deserialized = JsonSerializer.Deserialize<User>(json3, jsonOptions);
Console.WriteLine(deserialized.UserName);
// Output: "JohnSmith3"

try {
  const string json = """{"UserName": "JohnSmith1", "User": "JohnSmith2", "Name": "JohnSmith3"}""";
  deserialized = JsonSerializer.Deserialize<User>(json, jsonOptions);
  Console.WriteLine(deserialized.UserName);
} catch (JsonException) {
  Console.WriteLine("Deserialize failed");
}
// Output: "Deserialize failed"
```

- Different serialization key

```cs
public sealed class User
{
  [JsonPropertyNames(serializationName: "NickName", "UserName", "User", "Name")]
  public string UserName { get; set; }
}
```

```cs
JsonSerializerOptions jsonOptions = new()
{
  TypeInfoResolver = new DefaultJsonTypeInfoResolver()
  {
    Modifiers = { Modifiers.JsonMultiNameModifier }
  }
}

string json = """{"UserName": "JohnSmith"}""";
var deserialized = JsonSerializer.Deserialize<User>(json, jsonOptions);
Console.WriteLine(deserialized.UserName);
// Output: "JohnSmith"

json = JsonSerializer.Serialize(deserialized, jsonOptions);
Console.WriteLine(json);
// Output: '{"NickName":"JohnSmith"}'
```
