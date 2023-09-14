using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Betatalks.Testcontainers.Infrastructure.Database.ValueConverters;

public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter() : base(
        dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
        dateTime => DateOnly.FromDateTime(dateTime)
    )
    {
    }
}
