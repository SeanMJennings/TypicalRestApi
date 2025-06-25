using Domain.Primitives;
using Newtonsoft.Json;

namespace Domain;

public static class Converters
{
    public static List<JsonConverter> GetConverters =>
    [
        new NameConverter(),
        new EmailConverter()
    ];
}