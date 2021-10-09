# OpenAir

This is a Usecase for using the serializer on the (OpenAir XML API)[https://www.openair.com/download/OpenAirXMLAPIGuide.pdf]  
The example portraid is not complete and is only intended to illustrate how to (de)serialize the structure of the XML API into a C# model.
This does not include code to:
- Build the Authentication neatly by config
- Custom logic to split out clearing custom fields [Modify, custom equal to](https://www.openair.com/download/OpenAirXMLAPIGuide.pdf#page=56)
- Flatten out the responses into a single list of items
- Handle of error codes
