# Changelog

## @next

-

## v3.0.2

- [#8](https://github.com/Marvin-Brouwer/FluentSerializer/issues/8) Added `netframework` backwards compatibility

## v3.0.1

- [#146](https://github.com/Marvin-Brouwer/FluentSerializer/issues/146) Added `net7` support
- [#227](https://github.com/Marvin-Brouwer/FluentSerializer/issues/227) Added `IParsable` converter
- [#249](https://github.com/Marvin-Brouwer/FluentSerializer/issues/249) Added `IFormattable` converter

## v3.0.0

- [#91](https://github.com/Marvin-Brouwer/FluentSerializer/issues/91) Improved overall code quality
- [#93](https://github.com/Marvin-Brouwer/FluentSerializer/issues/93) Fixed `ConfigurationStack` overriding
- [#93](https://github.com/Marvin-Brouwer/FluentSerializer/issues/93) Simple type converters now allow empty strings to pass through
- [#93](https://github.com/Marvin-Brouwer/FluentSerializer/issues/93) Date and Time structures now fail on empty strings
- [#93](https://github.com/Marvin-Brouwer/FluentSerializer/issues/93) Incomplete JsonValues now fail Gracefully
- [#206](https://github.com/Marvin-Brouwer/FluentSerializer/issues/206) Moved `IDataNode` HashCode calculation to interface member
- [#206](https://github.com/Marvin-Brouwer/FluentSerializer/issues/206) Moved `IConverter` unique identifier from HashCode calculation to GUIDs
- [#206](https://github.com/Marvin-Brouwer/FluentSerializer/issues/206) Moved `DefaultConverter` uniqueness calculation to `IConverter.ConverterId`
- [#211](https://github.com/Marvin-Brouwer/FluentSerializer/issues/211) Internal Equality checks no longer rely on GetHashCode

## v2.2.0

- [#133](https://github.com/Marvin-Brouwer/FluentSerializer/issues/133) Improved performance
- [#81](https://github.com/Marvin-Brouwer/FluentSerializer/issues/81) Improved some whitespace handling

## v2.1.0

- [#157](https://github.com/Marvin-Brouwer/FluentSerializer/issues/157) Added `JsonSerializerFactory`

## v2.0.0

- [#150](https://github.com/Marvin-Brouwer/FluentSerializer/issues/150) Added release notes
- [#73](https://github.com/Marvin-Brouwer/FluentSerializer/issues/73) Added support for `DateTimeOffset`, `DateOnly`, `TimeOnly` and `TimeSpan`
- [#101](https://github.com/Marvin-Brouwer/FluentSerializer/issues/101) Added reference loop detection

## v1.0.0

- Initial release
