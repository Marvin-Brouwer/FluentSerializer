using System;
using System.Collections.Generic;
using FluentSerializer.Core.Configuration;

namespace FluentSerializer.Core.Mapping;

public interface IClassMapCollection
{
	IReadOnlyList<INewClassMap> GetAllClassMaps();
	INewClassMap? GetClassMapFor(in Type type, in SerializerDirection direction);
}