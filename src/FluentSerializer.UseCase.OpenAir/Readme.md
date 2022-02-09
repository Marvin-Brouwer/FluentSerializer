[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="https://github.com/Marvin-Brouwer/FluentSerializer/raw/main/doc/logo/Logo.default.optimized.svg" />
	<a href="https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/src/FluentSerializer.UseCase.OpenAir/Readme.md#readme">
		FluentSerializer.UseCase.OpenAir
	</a>
</h1>

[//]: # (Body)

This project contains some simulated real world examples to test the library against.
Meaning that the models used may not be production code but it's validating against the actual output of an existing api.

> **Important:** It's important that these examples are based on public api's that share public documentation so it's both verifiable by documentation
> and we're not exposing things that aren't public by design.

## OpenAir

This is a UseCase for using the serializer on the (OpenAir XML API)[https://www.openair.com/download/OpenAirXMLAPIGuide.pdf]  
The example portraid is not complete and is only intended to illustrate how to (de)serialize the structure of the XML API into a C# model.
This does not include code to:
- Build the Authentication neatly by config
- Custom logic to split out clearing custom fields [Modify, custom equal to](https://www.openair.com/download/OpenAirXMLAPIGuide.pdf#page=56)
- Flatten out the responses into a single list of items
- Handle of error codes
