using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ardalis.GuardClauses;
using FluentSerializer.Core.Configuration;
using FluentSerializer.Core.Mapping;

namespace FluentSerializer.Core.Profiles
{
    public static class ProfileScanner
    {
        private static IEnumerable<ISerializerProfile> ScanAssembly(Assembly assembly) =>
            assembly.GetTypes()
                .Where(type => typeof(ISerializerProfile).IsAssignableFrom(type))
                .Where(type => !type.IsAbstract)
                .Select(type => (ISerializerProfile)Activator.CreateInstance(type)!);

        public static IScanList<(Type type, SerializerDirection direction), IClassMap> FindClassMapsInAssembly(Assembly assembly)
        {
            Guard.Against.Null(assembly, nameof(assembly));

            var profiles = ScanAssembly(assembly);

            return new ClassMapScanList(profiles.SelectMany(profile => profile.Configure()).ToList().AsReadOnly());
        }
    }
}
