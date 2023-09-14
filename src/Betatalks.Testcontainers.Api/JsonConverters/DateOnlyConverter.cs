using System.Globalization;
using Newtonsoft.Json;

namespace Betatalks.Testcontainers.Api.JsonConverters;

public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        writer?.WriteValue(value.ToString("O", CultureInfo.InvariantCulture));
    }

    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var value = (reader?.Value) ?? throw new ArgumentNullException(nameof(reader));
        var valueType = value.GetType();
        if (valueType == typeof(DateTimeOffset))
        {
            return DateOnly.FromDateTime(((DateTimeOffset)value).DateTime);
        }
        else if (valueType == typeof(string))
        {
            return DateOnly.Parse((string)value, CultureInfo.InvariantCulture);
        }

        if (valueType == typeof(DateTime))
        {
            return DateOnly.FromDateTime((DateTime)value);
        }

        throw new NotSupportedException($"The type {valueType} cannot be parsed into DateTime.");
    }
}
