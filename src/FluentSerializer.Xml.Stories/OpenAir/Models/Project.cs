﻿using FluentSerializer.Xml.Stories.OpenAir;
using System;

namespace FluentSerializer.Xml.Tests
{
    internal sealed class Project : IOpenAirEntity
    {
        public string Id { get; set; } = string.Empty;
        public string? Name { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool Active { get; set; }
        public int? RateCardId { get; set; }
        public DateTime? CustomDate { get; set; }
    }
}
