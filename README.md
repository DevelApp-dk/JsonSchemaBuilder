# JsonSchemaBuilder
Simple Json Schema builder to make it faster to combine in code and to generate code for the schema making it possible to use schema first.

Why another code generation tool from Json Schema ? Mainly the ones made are either supporting a too old version of Json Schema or generated code with the wrong structure. I also needed dynamic code generation to make dynamic API based on defined messages for making dynamic MVC controller for a message based system so that an API can be supported on the system for others to interface with.


This has mainly been made to support my [Workflow project](https://github.com/DevelApp-dk/Workflow) which works on schema first principle.

## Schema parts supported
My intention is to support all parts of the official Json Schema Draft 7 and some special formats that are used often

* **Array**
* **Boolean**
* **Date**
* **DateTime**
* **Email**
* **Integer**
* **Number**
* **Object**
* **Schema**

  This is a bit different in that it only wraps one of the other parts as a  top part and otherwise handles definitions as they can only be handled on the schema level
* **String**
* **Time**
* **UriReference**

  This has the limitation that it does not get referenced schemas and expect referenced schemas to be located locally. References can only be referencing part of definitions or entire schema.

## Code generation
Code generation is built based on the above schema parts

### Supported languages
* C#

### Planned language support
* PHP
* TypeScript
* VB
* Java
* TSQL DDL for Database First approach to development
* C# Entities for Code First approach to development

## TODO
* Create Email data class (with System.Net.Mail.MailAddress as inner?)
* Create DataString data class to replace string ?
* Add implicit casts in classes
* Add Base64String with conversions
* More intelligent testing if possible for serialization of export formats
* Support for AnyOf, OneOf and AllOf (last done through conversion)
* C#: Expand support for serializer other than Newtonsoft version
  * Microsoft own
  * Akka.Net serializer Hyperion
* Support for later than Draft 7 schema
* Use of Microsoft own serializer and [json-everything](https://github.com/gregsdennis/json-everything) instead of [Manatee.Json](https://github.com/gregsdennis/Manatee.Json)
* Better support for UriReference