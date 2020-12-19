using DevelApp.JsonSchemaBuilder.Exceptions;
using DevelApp.Utility.Model;
using Manatee.Json;
using Manatee.Json.Schema;
using System.Collections.Generic;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Convenience array definition in Json Schema. Requires Items definition.
    /// </summary>
    public class JSBArray : AbstractJSBPart<JsonValue>
    {
        public JSBArray(IdentifierString arrayName, string description, List<IJSBPart> items,
            uint? minItems = null, uint? maxItems = null, bool uniqueItems = false, JsonValue defaultValue = null, List<JsonValue> examples = null, List<JsonValue> enums = null, bool isRequired = false) 
            : base(arrayName, description, isRequired, defaultValue, examples, enums)
        {
            if(items == null || items.Count == 0)
            {
                throw new JsonSchemaBuilderException($"An array without defined content does not make sense. {nameof(items)} has not been defined");
            }
            Items = items;
            MinItems = minItems;
            MaxItems = maxItems;
            UniqueItems = uniqueItems;
        }

        public List<IJSBPart> Items { get; }

        public uint? MinItems { get; }
        public uint? MaxItems { get; }

        public bool UniqueItems { get; }

        public override JSBPartType PartType
        {
            get
            {
                return JSBPartType.Array;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            JsonSchema returnSchema = InitialJsonSchema()
                .Type(JsonSchemaType.Array);
            foreach(IJSBPart jsonSchemaBuilderPart in Items)
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
