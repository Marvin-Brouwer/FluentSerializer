﻿[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="/docs/logo/Logo.json.optimized.svg" />
	<a href="/src/FluentSerializer.Json#readme">
		FluentSerializer.Json
	</a>
</h1>

[//]: # (Body)

The `FluentSerializer.Json` library is responsible for exposing the JSON API and implementing the JSON conversion logic.

- [JSON spec](https://www.json.org/json-en.html)
- [JSONC spec](https://code.visualstudio.com/docs/languages/json#_json-with-comments)

## Configuration

[configuring-di]: /src/FluentSerializer.Json.DependencyInjection.NetCoreDefault#readme

It is possible to configure the defaults of certain aspects the serializer uses.
You can override these when [configuring the DI injection][configuring-di] or when using the `SerializerFactory.For.Json()`.

By default it looks like this:

- **Encoding** = `Encoding.UTF8`
- **FormatOutput** = true
- **WriteNull** = false
- **DefaultNamingStrategy** = camelCase
- **DefaultConverters**
  - Converter that can handle DateTime objects (JSON spec compliant)
  - Converter that can handle IConvertible types
  - Converter to handle collection types
  
### Using the factory

For basic usage you can use this:

```csharp
SerializerFactory.For
	.Json()
	.UseProfilesFromAssembly<TAssemblyMarker>();
```

This will use the `JsonSerializerConfiguration.Default` as the applied config.
The type parameter of `TAssemblyMarker` will be used to scan that assembly for the profiles associated with this serializer.
Alternatively there are overloads that accept a `System.Reflection.Assembly` variable.

There are multiple overloads, for changing configuration the lambda approach is recommended:

```csharp
SerializerFactory.For
	.Json(static configuration =>
	{
		// Change configuration values
		configuration.NewLine = LineEndings.LineFeed;
	})
	.UseProfilesFromAssembly<TAssemblyMarker>();
```

This will use a new instance of the `JsonSerializerConfiguration` as the applied config and allows you to change some properties.

## Creating profiles

For the serializer to understand how to map the data structure to and from C# Models, you need to create a profile.
To do so create a class inheriting from `FluentSerializer.Json.Profiles.JsonSerializerProfile`.
The profile needs to implement the `protected override void Configure()` method, which will be called to construct the mappings inside of this profile.

To create a class mapping, use the `For<TModel>()` method.
This method has the following optional parameters:  

- **direction:** The direction for which this class mapping is valid, defaults to `Both`
- **namingStrategy:** A naming strategy for all properties in the mapping, overriding the Configuration value
  See: [Basic concepts/Naming strategies](/docs/help/basic-concepts/Naming-strategies.md#readme)

You can create multiple class mappings per profile if that fits your use-case.

To map the properties of the C# Model, use method chaining on the `For<TModel>()` method.
Available options: **`Property<TProperty>()`**.

### Mapping properties

To create a property mapping, use the `Property<TProperty>()` method.  
This method has the following optional parameters:

- **direction:** The direction for which this property mapping is valid, defaults to the class mapping's value.
- **namingStrategy:** A naming strategy for this property mapping, overriding the Configuration value and the parents strategy
  See: [Basic concepts/Naming strategies](/docs/help/basic-concepts/Naming-strategies.md#readme)
- **converter:** A custom converter for this property mapping, overriding the logic that normally looks up a converter in the default converters
  See: [Basic concepts/Converters](/docs/help/basic-concepts/Converters.md#readme)

### Example

Here is a simple example to illustrate how a profile would be implemented:

```jsonc
{
	"data": [{
		"identifier": 1,
		"name": "someName",
		// Some other properties we don't map
	}]
}
```

```csharp
public sealed class Request<TDataEntity> where TDataEntity: IDataEntity {
	public List<TDataEntity> Data { get; set; }
}
```

```csharp
public sealed class SomeDataEntity: IDataEntity {
	public string Id { get; set; }
	public string Name { get; set; }
}
```

```csharp
public sealed class RequestProfile : JsonSerializerProfile
{
	protected override void Configure()
	{
		For<Request<IDataEntity>>()
			.Property(request => request.Data);

		For<SomeDataEntity>()
			.Property(entity => entity.Id,
				namingStrategy: Names.Equal("identifier"))
			.Property(entity => entity.Name);
	}
}
```
