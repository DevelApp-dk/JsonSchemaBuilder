﻿using DevelApp.JsonSchemaBuilder.Exceptions;
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
    public class JsonSchemaBuilderInteger : AbstractJsonSchemaBuilderPart
    {
        public JsonSchemaBuilderInteger(IdentifierString objectName, string description, long? minimum, long? maximum, double? multipleOf = null, long? defaultValue = null, bool isRequired = false) : base(objectName, description, isRequired)
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
                if ((double)defaultValue.Value % multipleOf.Value != 0)
                {
                    throw new JsonSchemaBuilderException($"The default value ({defaultValue}) supplied is not a multiple of multipleOf ({multipleOf}) supplied");
                }
            }

            DefaultValue = defaultValue;
            MultipleOf = multipleOf;
            Minimum = minimum;
            Maximum = maximum;
        }

        public long? DefaultValue { get; }

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
            JsonSchema returnSchema = new JsonSchema()
                .Type(JsonSchemaType.Integer)
                .Title(Name)
                .Description(Description);

            if (DefaultValue.HasValue)
            {
                returnSchema.Default(new Manatee.Json.JsonValue(DefaultValue));
            }
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
