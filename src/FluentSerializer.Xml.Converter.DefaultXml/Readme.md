[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="https://github.com/Marvin-Brouwer/FluentSerializer/raw/main/docs/logo/Logo.xml.optimized.svg" />
	<a href="https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/src/FluentSerializer.Xml.Converter.DefaultXml#readme">
		FluentSerializer.Xml.Converter.DefaultXml
	</a>
</h1>

[//]: # (Body)

This library contains an `IXmlConverter` that is capable to convert to and from the XML data models used by `FluentSerializer.Xml` internally.

This can be useful for the following scenarios:

- You need comments in your document
- You need a feature that is out of the library's scope
- You need a feature that is currently in development to generate your structure, and you can't wait

## Usage

To use this feature you need to install a separate NuGet package:

```txt
dotnet add package FluentSerializer.Xml.Converter.DefaultXml
```

Next you need to register as converter for it (or reference it on a property mapping).

```csharp
using FluentSerializer.Xml.Converter.DefaultXml.Extensions;

serviceCollection.AddFluentXmlSerializer<TAssemblyMarker>(static configuration =>
{
	configuration.DefaultConverters.Use(Converter.For.Xml());
});
```

Then on your property mapping you can just map properties of `IXmlNode`'s implementations and use the `FluentSerializer.Xml.XmlBuilder` class to fill the values in code.
Options available:

- `FluentSerializer.Xml.DataNodes.IXmlElement`
- `FluentSerializer.Xml.DataNodes.IXmlAttribute`
- `FluentSerializer.Xml.DataNodes.IXmlText`
- `FluentSerializer.Xml.DataNodes.IXmlComment`

### Example

Consider this model:

```csharp
public sealed class ExampleModel {

	public IXmlElement ExampleProperty { get; set; }
}
```

When the converter is registered in the default mappers, and the model is mapped like this:

```csharp
public sealed class ExampleProfile : XmlSerializerProfile
{
	protected override void Configure()
	{
		For<ExampleModel>()
			.Element(example => example.ExampleProperty);
	}
}
```

You can set the values like this:

```csharp
using static FluentSerializer.Xml.XmlBuilder;

private void ExampleWorkaround(ExampleModel model)
{
	var customDataStructure = Element(
		Comment("Comments can be used now!"),
		Attribute("someBoolean", "true"),
		Element("someText", Text("text"))
	)
}
```

And the result will generate this:

```xml
<ExampleModel>
	<exampleProperty
		someBoolean="true">
		<!-- Comments can be used now! -->
		<someText>text</someText>
	</exampleProperty>
</ExampleModel>
```

**Be advised** these data structures are very bare bone. Even though the interface structure tries to prevent you from passing nodes you shouldn't you are building the data structure yourself so be careful.  
It's recommended to always start from the `IXmlElement` when using this technique.
