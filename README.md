# JsonExtensions

A collection of extension for .NET System.Text.Json

## SingleOrArrayJsonConverter

A JsonConverter that deserializes both a single JSON object and a JSON array as a C# array

```cs
public sealed class Example
{
  [JsonConverter(typeof(SingleOrArrayJsonConverter))]
  public string[]? Array { get; set; }
}
```

```cs
const string jsonSingle = """{"Array": "single"}""";
var deserializedSingle = JsonSerializer.Deserialize<Example>(jsonSingle);
Console.WriteLine($"{deserializedSingle.Array.Length}: {deserializedSingle.Array[0]}");
// Output: "1: single"

const string jsonArray = """{"Array": ["first", "second"]}""";
var deserializedArray = JsonSerializer.Deserialize<Example>(jsonArray);
Console.WriteLine($"{deserializedArray.Array.Length}: {deserializedArray.Array[0]}, {deserializedArray.Array[1]}");
// Output: "2: first, second"
```

## JsonMultiNameModifier

A JsonPropertyNames attribute and JsonMultiNameModifier that allows mapping of multiple JSON keys to one C# property.

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

## MIT License

Copyright (c) 2023 Guiorgy

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
