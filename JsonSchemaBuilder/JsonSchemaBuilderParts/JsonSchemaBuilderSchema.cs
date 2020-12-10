using DevelApp.Utility.Model;
using Manatee.Json;
using Manatee.Json.Schema;
using System.Collections.Generic;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JsonSchemaBuilderSchema : AbstractJsonSchemaBuilderPart<JsonValue>
    {
        public static JsonSchemaBuilderSchema BuildSchema(JsonSchema jsonSchema)
        {
            if(jsonSchema.Equals(JsonSchema.Empty))
            {
                return new JsonSchemaBuilderSchema("NoValidation", "Represents an empty schema with disabled validation");
            }

            IJsonSchemaBuilderPart topPart = null;
            
            //TODO build from the schema

            JsonSchemaBuilderSchema jsonSchemaBuilderSchema = new JsonSchemaBuilderSchema(jsonSchema.Id, jsonSchema.Description(), topPart);

            return jsonSchemaBuilderSchema;
        }

        public JsonSchemaBuilderSchema(IdentifierString schemaName, string description, 
            IJsonSchemaBuilderPart topPart = null, 
            Dictionary<IdentifierString, IJsonSchemaBuilderPart> definitions = null, JsonValue defaultValue = null,
            List<JsonValue> examples = null, List<JsonValue> enums = null, bool isRequired = false) 
            : base(schemaName, description, isRequired, defaultValue, examples, enums)
        {
            TopPart = topPart;
            if (definitions != null)
            {
                Definitions = definitions;
            }
            else
            {
                Definitions = new Dictionary<IdentifierString, IJsonSchemaBuilderPart>();
            }
        }

        public Dictionary<IdentifierString, IJsonSchemaBuilderPart> Definitions { get; }

        public IJsonSchemaBuilderPart TopPart { get; }

        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.Schema;
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
                    returnSchema.Definitions().Add(pair.Key, pair.Value.AsJsonSchema());
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
