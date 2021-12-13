using System;
using System.Globalization;
using FluentSerializer.Json.Converting.Converters.Base;

namespace FluentSerializer.Json.Converting.Converters
{
    public class DefaultDateConverter : SimpleTypeConverter<DateTime>
    {
        protected override DateTime ConvertToDataType(string currentValue) => DateTime.Parse(currentValue, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault);
        protected override string ConvertToString(DateTime value) => value.ToString(CultureInfo.CurrentCulture);
    }
}