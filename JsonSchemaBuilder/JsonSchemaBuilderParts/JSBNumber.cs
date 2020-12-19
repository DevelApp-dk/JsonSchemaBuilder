using DevelApp.JsonSchemaBuilder.Exceptions;
using DevelApp.Utility.Model;
using Manatee.Json.Schema;
using System.Collections.Generic;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Convenience number definition in Json Schema.
    /// </summary>
    public class JSBNumber : AbstractJSBPart<double?>
    {
        public JSBNumber(IdentifierString objectName, string description, double? minimum = null, double? maximum = null, 
            double? multipleOf = null, double? defaultValue = null, List<double?> examples = null,
            List<double?> enums = null, bool isRequired = false)
            : base(objectName, description, isRequired, defaultValue: defaultValue, examples: examples, enums: enums)
        {
            if (minimum.HasValue && defaultValue.HasValue)
            {
                if (defaultValue.Value < minimum.Value)
                {
                    throw new JsonSchemaBuilderException($"The default value ({defaultValue}) supplied is below the minimum ({minimum}) supplied");
                }
            }
            if (maximum.HasValue && defaultValue.HasValue)
            {
                if (defaultValue.Value > maximum.Value)
                {
                    throw new JsonSchemaBuilderException($"The default value ({defaultValue}) supplied is above the maximum ({maximum}) supplied");
                }
            }
            if (multipleOf.HasValue && defaultValue.HasValue)
            {
                if (defaultValue.Value % multipleOf.Value != 0)
                {
                    throw new JsonSchemaBuilderException($"The default value ({defaultValue}) supplied is not a multiple of multipleOf ({multipleOf}) supplied");
                }
            }

            MultipleOf = multipleOf;
            Minimum = minimum;
            Maximum = maximum;
        }

        public double? MultipleOf { get; }

        public double? Minimum { get; }
        public double? Maximum { get; }

        public override JSBPartType PartType
        {
            get
            {
                return JSBPartType.Number;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            JsonSchema returnSchema = InitialJsonSchema()
                .Type(JsonSchemaType.Number);

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
