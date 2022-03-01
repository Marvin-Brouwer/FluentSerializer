using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Profiles;
using System;

namespace FluentSerializer.Core.Mapping;

/// <inheritdoc />
public interface IClassMapScanList<TSerializerProfile> : IScanList<(Type type, SerializerDirection direction), IClassMap>
	where TSerializerProfile : ISerializerProfile
{
}