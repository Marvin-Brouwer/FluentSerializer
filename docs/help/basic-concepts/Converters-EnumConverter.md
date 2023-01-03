[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="/docs/logo/Logo.default.optimized.svg" />
	<a href="/docs/help/basic-concepts/Converters-EnumConverter.md#readme">
		Documentation: Converters, EnumConverter
	</a>
</h1>

[//]: # (Body)

To support every flavour of `enum` the library ships with a specialized converter for these `enum`s.  
It can be configured by replacing the default version by calling an extension method on the `Configuration.DefaultConverters` or it can be used like any converter by calling `Converter.For.Enum` / `Converter.For.Enum(EnumFormat format)`.

```csharp
serviceCollection.AddFluentJsonSerializer<TAssemblyMarker>(static configuration =>
{
	configuration.DefaultConverters.UseEnum(EnumFormat.UseName);
})
```

The `EnumFormat` has the following options:  

- **`UseEnumMember`**: Use the value of the `EnumMember` attribute to parse and write values.
- **`UseDescription`**: Use the description attribute to parse and write values.
- **`UseName`**: Use the member name to parse and write values.
- **`UseNumberValue`**: Use the member name to parse and write values.

<br/>

- **`Default`**: For simplicity sake the `EnumFormat` provides a `Default` option, including all of the above combined.
- **`Simple`**: For simplicity sake the `EnumFormat` provides a `Simple` option to allow for the member name to read and write, read from underlying number as fallback.

When these settings result in no values, null is returned to the serializer to either assign or throw.

Since this is a `Flag enum`, you can combine these in whatever way you like and the converter can handle it.  
However, it will always use the follow this quantity order: `UseEnumMember > UseDescription > UseName > UseNumberValue`;
This is straight forward for both serializing and deserializing.  
However, when serializing `... UseName | UseNumberValue` effectively has the same result as `... UseName` since it will always have a member name.  

In addition to the `EnumFormat` some converters may have additional configuration.  
For example, the JSON serializer allows you to specify whether you'd like the output of the number to be a string `"0"` instead of a number `0`:

```csharp
serviceCollection.AddFluentJsonSerializer<TAssemblyMarker>(static configuration =>
{
	configuration.DefaultConverters.UseEnum(EnumFormat.UseNumberValue, true);
})
```
