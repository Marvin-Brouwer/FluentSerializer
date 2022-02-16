[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="https://github.com/Marvin-Brouwer/FluentSerializer/raw/main/doc/logo/Logo.default.optimized.svg" />
	<a href="https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/doc/help/advanced-concepts/Converter-parent-access.md">
		Documentation: Custom converters and accessing parent nodes
	</a>
</h1>

[//]: # (Body)

It may be necessary to access the parent of the node being serialized.  
For example, internal collection converter for XML needs this.  

Because of the `IDataNodes` being immutable there's no direct reference to each parent node.  
The parent node of the current node being serialized will be available in the `ISerializationContext` inside of the `Serialize` method on any implementation of `I{SerialType}Converter` and can be accessed like the following example:  

```csharp
	var parent = (IJsonObject)context.ParentNode!;
```

It is important that you know the dataType of the parent up front otherwise your converter will fail.  
An example of a real world example needing parent access would be a data format where you need to reference collections on the same level als the node you're referring to. [Mavenlink use-case's MavenlinkResponseDataConverter](https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/src/FluentSerializer.UseCase.Mavenlink/Serializer/Converters/MavenlinkResponseDataConverter.cs) shows an example of where parent access can be useful.  

## Root node reference

Currently there is no access to the root node as a reference, if you need one please request a feature and include your use-case so we can use this as an example.  
See: [Adding a use-case](https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/doc/help/advanced-concepts/Adding-a-use-case.md#readme).
