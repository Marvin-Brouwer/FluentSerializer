# Api design

The way the programming api should look has to be similar for every vertical of this project.
Since we'd like to support Json, Bson, XML, Yaml and possibly more we need to be careful about this.
Mainly usage between the types of serialization **has** to be similar enough to not require a new learning curve.
Obviously there will be differences.

## Setting up the serializer

Because of how this serializer will work we'll need to register the serializer profiles on startup, similar to AutoMapper.
```cs
    services.AddFluentSerializer<IAssemblyMarker>();
```
```cs
public class SomeJsonProfile : JsonSerializerProfile {

    public override void Configure() {

        For<TypeToSerialize>(
            defaultNamingStrategy: CamelCaseNamingStrategy
        )
            .Member(t => t.Name)
            .Member(t => t.Age,
                direction: Direction.Serialize,
                namingStrategy: CustomNamingStrategy("userAge"))
            .Member(t => t.BirthDate, 
                customSerializer: typeof(DateSerializer))
            .Member(t => t.Skills)
    }
}
public class SomeXmlProfile : XmlSerializerProfile {

    public override void Configure() {

        For<TypeToSerialize>(
            rootNamingStrategy: CustomNamingStrategy("rootOverride"),
            defaultNamingStrategy: PascalCaseNamingStrategy
        )
            .Attribute(t => t.Name)
            .Attribute(t => t.Age, 
                direction: Direction.Serialize,
                namingStrategy: CustomNamingStrategy("userAge"))
            .Attribute(t => t.BirthDate, 
                customSerializer: typeof(DateSerializer))
            .Child(t => t.Skills)
    }
}
```

## Using the serializer

To use the serializer it has to be injected because of previously mentioned profiles, we did have a think about using it statically like Json.Net but That would cause issues with finding the profiles when having nested types.
It's an option to reflect on the fly but it seems not worth the performance loss.

To keep the api consistency every serializer should implement ISerializer, which is not registered as a type by itself.
The way to inject the serializer shall be with a specific type to distinguish between the two may your project call multiple api's with different serializations.

For example:
```cs
public interface ISerializer {
    
    string Serialize<TData>(TData dataObject);
    TData DeSerialize<TData>(string dataObject);
}
```
(This may benefit from overload with span and stringreaders etc.)
```cs
public interface IJsonSerializer : ISerializer {
    string Serialize(JObject dataObject);
    JObject DeSerializeToJObject(string dataObject);
}
```
```cs
public interface IXmlSerializer : ISerializer {
    string Serialize(XObject dataObject);
    XObject DeSerializeToXObject(string dataObject);
}
```
As suggested above the exposed interfaces may expose methods for (de)serializing to the raw object structure node.
This may seem trivial but we've seen situations where that would've saved us a lot of issues if we had the option while the feature request of the serializer library was being added.

These `IJsonSerializer` and `IXmlSerializer` will be the registered types that are injectable giving developers a choice to data representation by specifying the injected interface.

## Technical implications

This way of defining profiles with a similar Programming API structure does require a good thought about the shared logic underneath for building the profiles.
As much code as possible should be shared to prevent duplication while keeping it flexible to add a data type like Yaml at a later stage.
This will most likely be done by mapping properties to XPath / JsonPath because these are similar enough to eachother to make a simple data structure but keep the possibility for attributes vs elements in XML
Also, for performance reasons this solution will heavily rely on source generators.

## Success factors

One of the most important things is that this library is actually applicable to use in production settings.
Because of that real examples are manditory.
This project will have unit tests which serialize and deserialize based on real world api data and will assert against in-repo binary files to make sure the written data is as expected + input data comes out as usable C#.