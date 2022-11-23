[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="/docs/logo/Logo.default.optimized.svg" />
	<a href="/docs/help/advanced-concepts/Converter-node-access.md">
		Documentation: Custom converters and accessing other nodes
	</a>
</h1>

[//]: # (Body)

It may be necessary to access a parent, a sibling, a child, the root, or any other node related to the node being serialized.  
For example, internal collection converter for XML needs this.  
  
Because the `IDataNodes` are immutable there's no direct reference to each parent node.  
To facilitate that, the `ISerializerContext` provides access to the root and the parent node.  
  
## Parent node reference

The parent node of the current node being serialized will be available in the `ISerializationContext` inside of the `Serialize` method on any implementation of `I{SerialType}Converter` and can be accessed like the following example:  

```csharp
	var parent = (IJsonObject)context.ParentNode!;
```

It is important that you know the dataType of the parent up front otherwise your converter will fail.  
 
A real world example needing parent access would be a data format where you need to reference collections on the same level als the node you're referring to. [Mavenlink use-case's MavenlinkResponseDataConverter](/src/FluentSerializer.UseCase.Mavenlink/Serializer/Converters/MavenlinkResponseDataConverter.cs) shows an example of where parent access can be useful.  

## Root node reference

The root node of the current document being serialized will be available in the `ICoreContext` and thus the `ISerializationContext` inside of the `Serialize` method on any implementation of `I{SerialType}Converter` and can be accessed like the following example:  

```csharp
	var parent = (IJsonObject)context.RootNode!;
```

It is important that you know the dataType of the parent up front otherwise your converter will fail.  
  
A real world example needing the root node would be a data format where you need to reference collections via the root of the document, like an id reference pointing to an included collection in the root of the document. [Mavenlink use-case's MavenlinkIdReferenceConverter](/src/FluentSerializer.UseCase.Mavenlink/Serializer/Converters/MavenlinkIdReferenceConverter.cs) shows an example of where root node access can be useful.  
