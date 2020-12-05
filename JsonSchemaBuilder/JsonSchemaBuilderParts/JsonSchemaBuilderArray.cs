using DevelApp.JsonSchemaBuilder.Exceptions;
using DevelApp.JsonSchemaBuilder.Model;
using Manatee.Json;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Convenience array definition in Json Schema. Requires Items definition.
    /// </summary>
    public class JsonSchemaBuilderArray : AbstractJsonSchemaBuilderPart<JsonValue>
    {
        public JsonSchemaBuilderArray(IdentifierString arrayName, string description, List<IJsonSchemaBuilderPart> items,
            uint? minItems = null, uint? maxItems = null, bool uniqueItems = false, bool isRequired = false) 
            : base(arrayName, description, isRequired)
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
