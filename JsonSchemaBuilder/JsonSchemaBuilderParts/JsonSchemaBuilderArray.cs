using DevelApp.Utility.Model;
using Manatee.Json;
using Manatee.Json.Schema;
using System.Collections.Generic;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Convenience array definition in Json Schema. Requires Items definition.
    /// </summary>
    public class JsonSchemaBuilderArray : AbstractJsonSchemaBuilderPart<JsonValue>
    {
        public JsonSchemaBuilderArray(IdentifierString arrayName, string description, List<IJsonSchemaBuilderPart> items,
            uint? minItems = null, uint? maxItems = null, bool uniqueItems = false, JsonValue defaultValue = null, List<JsonValue> examples = null, List<JsonValue> enums = null, bool isRequired = false) 
            : base(arrayName, description, isRequired, defaultValue, examples, enums)
        {
            Items = items;
            MinItems = minItems;
            MaxItems = maxItems;
            UniqueItems = uniqueItems;
        }

        public List<IJsonSchemaBuilderPart> Items { get; }

        public uint? MinItems { get; }
        public uint? MaxItems { get; }

        public bool UniqueItems { get; }

        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.Array;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            JsonSchema returnSchema = InitialJsonSchema()
                .Type(JsonSchemaType.Array);
            foreach(IJsonSchemaBuilderPart jsonSchemaBuilderPart in Items)
            {
                returnSchema.Items(jsonSchemaBuilderPart.AsJsonSchema());
            }
            if (MinItems.HasValue)
            {
                returnSchema.MinItems(MinItems.Value);
            }
            if (MaxItems.HasValue)
            {
                returnSchema.MaxItems(MaxItems.Value);
            }
            if(UniqueItems)
            {
                returnSchema.UniqueItems(UniqueItems);
            }
            return returnSchema;
        }
    }
}
