using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Messaging;

public static class JsonSerializerOptionsDefault
{

    public static JsonSerializerOptions Default =>
        new()
        {
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

}