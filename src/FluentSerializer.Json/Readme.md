[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="https://github.com/Marvin-Brouwer/FluentSerializer/raw/main/doc/logo/Logo.json.optimized.svg" />
	<a href="https://github.com/Marvin-Brouwer/FluentSerializer/src/FluentSerializer.Json/Readme.md#readme">
		FluentSerializer.Json
	</a>
</h1>

[//]: # (Body)

This library is responsible for exposing the JSON API and implementing the JSON conversion logic.

See: 
- [JSON spec](https://www.json.org/json-en.html)
- [JSONC spec](https://code.visualstudio.com/docs/languages/json#_json-with-comments)

## Configuration

TODO: Section on the configuration, link to DI package or other way around.

## Creating profiles

For the serializer to understand how to map the data structure to and from C# Models, you need to create a profile.  
To do so create a class inheriting from `FluentSerializer.Json.Profiles.JsonSerializerProfile`.  
The profile needs to implement the `protected override void Configure()` method, which will be called to construct the mappings inside of this profile.  
  
To create a class mapping, use the `For<TModel>()` method.  
This method has the following optional parameters:
- **direction:** The direction for which this class mapping is valid, defaults to `Both`
- **namingStrategy:** A naming strategy for all properties in the mapping, overriding the Configuration value  
  TODO: Link to advanced concepts naming strategies  

You can create multiple class mappings per profile if that fits your use-case.
  
To map the properties of the C# Model, use method chaining on the `For<TModel>()` method.  
Available options: **`Property<TProperty>()`**.

### Mapping properties

To create a property mapping, use the `Property<TProperty>()` method.  
This method has the following optional parameters:
- **direction:** The direction for which this property mapping is valid, defaults to the class mapping's value.
- **namingStrategy:** A naming strategy for this property mapping, overriding the Configuration value and the parents strategy  
  TODO: Link to advanced concepts naming strategies  
- **converter:** A custom converter for this property mapping, overriding the logic that normally looks up a converter in the default converters  
  TODO: Link to advanced concepts converters  

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
			.Property(entity => entity.Id, ,
				namingStrategy: Names.Are("identifier"))
			.Property(entity => entity.Name);
	}
}
```