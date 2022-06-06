using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Profiles;
using System;

namespace FluentSerializer.Core.Mapping;

/// <inheritdoc />
public interface IClassMapScanList<TSerializerProfile, TConfiguration> : IScanList<(Type type, SerializerDirection direction), IClassMap>
	where TSerializerProfile : ISerializerProfile<TConfiguration>
	where TConfiguration : ISerializerConfiguration
{
}