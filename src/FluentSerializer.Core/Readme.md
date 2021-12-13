# FluentSerializer.Core

This library contains the core parts of the serializer logic.
Meaning:
- Shared interfaces
- Shared code that's not prone to changes

Currently this solution only contains a .Core project.
If there tends to be a lot of shared code with code that's prone to change, that part may get its own project.
For example: `FluentSerializer.Profiles` may be an option, or if it's little code that applies to this condition `FluentSerializer.Shared` may be an option.