using Domain.Primitives;
using Newtonsoft.Json;

namespace Domain;

public static class Converters
{
    public static void AddConverters(IList<JsonConverter> converters)
    {
        converters.Add(new NameConverter());
        converters.Add(new EmailConverter());
    }
}