using System.Data;

namespace CodingTracker.Data;

internal class TimeSpanHandler : SqlMapper.TypeHandler<TimeSpan>
{
    public override TimeSpan Parse(object value)
    {
        if (value is string stringValue)
        {
            return TimeSpan.Parse(stringValue);
        }
        return TimeSpan.Zero;
    }

    public override void SetValue(IDbDataParameter parameter, TimeSpan value)
    {
        parameter.Value = value.ToString();
    }
}
