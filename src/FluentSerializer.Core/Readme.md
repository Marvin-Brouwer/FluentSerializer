[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="/docs/logo/Logo.default.optimized.svg" />
	<a href="/src/FluentSerializer.Core#readme">
		FluentSerializer.Core
	</a>
</h1>

[//]: # (Body)

This library contains the core parts of the serializer logic.
Meaning:

- Shared interfaces
- Shared code that's not prone to changes

Currently this solution only contains a `FluentSerializer.Core` project.  
If there tends to be a lot of shared code with code that's prone to change, that part may get its own project.  
For example: `FluentSerializer.Profiles` may be an option, or if it's little code that applies to this condition `FluentSerializer.Shared` may be an option.
