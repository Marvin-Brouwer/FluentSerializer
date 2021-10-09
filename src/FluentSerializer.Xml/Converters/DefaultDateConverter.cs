using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Services;
using System;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Converters
{
    public class DefaultDateConverter : PrimitiveConverter<DateTime>
    {
        protected override DateTime ConvertToDataType(string value) => DateTime.Parse(value, CultureInfo.CurrentCulture, DateTimeStyles.NoCurrentDateDefault);
        protected override string ConvertToString(DateTime value) => value.ToString(CultureInfo.CurrentCulture);
    }
}