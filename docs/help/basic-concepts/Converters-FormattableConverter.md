[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="/docs/logo/Logo.default.optimized.svg" />
	<a href="/docs/help/basic-concepts/Converters-FormattableConverter.md#readme">
		Documentation: Converters, FormattableConverter
	</a>
</h1>

[//]: # (Body)
[formattable-interface-doc]: https://learn.microsoft.com/en-us/dotnet/api/system.iformattable?view=net-7.0

To support the [`IFormattable` interface][formattable-interface-doc] the library ships with a specialized converter for the types implementing this interface. By default it will use `null` as the configured format string and use `CultureInfo.CurrentCulture` as the formatProvider.

It can be configured by replacing the default version by calling an extension method on the `Configuration.DefaultConverters` or it can be used like any converter by calling any of the following options:

- `Converter.For.Formattable`
- `Converter.For.Formattable(string formatString)`
- `Converter.For.Formattable(IFormatProvider formatProvider)`
- `Converter.For.Formattable(string formatString, IFormatProvider formatProvider)`

Overriding the default converter can be done one the `DefaultConverters` configuration with the same parameter options, like this:

```csharp
serviceCollection.AddFluentJsonSerializer<TAssemblyMarker>(static configuration =>
{
	configuration.DefaultConverters.UseFormattable("G");
})
```

When choosing to implement `IFormattable` vs `IConvertible` note that the `IFormattable` interface will take preference over the `IConvertible` interface when implementing both.  
However, the `IConvertible` converter is bi-directional while the `IFormattable` is serialize only.
