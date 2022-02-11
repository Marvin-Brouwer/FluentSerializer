[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="https://github.com/Marvin-Brouwer/FluentSerializer/raw/main/doc/logo/Logo.default.optimized.svg" />
	<a href="https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/src/FluentSerializer.UseCase.Mavenlink/Readme.md#readme">
		FluentSerializer.UseCase.Mavenlink
	</a>
</h1>

[//]: # (Body)

This project contains some simulated real world examples to test the library against.
Meaning that the models used may not be production code but it's validating against the actual output of an existing api.

> **Important:** It's important that these examples are based on public api's that share public documentation so it's both verifiable by documentation
> and we're not exposing things that aren't public by design.

## Mavenlink

This is a usecase for (de)serializing data returned from the [Mavenlink REST API](https://developer.mavenlink.com/beta)  
The example portraid is not complete and is only intended to illustrate how to (de)serialize the structure of the JSON API into a C# model.  
This does not include code to:  
- Map error responses

### The use-case
This is mostly standard rest however there's one interesting quirk to this data structure.  
You can `include` certain data types when requesting an entity. So for example when requesting a user,  
you can include an account membership. And because of that the flat data list becomes somewhat difficult to map  
unless you know about this structure up-front, which the rest of your solution shouldn't know about.  
  
This use-case is mostly interesting for the deserializing part, 
the serializing to post/put is pretty straight forward though has some interesting logic as well.  