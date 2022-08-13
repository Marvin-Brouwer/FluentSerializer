[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="/docs/logo/Logo.default.optimized.svg" />
	<a href="/docs/help/basic-concepts/Naming-strategies.md#readme">
		Documentation: Naming strategies
	</a>
</h1>

[//]: # (Body)

A naming strategy is a strategy that determines the serialized name of a property or class.  
This name is then used to map back and from serialized data.  

## Configuring a naming strategy  
  
To configure a naming strategy you need to reference a `Func<INamingStrategy>` or a method group returning `INamingStrategy` this has a couple of reasons.

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
				namingStrategy: Names.Use.SnakeCase)
			.Property(entity => entity.Name);
	}
}
```

Or you want to statically map a property mapping to a given string value:

```csharp
public sealed class ExampleProfile : JsonSerializerProfile
{
	protected override void Configure()
	{
		For<SomeDataEntity>()
			.Property(entity => entity.Id,
				namingStrategy: Names.Equal("identifier"))
			.Property(entity => entity.Name);
	}
}
```

Out of the box you can use the following naming strategies:

- Hard coded strings `Names.Equal("identifier")`
- CamelCase: `Names.Use.CamelCase`
- KebabCase: `Names.Use.KebabCase`
- LowerCase: `Names.Use.LowerCase`
- PascalCase: `Names.Use.PascalCase`
- SnakeCase: `Names.Use.SnakeCase`

## Creating a custom naming strategy

Let's just say you want to shout at your api.

If you want all your properties to be UpperCase you can do the following:

```csharp
/// <summary>
/// SHOUTS all properties <br />
/// <example>
/// SomeName => SOMENAME
/// </example>
/// </summary>
public sealed class UpperCaseNamingStrategy : INamingStrategy
{
	/// <inheritdoc />
	public ReadOnlySpan<char> GetName(in PropertyInfo property, in Type propertyType, in INamingContext _) => GetName(property.Name);

	/// <inheritdoc />
	public ReadOnlySpan<char> GetName(in Type classType, in INamingContext _) => GetName(classType.Name);

	private ReadOnlySpan<char> GetName(in string name)
	{
		var genericIndex = name.IndexOf(NamingConstants.GenericTypeMarker);
		if (genericIndex == -1) return name.ToUpperInvariant();

		Span<char> nameSpan = stackalloc char[genericIndex];
		name.AsSpan()[..genericIndex].ToUpperInvariant(nameSpan);

		return nameSpan.ToString();
	}
}
```

Then you create an extension method to expose this:

```csharp
public static class NamingExtensions
{
	private static readonly INamingStrategy UpperCaseNamingStrategy = new UpperCaseNamingStrategy();

	/// <inheritdoc cref="Example.StringBitBooleanConverter" />
	public static INamingStrategy UpperCase (this IUseNamingStrategies _) => UpperCaseNamingStrategy;
}
```

> **Note:**  
> Keep in mind that when you store a naming strategy like this (a static readonly instance),  
> the converter will not be thread-safe in the sense that instance members are shared across threads.

And now you can use it on properties or your configuration by calling `Names.Use.UpperCase`.

For a more real-world example checkout the [OpenAir use-case's CustomFieldNamingStrategy](/src/FluentSerializer.UseCase.OpenAir/Serializer/NamingStrategies/CustomFieldNamingStrategy.cs) together with [their NamingExtensions](/src/FluentSerializer.UseCase.OpenAir/Serializer/NamingStrategies/NamingExtensions.cs).  
This setup allows you register a naming strategy with  
both `Names.Use.CustomFieldName` for automated `{fieldName}__c` where `{fieldName}` uses the `CamelCaseNamingStrategy`  
and `Names.Use.CustomFieldName({value})` for `{value}__c`.

### INamingContext

The naming context passed to naming strategies allows you to lookup registered naming strategies  
either by `Type` to get the strategy for the type's name,  
or `Type` with `Property` to get the strategy for that property on the corresponding type mapping.  
  
For example this can be useful in a scenario where you need your name to include the naming strategy of a generic subtype.  
This is illustrated in the [OpenAir use-case's ResponseTypeNamingStrategy](/src/FluentSerializer.UseCase.OpenAir/Serializer/NamingStrategies/ResponseTypeNamingStrategy.cs), here we need the name of the generic `Data` property as an attribute on it's `<Request>` node.

## Naming strategy lifetime

It is generally a good idea to register your naming strategy as a static readonly instance since it only manipulates input and output.  
However if you need a service to determine a name for any reason you can do this by providing a `Func<INamingStrategy>` in either the registration of the DI setup or on a property. The profiles themselves have access to services via the DI framework.  
  
If this is a scenario you need please create an issue for us to write some documentation in the [Advanced concepts](https://github.com/Marvin-Brouwer/FluentSerializer#advanced-concepts) section.
