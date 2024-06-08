using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Domain.Primitives;

public readonly record struct Name
{
    private string value { get; }

    public Name(string name)
    {
        Validation.BasedOn(errors =>
        {
            if (string.IsNullOrEmpty(name))
            {
                errors.Add("User name cannot be empty");
            }
        });
        value = name;
    }
    
    public override string ToString()
    {
        return value;
    }

    public static implicit operator string(Name name) => name.value;
    
    public static implicit operator Name(string name) => new(name);
}

public class NameConverter : JsonConverter<Name>
{
    public override void WriteJson(JsonWriter writer, Name value, JsonSerializer serializer)
    {
        JValue.CreateString(value!.ToString()).WriteTo(writer);
    }

    public override Name ReadJson(JsonReader reader, Type objectType, Name existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        return reader.Value!.ToString()!;
    }
}