using DevelApp.JsonSchemaBuilder.Model;
using Manatee.Json;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JsonSchemaBuilderBoolean : AbstractJsonSchemaBuilderPart<bool?>
    {
        public JsonSchemaBuilderBoolean(IdentifierString boolName, string description, 
            bool? defaultValue = null, bool isRequired = false) : 
            base(boolName, description, isRequired, defaultValue: defaultValue, examples: null, enums: null)
        {
        }


        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.Boolean;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            JsonSchema returnSchema = InitialJsonSchema()
                .Type(JsonSchemaType.Boolean);

            return returnSchema;
        }
    }
}
