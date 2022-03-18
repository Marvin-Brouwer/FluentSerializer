[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="https://github.com/Marvin-Brouwer/FluentSerializer/raw/main/docs/logo/Logo.default.optimized.svg" />
	<a href="https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/docs/help/basic-concepts/Converters-EnumConverter.md#readme">
		Documentation: Converters, EnumConverter
	</a>
</h1>

[//]: # (Body)

To support every flavour of `enum` the library ships with a specialized converter for these `enum`s.  
It can be converted by replacing the default version by calling an extension method on the `Configuration.DefaultConverters` or like any convert by calling `Converter.For.Enum` / `Converter.For.Enum(EnumFormat format)`.

```csharp
serviceCollection.AddFluentJsonSerializer<TAssemblyMarker>(static configuration =>
{
	configuration.DefaultConverters.UseEnum(EnumFormat.UseName);
})
```

The `EnumFormat` has the following options:  

- **`UseDescription`**: Use the description attribute to parse and write values.
- **`UseName`**: Use the member name to parse and write values.
- **`UseNumberValue`**: Use the member name to parse and write values.
- **`Default`**: For simplicity sake the `EnumFormat` provides a `Default` option, including all of the above combined.

When these settings result in no values, null is returned to the serializer to either assign or throw.

Since this is a `Flag enum`, you can combine these in whatever way you like and the converter can handle it.  
However, it will always use the follow this quantity order: `Description > UseName > UseNumberValue`;
This is straight forward for both serializing and deserializing. However, when serializing this effectively has the same result as `UseDescription | UseName` since it will always have a member name.  

In addition to the `EnumFormat` some converters may have additional configuration.  
For example, the JSON serializer allows you to specify whether you'd like the output of the number to be a string `"0"` instead of a number `0`:

```csharp
serviceCollection.AddFluentJsonSerializer<TAssemblyMarker>(static configuration =>
{
	configuration.DefaultConverters.UseEnum(EnumFormat.UseNumberValue, true);
})
```
