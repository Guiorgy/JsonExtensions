# SingleOrArrayJsonConverter

[![NuGet Badge](https://buildstats.info/nuget/SingleOrArrayJsonConverter)](https://www.nuget.org/packages/SingleOrArrayJsonConverter/)

A JsonConverter that deserializes both a single JSON object and a JSON array as a C# array

## Usage

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
