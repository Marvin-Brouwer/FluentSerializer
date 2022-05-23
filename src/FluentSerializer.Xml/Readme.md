[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="/docs/logo/Logo.xml.optimized.svg" />
	<a href="/src/FluentSerializer.Xml#readme">
		FluentSerializer.Xml
	</a>
</h1>

[//]: # (Body)

The `FluentSerializer.Xml` library is responsible for exposing the XML API and implementing the XML conversion logic.

- [XML spec](https://www.w3.org/TR/xml)

## Configuration

[configuring-di]: /src/FluentSerializer.Xml.DependencyInjection.NetCoreDefault#readme

It is possible to configure the defaults of certain aspects the serializer uses.
You can override these when [configuring the DI injection][configuring-di].

By default it looks like this:

- **Encoding** = `Encoding.Unicode` (utf-16)
- **FormatOutput** = true
- **WriteNull** = false
- **DefaultClassNamingStrategy** = PascalCase
- **DefaultPropertyNamingStrategy** = camelCase
- **DefaultConverters**
  - Converter that can handle DateTime objects (XML spec compliant)
  - Converter that can handle IConvertible types
  - Converter to handle collection types (wrapped XML collection)
  
## Creating profiles

For the serializer to understand how to map the data structure to and from C# Models, you need to create a profile.  
To do so create a class inheriting from `FluentSerializer.Xml.Profiles.XmlSerializerProfile`.  
The profile needs to implement the `protected override void Configure()` method, which will be called to construct the mappings inside of this profile.  
  
To create a class mapping, use the `For<TModel>()` method.  
This method has the following optional parameters:

- **direction:** The direction for which this class mapping is valid, defaults to `Both`
- **tagNamingStrategy:** A naming strategy for all property to element mappings, overriding the Configuration value  
  See: [Basic concepts/Naming strategies](/docs/help/basic-concepts/Naming-strategies.md#readme)  
- **attributeNamingStrategy:** A naming strategy for all property to attribute mappings, overriding the Configuration value  
  See: [Basic concepts/Naming strategies](/docs/help/basic-concepts/Naming-strategies.md#readme)  

You can create multiple class mappings per profile if that fits your use-case.
  
To map the properties of the C# Model, use method chaining on the `For<TModel>()` method.  
Available options: **`Attribute<TAttribute>()`**, **`Child<TElement>()`**, **`Text<TText>()`**.

### Mapping properties

To create a property to attribute mapping, use the `Attribute<TAttribute>()` method.  
This method has the following optional parameters:

- **direction:** The direction for which this property mapping is valid, defaults to the class mapping's value.
- **namingStrategy:** A naming strategy for this property mapping, overriding the Configuration value and the parents strategy  
  See: [Basic concepts/Naming strategies](/docs/help/basic-concepts/Naming-strategies.md#readme)  
- **converter:** A custom converter for this property mapping, overriding the logic that normally looks up a converter in the default converters  
  See: [Basic concepts/Converters](/docs/help/basic-concepts/Converters.md#readme)  

To create a property to element mapping, use the `Child<TElement>()` method.  
This method has the following optional parameters:

- **direction:** The direction for which this property mapping is valid, defaults to the class mapping's value.
- **namingStrategy:** A naming strategy for this property mapping, overriding the Configuration value and the parents strategy  
  See: [Basic concepts/Naming strategies](/docs/help/basic-concepts/Naming-strategies.md#readme)  
- **converter:** A custom converter for this property mapping, overriding the logic that normally looks up a converter in the default converters  
  See: [Basic concepts/Converters](/docs/help/basic-concepts/Converters.md#readme)  

To create a property to element mapping, use the `Text<TText>()` method.  
This method has the following optional parameters:

- **direction:** The direction for which this property mapping is valid, defaults to the class mapping's value.
- **converter:** A custom converter for this property mapping, overriding the logic that normally looks up a converter in the default converters  
  See: [Basic concepts/Converters](/docs/help/basic-concepts/Converters.md#readme)  

The text nodes don't have names, so this mapping has no **namingStrategy** parameter.  

### Example

Here is a simple example to illustrate how a profile would be implemented:

```xml
<Request>
	<data>
		<List>
			<SomeDataEntity identifier="1">
				<name>someName</name>
				<!-- Some other properties we don't map -->
			</SomeDataEntity>
		</List>
	<data>
</Request>
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
			.Child(request => request.Data);
		
		For<SomeDataEntity>()
			.Attribute(entity => entity.Id,
				namingStrategy: Names.Equal("identifier"))
			.Child(entity => entity.Name);
	}
}
```
