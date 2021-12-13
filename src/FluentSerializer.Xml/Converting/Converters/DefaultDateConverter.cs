using System;
using System.Globalization;
using FluentSerializer.Xml.Converting.Converters.Base;

namespace FluentSerializer.Xml.Converting.Converters
{
    public class DefaultDateConverter : SimpleTypeConverter<DateTime>
    {
        protected override DateTime ConvertToDataType(string currentValue) => DateTime.Parse(currentValue, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault);
        protected override string ConvertToString(DateTime value) => value.ToString(CultureInfo.CurrentCulture);
    }
}