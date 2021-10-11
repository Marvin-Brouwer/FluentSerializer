﻿using System.Xml.Linq;

using FluentSerializer.Core.NamingStrategies;

namespace FluentSerializer.Xml.Constants
{
    public static class XmlConstants
    {
        internal const string TextNodeDisplayTag = "#textNode";
        internal const string FragmentNodeDisplayTag = "__fragmentNode__";
        internal static readonly CustomNamingStrategy TextNodeNamingStrategy = new CustomNamingStrategy(TextNodeDisplayTag);
    }
}