﻿using FluentSerializer.Core.Services;
using System.Xml.Linq;

namespace FluentSerializer.Xml.Profiles
{
    public interface ICustomAttributeConverter : IConverter<XAttribute>
    {
    }
}
