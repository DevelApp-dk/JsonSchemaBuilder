using DevelApp.JsonSchemaBuilder.Exceptions;
using DevelApp.JsonSchemaBuilder.Model;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Convenience integer definition in Json Schema.
    /// </summary>
    public class JsonSchemaBuilderInteger : AbstractJsonSchemaBuilderPart<long?>
    {
        public JsonSchemaBuilderInteger(IdentifierString objectName, string description, long? minimum, 
            long? maximum, double? multipleOf = null, long? defaultValue = null, List<long?> examples = null, 
            List<long?> enums = null, bool isRequired = false) 
            : base(objectName, description, isRequired, defaultValue:defaultValue, examples:examples, enums:enums)
        {
            if (minimum.HasValue && DefaultValue.HasValue)
            {
                if (DefaultValue.Value < minimum.Value)
                {
                    throw new JsonSchemaBuilderException($"The default value ({DefaultValue}) supplied is below the minimum ({minimum}) supplied");
                }
            }
            if (maximum.HasValue && DefaultValue.HasValue)
            {
                if (DefaultValue.Value > maximum.Value)
                {
                    throw new JsonSchemaBuilderException($"The default value ({DefaultValue}) supplied is above the maximum ({maximum}) supplied");
                }
            }
            if (multipleOf.HasValue && DefaultValue.HasValue)
            {
                if (((double)DefaultValue.Value) % multipleOf.Value != 0)
                {
                    throw new JsonSchemaBuilderException($"The default value ({DefaultValue}) supplied is not a multiple of multipleOf ({multipleOf}) supplied");
                }
            }

            MultipleOf = multipleOf;
            Minimum = minimum;
            Maximum = maximum;
        }

        public double? MultipleOf { get; }

        public long? Minimum { get; }
        public long? Maximum { get; }

        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.Integer;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            JsonSchema returnSchema = InitialJsonSchema()
                .Type(JsonSchemaType.Integer);

            if (MultipleOf.HasValue)
            {
                returnSchema.MultipleOf(MultipleOf.Value);
            }
            if (Minimum.HasValue)
            {
                returnSchema.Minimum(Minimum.Value);
            }
            if (Maximum.HasValue)
            {
                returnSchema.Maximum(Maximum.Value);
            }
            return returnSchema;
        }
    }
}
