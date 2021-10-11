using Ardalis.GuardClauses;
using FluentSerializer.Xml.Converters.Base;
using System;
using System.Globalization;

namespace FluentSerializer.Xml.Converters
{

    public class DateByFormatConverter : SimpleStructConverter<DateTime>
    {
        private readonly string _format;
        private readonly CultureInfo _cultureInfo;
        private readonly DateTimeStyles _dateTimeStyle;

        public DateByFormatConverter(string format, CultureInfo cultureInfo, DateTimeStyles dateTimeStyle)
        {
            Guard.Against.NullOrWhiteSpace(format, nameof(format));
            Guard.Against.Null(cultureInfo, nameof(cultureInfo));
            Guard.Against.Null(dateTimeStyle, nameof(dateTimeStyle));

            _format = format;
            _cultureInfo = cultureInfo;
            _dateTimeStyle = dateTimeStyle;
        }

        protected override DateTime ConvertToDataType(string value) => DateTime.ParseExact(value, _format, _cultureInfo, _dateTimeStyle);
        protected override string ConvertToString(DateTime value) => value.ToString(_format, _cultureInfo);
    }
}