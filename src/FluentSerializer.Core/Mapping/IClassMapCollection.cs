using FluentSerializer.Core.Configuration;

using System;
using System.Collections.Generic;

namespace FluentSerializer.Core.Mapping;

public interface IClassMapCollection
{
	IReadOnlyList<INewClassMap> GetAllClassMaps();
	INewClassMap? GetClassMapFor(in Type type, in SerializerDirection direction);
}