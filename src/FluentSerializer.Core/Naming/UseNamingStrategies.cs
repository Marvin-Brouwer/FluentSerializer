using System.Runtime.CompilerServices;
using FluentSerializer.Core.Naming.NamingStrategies;

namespace FluentSerializer.Core.Naming
{
    internal struct UseNamingStrategies : IUseNamingStrategies
    {
        private static readonly CamelCaseNamingStrategy CamelCaseNamingStrategy = new CamelCaseNamingStrategy();
        private static readonly LowerCaseNamingStrategy LowerCaseNamingStrategy = new LowerCaseNamingStrategy();
        private static readonly PascalCaseNamingStrategy PascalCaseNamingStrategy = new PascalCaseNamingStrategy();
        private static readonly SnakeCaseNamingStrategy SnakeCaseNamingStrategy = new SnakeCaseNamingStrategy();
        private static readonly KebabCaseNamingStrategy KebabCaseNamingStrategy = new KebabCaseNamingStrategy();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public INamingStrategy CamelCase() => CamelCaseNamingStrategy;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public INamingStrategy LowerCase() => LowerCaseNamingStrategy;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public INamingStrategy PascalCase() => PascalCaseNamingStrategy;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public INamingStrategy SnakeCase() => SnakeCaseNamingStrategy;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public INamingStrategy KebabCase() => KebabCaseNamingStrategy;
    }
}