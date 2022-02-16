[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="https://github.com/Marvin-Brouwer/FluentSerializer/raw/main/doc/logo/Logo.json.optimized.svg" />
	<a href="https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/src/FluentSerializer.Json.Converter.DefaultJson#readme">
		FluentSerializer.Json.Converter.DefaultJson
	</a>
</h1>

[//]: # (Body)

This library contains an `IJsonConverter` that is capable to convert to and from the JSON data models used by `FluentSerializer.Json` internally.

This can be useful for the following scenarios:

- You need comments in your document
- You need a feature that is out of the library's scope
- You need a feature that is currently in development to generate your structure, and you can't wait

## Usage

To use this feature you need to install a separate NuGet package:

```txt
dotnet add package FluentSerializer.Json.Converter.DefaultJson
```

Next you need to register as converter for it (or reference it on a property mapping).

```csharp
using FluentSerializer.Json.Converter.DefaultJson.Extensions;

serviceCollection.AddFluentJsonSerializer<TAssemblyMarker>(static configuration =>
{
	configuration.DefaultConverters.Add(Converter.For.Json());
});
```

Then on your property mapping you can just map properties of `IJsonNode`'s implementations and use the `FluentSerializer.Json.JsonBuilder` class to fill the values in code.
Options available:

- `FluentSerializer.Json.DataNodes.IJsonObject`
- `FluentSerializer.Json.DataNodes.IJsonArray`
- `FluentSerializer.Json.DataNodes.IJsonProperty`
- `FluentSerializer.Json.DataNodes.IJsonValue`
- `FluentSerializer.Json.DataNodes.IJsonValue`
- `FluentSerializer.Json.DataNodes.IJsonComment`

### Example

Consider this model:

```csharp
public sealed class ExampleModel {

	public IJsonObject ExampleProperty { get; set; }
}
```

When the converter is registered in the default mappers, and the model is mapped like this:

```csharp
public sealed class ExampleProfile : JsonSerializerProfile
{
	protected override void Configure()
	{
		For<ExampleModel>()
			.Property(example => example.ExampleProperty);
	}
}
```

You can set the values like this:

```csharp
using FluentSerializer.Json.Converter.DefaultJson.Extensions;

using static FluentSerializer.Json.JsonBuilder;

private void ExampleWorkaround(ExampleModel model)
{
	var customDataStructure = Object(
		Comment("Comments can be used now!"),
		Property("someBoolean", JsonValue("true")),
		Property("someText", JsonValue("text".WrapString())),
	)
}
```

And the result will generate this:

```jsonc
{
	"exampleProperty": {
		// Comments can be used now!
		"someBoolean": true,
		"someText": "text"
	}
}
```

**Be advised** these data structures are very bare bone. Even though the interface structure tries to prevent you from passing nodes you shouldn't you are building the data structure yourself so be careful.  
It's recommended to always start from the `IJsonObject` or `IJsonArray` when using this technique.  
Also because of how we built the serializer there are no value types, everything is string so you'll have to wrap text with quotes yourself. Alternatively you can use the `WrapString()` extension method from `FluentSerializer.Json.Converter.DefaultJson.Extensions` as shown in the example above.
