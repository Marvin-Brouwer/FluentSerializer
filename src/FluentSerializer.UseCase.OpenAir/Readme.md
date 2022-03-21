[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="/docs/logo/Logo.default.optimized.svg" />
	<a href="/src/FluentSerializer.UseCase.OpenAir#readme">
		FluentSerializer.UseCase.OpenAir
	</a>
</h1>

[//]: # (Body)

This project contains some simulated real world examples to test the library against.
Meaning that the models used may not be production code but it's validating against the actual output of an existing api.

> **Important:** It's important that these examples are based on public api's that share public documentation so it's both verifiable by documentation
> and we're not exposing things that aren't public by design.

## OpenAir

This is a UseCase for using the serializer on the [OpenAir XML AP](https://www.openair.com/download/OpenAirXMLAPIGuide.pdf)  
The example portrayed is not complete and is only intended to illustrate how to (de)serialize the structure of the XML API into a C# model.
This does not include code to:

- Build the Authentication neatly by config
- Custom logic to split out clearing custom fields [Modify, custom equal to](https://www.openair.com/download/OpenAirXMLAPIGuide.pdf#page=56)
- Flatten out the responses into a single list of items
- Map error responses

### The use-case

OpenAir exposes a highly flexible API which allows you to POST multiple types of request through data representation.
In short, you can CRUD in one request and the types you're deleting don't need to match the type you're updating.
To keep this connection a bit more sane it may be worth sticking to a single entity type and a single type of mutation per request,
so that's what this example sticks to.
In addition to this flexible structure the error responses are somewhat exotic and inconsistent between types of errors, but those are considered out of scope.  
Finally OpenAir uses a different DateTime structure than most api's and we'll use that to illustrate a good use of a custom converter.
