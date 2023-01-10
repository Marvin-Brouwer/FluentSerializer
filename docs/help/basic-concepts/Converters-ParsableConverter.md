[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="/docs/logo/Logo.default.optimized.svg" />
	<a href="/docs/help/basic-concepts/Converters-ParsableConverter.md#readme">
		Documentation: Converters, ParsableConverter
	</a>
</h1>

[//]: # (Body)
[parsable-interface-doc]: https://learn.microsoft.com/en-us/dotnet/api/system.iparsable-1?view=net-7.0

> **Note:** This converter is only available for `net7` and up.  
> Older versions will use the `ConvertibleConverter` instead.

To support the new [`IParsable<TSelf>` interface][parsable-interface-doc] the library ships with a specialized converter for the types implementing this interface. By default it will use `Parse` and throw `FormatException`s, as well as use `CultureInfo.CurrentCulture` as the formatProvider.

It can be configured by replacing the default version by calling an extension method on the `Configuration.DefaultConverters` or it can be used like any converter by calling any of the following options:

- `Converter.For.Parsable`
- `Converter.For.Parsable(bool useTryParse)` _technically you can use `false` here, but that would be the same as `Converter.For.Parsable`_
- `Converter.For.Parsable(IFormatProvider formatProvider)`
- `Converter.For.Parsable(bool useTryParse, IFormatProvider formatProvider)` _technically you can use `false` here, but that would be the same as `Converter.For.Parsable(IFormatProvider formatProvider)`_

Overriding the default converter can be done one the `DefaultConverters` configuration with the same parameter options, like this:

```csharp
serviceCollection.AddFluentJsonSerializer<TAssemblyMarker>(static configuration =>
{
	configuration.DefaultConverters.UseParsable(true);
})
```

When choosing to implement `IParsable<TSelf>` vs `IConvertible` note that the `IParsable<TSelf>` interface will take preference over the `IConvertible` interface when implementing both.  
However, the `IConvertible` converter is bi-directional while the `IParsable<TSelf>` is deserialize only.
