[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="/docs/logo/Logo.default.optimized.svg" />
	<a href="/docs/help/basic-concepts/Converters.md#readme">
		Documentation: Converters
	</a>
</h1>

[//]: # (Body)

A converter is class that determines the way C# classes get converted to and from the data nodes that are part of the Serializer's format.  
Every serializer has it's own interface on top on or more `IConverter<TSerialContainer, TDataNode>` where TSerial container is a DataNode that extends `IDataNode`.  

- In the case of JSON, this is `IJsonConverter` using `IJsonNode`.
- In the case of XML, this is `IXmlConverter<TDataNode>` where `TDataNode` is an `IXmlNode` like `IXmlAttribute`, `IXmlElement` or `IXmlText`
- And so forth.

## Configuring a converter  
  
To configure a custom converter you need to reference a `Func<I{Format}Converter>`, a method group returning `I{Format}Converter` - or a specific one that matches the property type when applicable - this has a couple of reasons.

- It allows for type safe registration
- It allows for a readable and extendable solution
- It allows for a lifetime management solution outside of the DI framework.

The places where you can configure these strategies is both on the serializer's configuration when you register it, or on a property itself.
For example, you want a property to use snake case:

```csharp
public sealed class ExampleProfile : JsonSerializerProfile
{
	protected override void Configure()
	{
		For<SomeDataEntity>()
			.Property(entity => entity.Id,
				converter: Converters.Use.StringGuidConverter)
			.Property(entity => entity.Name);
	}
}
```

Every converter has a check to determine if it fits the given datatype and/or direction.  
If the converter does not match, the serializer will throw an exception.  
  
If you don't specify a property overload the serializer will lookup a fitting converter in the configuration's `DefaultConverters`.  
However if it finds no suitable converters it will throw an exception.

Out of the box you can expect the following converters to be present:

- DateTime: `Converters.Use.DateTime` (Using the default DateTime.Parse)
- DateTime with patterns: `Converters.Use.DateTime("pattern")` (Using DateTime.ParseExact)
- Collections: `Converters.Use.Collection` 
  (In some data formats you have different ways to do collections, for example XML has a built-in alternative to not wrap the collection)

You don't need to specify these Converters they come out of the box.

## Creating a custom converter

Let's just say you have a boolean represented by a `0` and a `1`, you'd like to just use booleans in code but the API gives you bits and expects bits from you.  
This is not rare, but rare enough to not be included out of the box.

```csharp
/// <summary>
/// Depicts booleans as 0 and 1 <br />
/// <example>
/// true => 1,
/// false => 0,
/// 1 => false,
/// 0 => true
/// </example>
/// </summary>
public sealed class StringBitBooleanConverter : IJsonConverter
{
	/// <inheritdoc />
	public SerializerDirection Direction { get; } = SerializerDirection.Both;
	/// <inheritdoc />
	public bool CanConvert(in Type targetType) => typeof(bool).IsAssignableFrom(targetType) 
											   || typeof(bool?).IsAssignableFrom(targetType);

	private static string ConvertToString(in bool currentValue) => currentValue ? "1" : "0";
	private static bool? ConvertToBool(in string? currentValue, in bool? defaultValue)
	{
		if (string.IsNullOrWhiteSpace(currentValue)) return defaultValue;
		if (currentValue.Equals("1", StringComparison.OrdinalIgnoreCase)) return true;
		if (currentValue.Equals("0", StringComparison.OrdinalIgnoreCase)) return false;

		throw new NotSupportedException($"A value of '{currentValue}' is not supported");
	}

	/// <inheritdoc />
	public object? Deserialize(in IJsonNode objectToDeserialize, in ISerializerContext context)
	{
		if (objectToDeserialize is not IJsonValue valueToDeserialize)
			throw new NotSupportedException($"The json object you attempted to deserialize was not a value");

		var defaultValue = context.Property.IsNullable() ? default(bool?) : default(bool);
		return ConvertToBool(valueToDeserialize.Value, defaultValue);
	}

	/// <inheritdoc />
	IJsonNode? Serialize(in object objectToSerialize, in ISerializerContext context)
	{
		if (objectToSerialize is not bool booleanToSerialize)
			throw new NotSupportedException($"Type '{objectToSerialize.GetType().FullName}' is not a boolean");

		var stringValue = ConvertToString(in booleanToSerialize);
		return Value(in stringValue);
	}
}
```

Then you create an extension method to expose this:

```csharp
public static class ConverterExtensions
{
	private static readonly IConverter StringBitBooleanConverter = new StringBitBooleanConverter();

	/// <inheritdoc cref="Example.StringBitBooleanConverter" />
	public static INamingStrategy StringBitBoolean(this IUseJsonConverters _) => StringBitBooleanConverter;
}
```

> **Note:**  
> Keep in mind that when you store a converter like this (a static readonly instance),  
> the converter will not be thread-safe in the sense that instance members are shared across threads.

And now you can use it on properties or your configuration by calling `Converter.Use.StringBitBoolean`.

For a more real-world example checkout the [OpenAir use-case's StringBitBooleanConverter](/src/FluentSerializer.UseCase.OpenAir/Serializer/Converters/StringBitBooleanConverter.cs) together with [their NamingExtensions](/src/FluentSerializer.UseCase.OpenAir/Serializer/Converters/ConverterExtensions.cs).  
This setup is for XML so it shows an example of using a naming strategy in a custom converter.

### ISerializerContext

[naming-strategy]: /docs/help/basic-concepts/Naming-strategies.md#inamingstrategy
 
The serializer context passed to converters is a container holding essential information for converting custom data models.  
It contains access to the Property being serialized, the type of that Property The class type of the type holding the property, the naming strategy assigned to the property, a reference to the current serializer.  
  
It also implements the `INamingStrategy` ([discussed here][naming-strategy]) with an additional overload that accepts just a type for the class map currently being converted.  
  
## Converter lifetime

It is generally a good idea to register your converter as a static readonly instance since it only manipulates input and output.  
However if you need a service for any reason you can do this by providing a `Func<I{Format}Converter>` in either the registration of the DI setup or on a property. The profiles themselves have access to services via the DI framework.  
  
If this is a scenario you need please create an issue for us to write some documentation in the [Advanced concepts](https://github.com/Marvin-Brouwer/FluentSerializer#advanced-concepts) section.

## Converter identification

In some specific scenarios you may need to override a converter in the configuration's `DefaultConverters`, for example when picking a different converter for `IEnumerable` types.  
By default the `IConverter` interface has a default implementation simply looking at the `object.GetHashCode()` to identify uniqueness when configuring.  
All the converters that are shipped with this library simply calculate the HashCode of the type it's supposed to serialize.  
So when building a custom collection converter you can override the existing by overriding the `GetHashCode` method:

```csharp
/// <inheritdoc />
public override int GetHashCode() => typeof(IEnumerable).GetHashCode();
```

When using that, you can simply register it like normal, and the `IConfigurationStack` will replace the existing collection converter:  

```csharp
serviceCollection.AddFluentJsonSerializer<TAssemblyMarker>(static configuration =>
{
	configuration.DefaultConverters.UseEnum(EnumFormat.UseNumberValue, true);
})
```
