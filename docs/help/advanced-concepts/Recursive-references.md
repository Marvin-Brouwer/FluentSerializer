[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="https://github.com/Marvin-Brouwer/FluentSerializer/raw/main/docs/logo/Logo.default.optimized.svg" />
	<a href="https://github.com/Marvin-Brouwer/FluentSerializer/blob/main/docs/help/advanced-concepts/Recursive-references.md#readme">
		Documentation: Recursive references
	</a>
</h1>

[//]: # (Body)

It's possible to recursively reference instances in C# and serializing this causes issues.  
In most serializers this is only the case for writing out serialized data. However, because of the model-first nature of this library this also counts for reading.  
  
At the moment of writing there is no real solution and/or failsafe for this, there is a feature planned for the next release to add a failsafe: [Add recursive detection to default collection converters #101](https://github.com/Marvin-Brouwer/FluentSerializer/issues/101).  
