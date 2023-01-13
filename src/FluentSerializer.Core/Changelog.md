# Changelog

## @next

-

## v3.0.2

- [#8](https://github.com/Marvin-Brouwer/FluentSerializer/issues/8) Added `netframework` backwards compatibility

## v3.0.1

- [#146](https://github.com/Marvin-Brouwer/FluentSerializer/issues/146) Added `net7` support
- [#227](https://github.com/Marvin-Brouwer/FluentSerializer/issues/227) Added `IParsable` converter
- [#249](https://github.com/Marvin-Brouwer/FluentSerializer/issues/249) Added `IFormattable` converter
- [#230](https://github.com/Marvin-Brouwer/FluentSerializer/issues/230) Improved iteration speed

## v3.0.0

- [#91](https://github.com/Marvin-Brouwer/FluentSerializer/issues/91) Improved overall code quality
- [#93](https://github.com/Marvin-Brouwer/FluentSerializer/issues/93) Fixed `ConfigurationStack` overriding
- [#206](https://github.com/Marvin-Brouwer/FluentSerializer/issues/206) Moved `IDataNode` HashCode calculation to interface member
- [#206](https://github.com/Marvin-Brouwer/FluentSerializer/issues/206) Moved `IConverter` unique identifier from HashCode calculation to GUIDs
- [#206](https://github.com/Marvin-Brouwer/FluentSerializer/issues/206) Moved `DefaultConverter` uniqueness calculation to `IConverter.ConverterId`
- [#211](https://github.com/Marvin-Brouwer/FluentSerializer/issues/211) Internal Equality checks no longer rely on GetHashCode

## v2.2.0

- [#133](https://github.com/Marvin-Brouwer/FluentSerializer/issues/133) Improved performance

## v2.1.0

- [#157](https://github.com/Marvin-Brouwer/FluentSerializer/issues/157) Added base functionality for Serializer factories

## v2.0.0

- [#150](https://github.com/Marvin-Brouwer/FluentSerializer/issues/150) Added release notes
- [#127](https://github.com/Marvin-Brouwer/FluentSerializer/issues/127) Added `EnumMemberAttribute` support
- [#73](https://github.com/Marvin-Brouwer/FluentSerializer/issues/73) Added constants for Date and Time formats
- [#101](https://github.com/Marvin-Brouwer/FluentSerializer/issues/101) Added support for reference loop detection

## v1.0.0

- Initial release
