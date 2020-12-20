using DevelApp.Utility.Model;
using Manatee.Json;
using Manatee.Json.Schema;
using System.Collections.Generic;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JSBSchema : AbstractJSBPart<JsonValue>
    {
        public static JSBSchema BuildSchema(JsonSchema jsonSchema)
        {
            if(jsonSchema.Equals(JsonSchema.Empty))
            {
                return new JSBSchema("NoValidation", "Represents an empty schema with disabled validation");
            }

            IJSBPart topPart = null;
            
            //TODO build from the schema

            JSBSchema jsonSchemaBuilderSchema = new JSBSchema(jsonSchema.Id, jsonSchema.Description(), topPart);

            return jsonSchemaBuilderSchema;
        }

        public JSBSchema(IdentifierString schemaName, string description, 
            IJSBPart topPart = null, 
            List<IJSBPart> defs = null, JsonValue defaultValue = null,
            List<JsonValue> examples = null, List<JsonValue> enums = null, bool isRequired = false) 
            : base(schemaName, description, isRequired, defaultValue, examples, enums)
        {
            TopPart = topPart;
            if (defs != null)
            {
                foreach (IJSBPart part in defs)
                {
                    Definitions.Add(part.Name, part);
                }
            }
        }

        public Dictionary<string, IJSBPart> Definitions { get; } = new Dictionary<string, IJSBPart>();

        public IJSBPart TopPart { get; }

        public override JSBPartType PartType
        {
            get
            {
                return JSBPartType.Schema;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            if (TopPart != null)
            {
                JsonSchema returnSchema = TopPart.AsJsonSchema();
                returnSchema.Id(Name);
                returnSchema.Schema("http://json-schema.org/draft-07/schema#");
                foreach (var pair in Definitions)
                {
                    returnSchema.Definition(pair.Key, pair.Value.AsJsonSchema());
                }
                return returnSchema;
            }
            else
            {
                JsonSchema returnSchema = JsonSchema.Empty;
                returnSchema.Id(Name);
                returnSchema.Schema("http://json-schema.org/draft-07/schema#");
                returnSchema.Title(Name);
                returnSchema.Description(Description);
                return returnSchema;
            }
        }
    }
}
