[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="https://github.com/Marvin-Brouwer/FluentSerializer/raw/main/doc/logo/Logo.default.optimized.svg" />
	<a href="https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/doc/help/basic-concepts/Naming-strategies.md#readme">
		Documentation: Naming strategies
	</a>
</h1>

[//]: # (Body)

A naming strategy is a strategy that determines the serialized name of a property or class.  
This name is then used to map back and from serialized data.  

## Configuring a naming strategy  
  
To configure a naming strategy you need to reference a `Func<INamingStrategy>` this has a couple of reasons.
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
			.Property(entity => entity.Id, ,
				namingStrategy: Names.Use.SnakeCase)
			.Property(entity => entity.Name);
	}
}
```
Of you want to statically map a property mapping to a given string value:
```csharp
public sealed class ExampleProfile : JsonSerializerProfile
{
	protected override void Configure()
	{
		For<SomeDataEntity>()
			.Property(entity => entity.Id, ,
				namingStrategy: Names.Are("identifier"))
			.Property(entity => entity.Name);
	}
}
```

Out of the box you can use the following naming strategies:
- Hard coded strings `Names.Are("identifier")`
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
public class UpperCaseNamingStrategy : INamingStrategy
{
	/// <inheritdoc />
	public string GetName(in PropertyInfo property, in INamingContext _) => property.Name.Split('`')[0].ToUpperInvariant();
	/// <inheritdoc />
	public string GetName(in Type classType, in INamingContext _) => classType.Name.Split('`')[0].ToUpperInvariant();
}
```
Then you create an extension method to expose this:
```csharp
public static class NamingExtensions
{
	private static readonly INamingStrategy UpperCaseNamingStrategy = new UpperCaseNamingStrategy()
	public static INamingStrategy UpperCase (this IUseNamingStrategies _) => UpperCaseNamingStrategy;
}
```
And now you can use it on properties or your configuration by calling `Names.Use.UpperCase`.

For a more real-world example checkout the [OpenAir use-case's CustomFieldNamingStrategy](https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/src/FluentSerializer.UseCase.OpenAir/Serializer/NamingStrategies/CustomFieldNamingStrategy.cs) together with [their NamingExtensions](https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/src/FluentSerializer.UseCase.OpenAir/Serializer/NamingStrategies/NamingExtensions.cs).  
This setup allows you register a naming strategy with  
both `Names.Use.CustomFieldName` for automated `{fieldName}__c` where `{fieldName}` uses the `CamelCaseNamingStrategy`  
and `Names.Use.CustomFieldName({value})` for `{value}__c`.

### INamingContext
 
TODO

## Naming strategy lifetime

It is generally a good idea to register your naming strategy as a static readonly instance since it only manipulates input and output.  
However if you need a service to determine a name for any reason you can do this by providing a `Func<INamingStrategy>` in either the registration of the DI setup or on a property. The profiles themselves have access to services via the DI framework.  
  
If this is a scenario you need please create an issue for us to write some documentation in the [Advanced concepts](https://github.com/Marvin-Brouwer/FluentSerializer#advanced-concepts) section.
