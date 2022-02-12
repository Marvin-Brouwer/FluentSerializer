[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="https://github.com/Marvin-Brouwer/FluentSerializer/raw/main/doc/logo/Logo.default.optimized.svg" />
	<a href="https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/doc/help/advanced-concepts/Adding-a-serializer.md#readme">
		Documentation: Adding a serializer
	</a>
</h1>

[//]: # (Body)

If you want to add a custom serializer, keep the following in mind.  

- You need an issue in github to track against
- You need to reference the documentation in code and you need to write a Readme similar to XML and JSON
- You need to add libraries for dependency injection too
- You need to follow the concept of profiles strictly, expect discussing in PR's
- You need to create a library where your data models can be used plainly with a supporting `IConverter`

Please take inspiration from what's already there, we value consistency.

## The FluentSerializer concept

As you may have noticed, the usage of the serializer itself is not particularly fluent.  
The fluent part comes in the profiles, the core concept of this library is that it should be immediately apparent how your model translates to- and from data structures.  
This translation is configured with the fluent profiles.  
An important part here is consistency and predictability, so you can read profiles without reading additional documentation.  
Consider these two profiles:

```csharp
public sealed class ExampleProfile : JsonSerializerProfile
{
	protected override void Configure()
	{
		For<ExampleModel>()
			.Property(example => example.Id)
			.Property(example => example.Name);
	}
}
```

```csharp
public sealed class ExampleProfile : XmlSerializerProfile
{
	protected override void Configure()
	{
		For<ExampleModel>()
			.Attribute(example => example.Id)
			.Element(example => example.Name);
	}
}
```

The profiles are different because their respective data types have different constructs.  
However, they are visibly similar in _**HOW**_ they define the mappings to those constructs.  
It is important that new libraries follow a similar way of defining mappings.

## Solution structure

For consistency sake we ask you to follow the pattern defined by what's already in the solution.  
When you add a serializer you're requested to structure your solution like so:

- DataType(solution folder)
  - FluentSerializer.DataType.csproj
  - FluentSerializer.DataType.Benchmark.csproj
  - FluentSerializer.DataType.Converter.DefaultDataType.csproj
  - FluentSerializer.DataType.DependencyInjection.NetCoreDefault.csproj (and possibly others)
  - FluentSerializer.DataType.Tests.csproj

## Use-case example

If there's no use-case for this serializer we can't accept it into our repository.  
We aren't supporting data types for the sake of it, we're trying to help developers in their needs.  
So if you write a serializer, please also refer to [Adding a use-case](https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/doc/help/advanced-concepts/Adding-a-use-case.md#readme).

## In need of help

If you have questions or need help, use the issue you created in GitHub.  
The maintainers will inspect the issues on a weekly basis and will answer any specific questions there.  
If you have unrelated questions or generic questions, feel free to start a discussion in the discussions section.
