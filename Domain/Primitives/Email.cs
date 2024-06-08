using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Domain.Primitives;

public readonly record struct Email
{
    private string value { get; }

    public Email(string email)
    {
        Validation.BasedOn(errors =>
        {
            if (string.IsNullOrEmpty(email))
            {
                errors.Add("Email cannot be empty");
            }
            else if (!Regex.IsMatch(email,@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
            {
                errors.Add("Email must be valid");
            }
        });
        value = email;
    }
    
    public override string ToString()
    {
        return value;
    }

    public static implicit operator string(Email email) => email.value;
    
    public static implicit operator Email(string email) => new(email);
}

public class EmailConverter : JsonConverter<Email>
{
    public override void WriteJson(JsonWriter writer, Email value, JsonSerializer serializer)
    {
        JValue.CreateString(value!.ToString()).WriteTo(writer);
    }

    public override Email ReadJson(JsonReader reader, Type objectType, Email existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        return reader.Value!.ToString()!;
    }
}