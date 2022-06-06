[//]: # (Header)

<a href="https://github.com/Marvin-Brouwer/FluentSerializer#readme">
	View main readme
</a><hr/>
<h1>
	<img alt="icon" width="26" height="26"
		src="/docs/logo/Logo.default.optimized.svg" />
	<a href="/docs/help/advanced-concepts/Recursive-references.md#readme">
		Documentation: Recursive references
	</a>
</h1>

[//]: # (Body)

It's possible to recursively reference instances in C# and serializing this causes issues.  
Just like most serializer libraries the way reference loops are handled is configurable.  
There are two settings related to this:  

- **ReferenceLoopBehavior**  
  The `ReferenceLoopBehavior` setting dictates how the serializer handles this instance.
  There are two possible options:
  - `ReferenceLoopBehavior.Throw`: This simply throws a `ReferenceLoopException` when a reference loop is detected.
  - `ReferenceLoopBehavior.Ignore`: This will skip serializing the property, note that this ignores the `WriteNull` setting.
- **ReferenceComparer**  
  The `ReferenceComparer` is an `IComparer` that is responsible for detecting whether an instance has been seen before.
  By default this uses the `.GetHashCode()` method of the instance. You can override this to use bespoke logic to your classes if necessary.
  
_In most serializers it's is only possible to encounter reference loops when writing out serialized data._  
_However, because of the model-first nature of this library this may also count for deserializing using custom `IConverter` implementations._  
_When using OOTB functionality, this is only possible when serializing so the library only has detection for serializing._  
_Because most data formats are linear, having looped or recursive references won't be possible so the profile will just not be able to find a self referencing properties value._  
