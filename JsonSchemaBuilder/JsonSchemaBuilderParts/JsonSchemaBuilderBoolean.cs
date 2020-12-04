using DevelApp.JsonSchemaBuilder.Model;
using Manatee.Json;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JsonSchemaBuilderBoolean : AbstractJsonSchemaBuilderPart
    {
        public JsonSchemaBuilderBoolean(IdentifierString boolName, string description, bool? defaultValue = null, bool isRequired = false) : base(boolName, description, isRequired)
        {
            DefaultValue = defaultValue;
        }

        public bool? DefaultValue { get; }

        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.Boolean;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            JsonSchema returnSchema = new JsonSchema()
                .Type(JsonSchemaType.Boolean)
                .Title(Name)
                .Description(Description);

            if(DefaultValue.HasValue)
            {
                returnSchema.Default(new JsonValue(DefaultValue.Value));
            }

            return returnSchema;
        }
    }
}
