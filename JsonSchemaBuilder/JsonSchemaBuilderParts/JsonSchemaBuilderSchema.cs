using DevelApp.JsonSchemaBuilder.Model;
using Manatee.Json;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JsonSchemaBuilderSchema : AbstractJsonSchemaBuilderPart<JsonValue>
    {
        public JsonSchemaBuilderSchema(IdentifierString schemaName, string description, 
            IJsonSchemaBuilderPart topPart, 
            Dictionary<IdentifierString, IJsonSchemaBuilderPart> definitions = null, bool isRequired = false) 
            : base(schemaName, description, isRequired)
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
            JsonSchema returnSchema = TopPart.AsJsonSchema();
            returnSchema.Id(Name);
            returnSchema.Schema("http://json-schema.org/draft-07/schema#");
            foreach(var pair in Definitions)
            {
                returnSchema.Definitions().Add(pair.Key, pair.Value.AsJsonSchema());
            }
            return returnSchema;
        }
    }
}
