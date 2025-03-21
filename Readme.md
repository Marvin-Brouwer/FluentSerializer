[//]: # (Header)

[package-spacer]: https://img.shields.io/badge/--lightgrey?style=flat-square
[package-url-xml]: https://www.nuget.org/packages/FluentSerializer.Xml/
[package-shield-i-xml]: https://img.shields.io/badge/-XML-lightgrey?style=flat-square
[package-shield-v-xml]: https://img.shields.io/nuget/v/FluentSerializer.Xml.svg?style=flat-square
[package-shield-d-xml]: https://img.shields.io/nuget/dt/FluentSerializer.Xml.svg?style=flat-square
[package-url-json]: https://www.nuget.org/packages/FluentSerializer.Json/
[package-shield-i-Json]: https://img.shields.io/badge/-JSON-lightgrey?style=flat-square
[package-shield-v-json]: https://img.shields.io/nuget/v/FluentSerializer.Json.svg?style=flat-square
[package-shield-d-json]: https://img.shields.io/nuget/dt/FluentSerializer.Json.svg?style=flat-square

[license-url]: /License.md#readme
[license-shield]: https://img.shields.io/badge/license-Apache--2.0-blue.svg?style=flat-square
[repo-stars-url]: https://github.com/Marvin-Brouwer/FluentSerializer/stargazers
[repo-stars-shield]: https://img.shields.io/github/stars/Marvin-Brouwer/FluentSerializer.svg?color=brightgreen&style=flat-square
[discord-url]: https://discord.gg/fkw3Tmyu
[discord-shield]: https://img.shields.io/discord/958295823001722890?label=discord&logo=discord&style=flat-square

> [!IMPORTANT]  
> We are looking for **you**!  
> This library has been in a state of disrepair for a while because the pipelines broke.  
> This is not an excuse, the reason is that maintaining a library takes significant time, and, we find it hard to find this time when nobody uses the library.
>
> If you are a user of this library, please [leave a message here](https://github.com/Marvin-Brouwer/FluentSerializer/discussions/401) to let us know. So, we know for whom we're keeping this running.  

<h1 align="center">
	<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	<img alt="Fluent Serializer banner"
		src="/docs/logo/Banner.optimized.svg" />
	</a>
</h1>

<h3 align="center">

[![XML][package-shield-i-xml]][package-url-xml] [![XML Nuget][package-shield-v-xml]![XML Downloads][package-shield-d-xml]][package-url-xml]  
[![JSON][package-shield-i-json]![JSON Nuget][package-shield-v-json]![JSON Downloads][package-shield-d-Json]][package-url-json]  

[![Stars][repo-stars-shield]][repo-stars-url] [![License][license-shield]][license-url] [![Discord][discord-shield]][discord-url]

</h3>

[//]: # (TOC)

<h3 align="center">

[Getting started](#getting-started) - [Basic usage](#basic-usage) - [Use-case examples](#use-case-examples) - [Basic concepts](#basic-concepts) - [Advanced concepts](#advanced-concepts)

</h3>
<hr/>

[//]: # (Document)
[mappster]: https://github.com/MapsterMapper/Mapster#readme
[automapper]: https://github.com/AutoMapper/AutoMapper#readme

`FluentSerializer` is a library to help you with serializing to-and-from C# POCOs using profiles.
The use of profiles helps making it easier to understand how the data vs the code looks in a single glance.
So instead of needing a library like [Mappster][mappster] or [AutoMapper][automapper] to mold data into your code structure, you can now map that same data into a clean data representation and use the mapping frameworks for what their intended, to translate data.

Next to a clear overview of how the data looks, this library provides you with serializing methods for multiple data formats with a similar api.
So when you're required to tie XML and JSON together at least the code for serializing looks similar across your solution.

If you're just looking for a simple JSON or XML serializer checkout these options:

- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json#readme)
  The most commonly used dotnet JSON serializer
- [JaxLib](https://github.com/YAXLib/YAXLib#readme)
  An awesome XML serializer that's miles ahead of the DotNet default one.

## Getting started

[json-readme]: /src/FluentSerializer.Json#readme
[xml-readme]: /src/FluentSerializer.Xml#readme

Install a `FluentSerializer` for the serial format you need.

- **Json**: [`FluentSerializer.Json`][json-readme]  

  ```txt
  dotnet add package FluentSerializer.Json
  ```

- **Xml**: [`FluentSerializer.Xml`][xml-readme]  

  ```txt
  dotnet add package FluentSerializer.Xml
  ```

## Basic usage

For the serializer to work you need to create a profile and call the serializer.

### Creating profiles

You create a profile by creating a class that inherits from the serializer's profile class.
`FluentSerializer.Json.JsonSerializerProfile`, `FluentSerializer.Xml.JsonSerializerProfile`, and maybe others.

When these profiles are created in an assembly that has been registered in the DI startup the startup will find the correct profiles for the correct serializer. Each profile has it's own builder methods but follow a similar style.

- [Creating a JSON profile](/src/FluentSerializer.Json/Readme.md#creating-profiles)
- [Creating an XML profile](/src/FluentSerializer.Xml/Readme.md#creating-profiles)

For illustration's sake, here's a basic example of a profile:
<details>
  <summary>JSON structure & supporting classes</summary>

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

</details>

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

### Calling the serializer

Once the profiles are registered all you have to do is inject the serializer into the service responsible for handling serialized application dependencies and call the serializer or deserialize method.

```csharp
public sealed class WeirdExample : IWeirdExample {

	private readonly IWebClient _webClient;
	private readonly IJsonSerializer _jsonSerializer;
	private readonly IXmlSerializer _xmlSerializer;

	public WeirdExample(IWebClient webClient) {

		_webClient = webClient;

		_jsonSerializer = SerializerFactory.For
			.Json()
			.UseProfilesFromAssembly<IAssemblyMarker>();
		_xmlSerializer =  SerializerFactory.For
			.Xml()
			.UseProfilesFromAssembly<IAssemblyMarker>();
	}

	public TReceive DoApiCall<TSend, TReceive>(TSend sendModel) {

		var sendXML = _xmlSerializer.Serialize(sendModel);
		var idResponse = _webClient.Post(sendXML);

		var otherApiJsonResponse = _webClient.Get(idResponse);
		return _jsonSerializer.Deserialize<TReceive>(otherApiJsonResponse);
	}
}
```

The serialize will automatically find the correct profile for the types that are passed or requested and (de)serialize as expected.

### Configuration

Every serializer has overloads for the factory that will allow you to configure the serialier to fit your application.  
To read more about that you can either visit the specific serializer's readme, check out the [Basic concepts](#basic-concepts) or the [Advanced concepts](#advanced-concepts) section of this guide.

- [Configuring `JSON`](./src/FluentSerializer.Json#Configuration)
- [Configuring `XML`](./src/FluentSerializer.Xml#Configuration)

### Dependency injection

[json-di-dotnet-readme]: /src/FluentSerializer.Json.DependencyInjection.NetCoreDefault#readme
[xml-di-dotnet-readme]: /src/FluentSerializer.Xml.DependencyInjection.NetCoreDefault#readme

Alternatively, if you prefer dependency injection;  
Each serializer has an adjacent NuGet package that makes registering the serializer to the default DotNet dependency injection library easier.

Install a corresponding NuGet package for the serial format you need.

- **Json**: [`FluentSerializer.Json.DependencyInjection.NetCoreDefault`][json-di-dotnet-readme]  

  ```txt
  dotnet add package FluentSerializer.Json.DependencyInjection.NetCoreDefault
  ```

- **Xml**: [`FluentSerializer.Xml.DependencyInjection.NetCoreDefault`][xml-di-dotnet-readme]  

  ```txt
  dotnet add package FluentSerializer.Xml.DependencyInjection.NetCoreDefault
  ```

And then add the serializer to the DI registration, pointing to the a type in the assembly where your profiles live.

```csharp
serviceCollection
	.AddFluentJsonSerializer<TAssemblyMarker>()
	.AddFluentXmlSerializer<TAssemblyMarker>();
```

Like for the factory approach, there are multiple overloads for overriding configurations and passing assemblies.  
Please read the respective readme's for the `DependencyInjection` libraries to read more.

## Use-case Examples

To get a quick view of how this library may benefit you, check out these use-cases:

- [Mavenlink](/src/FluentSerializer.UseCase.Mavenlink#readme) (`JSON`)
- [OpenAir](/src/FluentSerializer.UseCase.OpenAir#readme) (`XML`)

## Basic concepts

Here are some links to some basic concepts:

- [Naming strategies](/docs/help/basic-concepts/Naming-strategies.md#readme)
- [Converters](/docs/help/basic-concepts/Converters.md#readme)
- [Converters, EnumConverter](/docs/help/basic-concepts/Converters-EnumConverter.md#readme)
- [Converters, FormattableConverter](/docs/help/basic-concepts/Converters-FormattableConverter.md#readme)
- [Converters, ParsableConverter](/docs/help/basic-concepts/Converters-ParsableConverter.md#readme)

## Advanced concepts

Here are some links to some more advanced topics:

### General

- [Recursive references](/docs/help/advanced-concepts/Recursive-references.md#readme)
- [Custom converters and accessing parent nodes](/docs/help/advanced-concepts/Converter-node-access.md#parent-node-reference)
- [Custom converters and accessing the root node](/docs/help/advanced-concepts/Converter-node-access.md#root-node-reference)
- [Adding a serializer](/docs/help/advanced-concepts/Adding-a-serializer.md#readme)
- [Adding a use-case](/docs/help/advanced-concepts/Adding-a-use-case.md#readme)

### JSON

- [Using raw JSON](/src/FluentSerializer.Json.Converter.DefaultJson#readme)

### XML

- [Using raw XML](/src/FluentSerializer.Xml.Converter.DefaultXml#readme)

## Contributing

> We are currently figuring out what the best branching model is, and we're still fleshing out release, contribution and development guidelines.  
> **Suggestions on how to do this are very welcome.**  
  
If you want to help out the project, you are very welcome to.  
Please read our [Contribution guidelines](/docs/Contributing.md#readme) and [contributor covenant's code of conduct](https://www.contributor-covenant.org) before starting.
For maintainers we have an additional [Maintenance guide](/docs/Maintaining.md#readme).  

> **Note**
> Because of backwards compatibility, you will need to install additional runtimes if you want to debug the solution locally.  
> See [Contribution guidelines - Technical prerequisites](/docs/Contributing.md#technical-prerequisites) for more detail.

### Release management

Maintainers are responsible for the release cycles.
However, if you register a bug blocking your development you can request an alpha package version for your branch to be deployed while the code is in review.

For more information, review the [Maintenance guides Release management chapter](/docs/Maintaining.md#release-management).  

### Contributors

<a href="https://github.com/Marvin-Brouwer/FluentSerializer/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=Marvin-Brouwer/FluentSerializer" />
</a>

Made with [contrib.rocks](https://contrib.rocks).
