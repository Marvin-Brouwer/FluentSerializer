[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="https://github.com/Marvin-Brouwer/FluentSerializer/raw/main/doc/logo/Logo.default.optimized.svg" />
	<a href="https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/src/FluentSerializer.UseCase.Mavenlink#readme">
		FluentSerializer.UseCase.Mavenlink
	</a>
</h1>

[//]: # (Body)

This project contains some simulated real world examples to test the library against.
Meaning that the models used may not be production code but it's validating against the actual output of an existing api.

> **Important:** It's important that these examples are based on public api's that share public documentation so it's both verifiable by documentation
> and we're not exposing things that aren't public by design.

## Mavenlink

This is a use-case for (de)serializing data returned from the [Mavenlink REST API](https://developer.mavenlink.com/beta)  
The example portrayed is not complete and is only intended to illustrate how to (de)serialize the structure of the JSON API into a C# model.  
This does not include code to:

- Map error responses

### The use-case

This is mostly standard rest however there's one interesting quirk to this data structure.  
You can `include` certain data types when requesting an entity. So for example when requesting a user,  
you can include an account membership. And because of that the flat data list becomes somewhat difficult to map  
unless you know about this structure up-front, which the rest of your solution shouldn't know about.  
  
This use-case is mostly interesting for the deserializing part, the serializing to post/put is pretty straight forward though has some interesting logic as well.

Arguably, this can be done differently.  
This example explores the use of the "data" node to determine the main requested resource and track back referencing dependencies in the data.  
However, since you usually know which type you're requesting in code. You could also create a response type per api call containing all its 
includable types to be deserialized automatically and then reference them in a separate step afterwards.  
The pro of that solution would be that it's a more robust solution for recursive dependencies.
However, the con is that it's less flexible than the aproach used in this example.  
Either way this example is just an example to show how you **could** solve these kind of dependencies but, it's probably not how you _should_.
