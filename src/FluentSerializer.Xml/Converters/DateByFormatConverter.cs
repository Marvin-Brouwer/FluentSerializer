using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Context;
using FluentSerializer.Core.Services;
using System;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Converters
{

    public class DateByFormatConverter : PrimitiveConverter<DateTime>
    {
        private readonly string _format;
        private readonly CultureInfo _cultureInfo;
        private readonly DateTimeStyles _dateTimeStyle;

        public DateByFormatConverter(string format, CultureInfo cultureInfo, DateTimeStyles dateTimeStyle)
        {
            _format = format;
            _cultureInfo = cultureInfo;
            _dateTimeStyle = dateTimeStyle;
        }

        protected override DateTime ConvertToDataType(string value) => DateTime.ParseExact(value, _format, _cultureInfo, _dateTimeStyle);
        protected override string ConvertToString(DateTime value) => value.ToString(_format, _cultureInfo);
    }
}