# SingleOrArrayJsonConverter

[![NuGet Badge](https://badgen.net/nuget/v/SingleOrArrayJsonConverter)](https://www.nuget.org/packages/SingleOrArrayJsonConverter#versions-body-tab)
[![NuGet Badge](https://badgen.net/nuget/dt/SingleOrArrayJsonConverter)](https://www.nuget.org/stats/packages/SingleOrArrayJsonConverter?groupby=Version)

A JsonConverter that deserializes both a single JSON object and a JSON array as a C# array.

## Usage

- With the default JsonConverter

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

- With a custom JsonConverter

```cs
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

// Converts a Person object into a "${FirstName} ${LastName}" string
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

public sealed class Example
{
  [JsonConverter(typeof(SingleOrArrayJsonConverter<PersonJsonConverter>))]
  public string[]? Array { get; set; }
}
```

```cs
const string jsonSingle = """{"Array": {"FirstName": "John", "LastName": "Smith"}}""";
var deserializedSingle = JsonSerializer.Deserialize<Example>(json);
Console.WriteLine($"{deserializedSingle.Array.Length}: {deserializedSingle.Array[0]}");
// Output: "1: John Smith"

const string jsonArray = """{"Array": [{"FirstName": "John", "LastName": "Smith"}, {"FirstName": "John", "LastName": "Doe"}]}""";
var deserializedArray = JsonSerializer.Deserialize<Example>(json);
Console.WriteLine($"{deserializedArray.Array.Length}: {deserializedArray.Array[0]}, {deserializedArray.Array[1]}");
// Output: "2: John Smith, John Doe"
```
