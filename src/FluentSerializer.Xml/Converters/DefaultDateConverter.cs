using FluentSerializer.Xml.Converters.Base;
using System;
using System.Globalization;

namespace FluentSerializer.Xml.Converters
{
    public class DefaultDateConverter : PrimitiveConverter<DateTime>
    {
        protected override DateTime ConvertToDataType(string value) => DateTime.Parse(value, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault);
        protected override string ConvertToString(DateTime value) => value.ToString(CultureInfo.CurrentCulture);
    }
}