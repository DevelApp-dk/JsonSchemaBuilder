using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder
{
    public interface IJsonSchemaPart
    {
        JsonSchema AsJsonSchema();
        IJsonContainer GetJsonContainer();
    }
}
