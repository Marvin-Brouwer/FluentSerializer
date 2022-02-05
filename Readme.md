[//]: # (Header)

[package-url-xml]: https://www.nuget.org/packages/FluentSerializer.Xml/
[package-shield-v-xml]: https://img.shields.io/nuget/v/FluentSerializer.Xml.svg?style=flat-square
[package-shield-d-xml]: https://img.shields.io/nuget/dt/FluentSerializer.Xml.svg?style=flat-square
[package-url-json]: https://www.nuget.org/packages/FluentSerializer.Json/
[package-shield-v-json]: https://img.shields.io/nuget/v/FluentSerializer.Json.svg?style=flat-square
[package-shield-d-json]: https://img.shields.io/nuget/dt/FluentSerializer.Json.svg?style=flat-square

[license-url]: https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/License.md#readme
[license-shield]: https://img.shields.io/badge/license-Apache--2.0-blue.svg?style=flat-square
[repo-stars-url]: https://github.com/Marvin-Brouwer/FluentSerializer/stargazers
[repo-stars-shield]: https://img.shields.io/github/stars/Marvin-Brouwer/FluentSerializer.svg?color=brightgreen&style=flat-square

<h1 align="center">

  [![Fluent Serializer](./doc/logo/Banner.optimized.svg?token=GHSAT0AAAAAABNT2E5XSKNFQBQT2XRIDGS6YPD3THA)](#Readme)

</h1>

<h3 align="center">

  [![XML Nuget][package-shield-v-xml]![XML Downloads][package-shield-d-xml]][package-url-xml]  
  [![JSON Nuget][package-shield-v-json]![JSON Downloads][package-shield-d-Json]][package-url-json]  
  [![Stars][repo-starts-shield]][repo-stars-url] [![License][license-schield]][license-url] 

</h3>

[//]: # (TOC)

<h3 align="center">
  
  [Getting started](#GettingStarted) - 
  [Basic usage](#BasicUsage) - 
  [Advanced usage](#AdvancedUsage)

</h3>

[//]: # (Introduction)
`FluentSerializer` is a library to help you with serializing to-and-from C# POCOs using profiles.  
The use of profiles helps making it easier to understand how the data vs the code looks in a single glance.

[//]: # (GettingStarted)
[json-di-dotnet-readme]: https://github.com/Marvin-Brouwer/FluentSerializer/tree/main/src/FluentSerializer.Json.DependencyInjection.NetCoreDefault#readme
[xml-di-dotnet-readme]: https://github.com/Marvin-Brouwer/FluentSerializer/tree/main/src/FluentSerializer.Xml.DependencyInjection.NetCoreDefault#readme
## Getting started

Install a `FluentSerializer` for the serial format you need. Currently we only support the default DotNet dependency injection framework. 

<sub>[FluentSerializer.Json.DependencyInjection.NetCoreDefault][json-di-dotnet-readme]</sub>
```txt
dotnet add package FluentSerializer.Json.DependencyInjection.NetCoreDefault
```  
<sub>[FluentSerializer.Xml.DependencyInjection.NetCoreDefault][xml-di-dotnet-readme]</sub>
```txt
dotnet add package FluentSerializer.Xml.DependencyInjection.NetCoreDefault
```

And then add the serializer to the DI registration, pointing to the a type in the assembly where your profiles live.
```csharp
serviceCollection
  .AddFluentJsonSerializer<TAssemblyMarker>()
  .AddFluentXmlSerializer<TAssemblyMarker>();
```
There are multiple overloads for overriding configurations and passing assemblies, please read the respective readme's for the `DependencyInjection` libraries.

[//]: # (BasicUsage)
## Basic usage

For the serializer to work you need to create a profile and call the serializer.

### Creating profiles
You create a profile by creating a class that inherits from the serializers profile class.  
`FluentSerializer.Json.JsonSerializerProfile`, `FluentSerializer.Xml.JsonSerializerProfile`, and maybe others.  
 
When these profiles are created in an assembly that has been registered in the DI startup the startup will find the correct profiles for the correct serializer. Each profile has it's own builder methods but follow a similar style.  
<!--  todo create profile readme's -->
- [Creating a JSON profile](https://github.com/Marvin-Brouwer/FluentSerializer/tree/main/src/FluentSerializer.Json#CreatingProfile)
- [Creating an XML profile](https://github.com/Marvin-Brouwer/FluentSerializer/tree/main/src/FluentSerializer.Xml#CreatingProfile)

### Calling the serializer
Once the profiles are registered all you have to do is inject the serializer into the service responsible for handling serialized application dependencies and call the serializer or deserialize method.
```csharp
public sealed class WeirdExample : IWeirdExample {

  private readonly IWebClient _webClient;
  private readonly IJsonSerializer _jsonSerializer;
  private readonly IXmlSerializer _xmlSerializer;

  public WeirdExample(IWebClient webClient, IJsonSerializer jsonSerializer, IXmlSerializer xmlSerializer) {
    _webClient = webClient;
    _jsonSerializer = jsonSerializer;
    _xmlSerializer = xmlSerializer;
  }

  public TReceive DoApiCall<TSend, TReceive>(TSend sendModel) {

    var sendXML = _xmlSerializer.Serialize(sendModel);
    var idResponse = _webClient.Post(sendXML);

    var otherApiJsonResponse = _webClient.Get(idResponse);
    return _jsonSerializer.Deserialize(otherApiJsonResponse);
  }
}
```
The serialize will automatically find the correct profile for the types that are passed or requested and (de)serialize as expected.

[//]: # (UseCaseExamples)
## Use-case Examples

To get a quick view of how this library may benefit you, check out these use-cases:

- [Mavenlink](https://github.com/Marvin-Brouwer/FluentSerializer/tree/main/src/FluentSerializer.UseCase.Mavenlink#readme) (`JSON`)
- [OpenAir](https://github.com/Marvin-Brouwer/FluentSerializer/tree/main/src/FluentSerializer.UseCase.OpenAir#readme) (`XML`)

[//]: # (AdvancedUsage)
## Advanced usage

Here are some links to some more advanced topics:

- 

[//]: # (Misc)
## Contributing
We are currently figuring out what the best branching model is, and what the best automated release setup is.  
Because of that we aren't really looking for code contributions until that is in place.  
  
Suggestions on how to do this are very welcome.  