using DevelApp.JsonSchemaBuilder.Exceptions;
using DevelApp.JsonSchemaBuilder.Model;
using Manatee.Json;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Ordinary string and parent to all string format types ref http://json-schema.org/understanding-json-schema/reference/string.html
    /// </summary>
    public class JsonSchemaBuilderString : AbstractJsonSchemaBuilderPart
    {
        public JsonSchemaBuilderString(IdentifierString stringName, string description, string format = null, uint minLength = 0, uint? maxLength = null, string pattern = null, string defaultValue = null, bool isRequired = false) : base(stringName, description, isRequired)
        {
            if(!string.IsNullOrWhiteSpace(defaultValue) && (defaultValue.Length < minLength || maxLength.HasValue && defaultValue.Length > maxLength.Value))
            {
                throw new JsonSchemaBuilderException($"The default value ({defaultValue}) supplied in {PartType} is outside the minlength {minLength} and the maxlength {maxLength} defined");
            }
            if(!string.IsNullOrWhiteSpace(defaultValue) && !string.IsNullOrWhiteSpace(pattern))
            {
                if(!Regex.IsMatch(defaultValue, pattern))
                {
                    throw new JsonSchemaBuilderException($"The default value ({defaultValue}) supplied in JsonSchemaBuilder{PartType} does not match the pattern ({pattern}) supplied");
                }
            }
            DefaultValue = defaultValue;
            MinLength = minLength;
            MaxLength = maxLength;
            Pattern = pattern;
            Format = format;
        }

        public uint MinLength { get; }
        public uint? MaxLength { get; }
        public string Pattern { get; }

        public string DefaultValue { get; }

        public string Format { get; }

        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.String;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            JsonSchema returnSchema = new JsonSchema()
                .Type(JsonSchemaType.String)
                .Title(Name)
                .Description(Description);

            if (string.IsNullOrWhiteSpace(Format))
            {
                returnSchema.Format(Format);
            }

            if (string.IsNullOrWhiteSpace(DefaultValue))
            {
                returnSchema.Default(new JsonValue(DefaultValue));
            }

            if (MinLength > 0)
            {
                returnSchema.MinLength(MinLength);
            }

            if (MaxLength.HasValue)
            {
                returnSchema.MaxLength(MaxLength.Value);
            }

            if (string.IsNullOrWhiteSpace(Pattern))
            {
                returnSchema.Pattern(Pattern);
            }

            return returnSchema;
        }
    }
}
