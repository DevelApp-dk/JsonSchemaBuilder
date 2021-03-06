﻿using DevelApp.JsonSchemaBuilder.Exceptions;
using DevelApp.Utility.Model;
using Manatee.Json.Schema;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Ordinary string and parent to all string format types ref http://json-schema.org/understanding-json-schema/reference/string.html
    /// </summary>
    public class JSBString : AbstractJSBPart<string>
    {
        public JSBString(IdentifierString stringName, string description, string format = null, uint minLength = 0, 
            uint? maxLength = null, string pattern = null, string defaultValue = null, List<string> examples = null, List<string> enums = null, bool isRequired = false) 
            : base(stringName, description, isRequired, defaultValue, examples, enums)
        {
            if(!string.IsNullOrWhiteSpace(DefaultValue) && (DefaultValue.Length < minLength || maxLength.HasValue && DefaultValue.Length > maxLength.Value))
            {
                throw new JsonSchemaBuilderException($"The default value ({DefaultValue}) supplied in {PartType} is outside the minlength {minLength} and the maxlength {maxLength} defined");
            }
            if(!string.IsNullOrWhiteSpace(DefaultValue) && !string.IsNullOrWhiteSpace(pattern))
            {
                if(!Regex.IsMatch(DefaultValue, pattern))
                {
                    throw new JsonSchemaBuilderException($"The default value ({DefaultValue}) supplied in JsonSchemaBuilder{PartType} does not match the pattern ({pattern}) supplied");
                }
            }
            MinLength = minLength;
            MaxLength = maxLength;
            Pattern = pattern;
            Format = format;
        }

        public uint MinLength { get; }
        public uint? MaxLength { get; }
        public string Pattern { get; }

        public string Format { get; }

        public override JSBPartType PartType
        {
            get
            {
                return JSBPartType.String;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            JsonSchema returnSchema = InitialJsonSchema()
                .Type(JsonSchemaType.String);

            if (string.IsNullOrWhiteSpace(Format))
            {
                returnSchema.Format(Format);
            }

            if (MinLength > 0)
            {
                returnSchema.MinLength(MinLength);
            }

            if (MaxLength.HasValue)
            {
                returnSchema.MaxLength(MaxLength.Value);
            }

            if (!string.IsNullOrWhiteSpace(Pattern))
            {
                returnSchema.Pattern(Pattern);
            }

            if(!string.IsNullOrWhiteSpace(Format))
            {
                returnSchema.Format(Format);
            }

            return returnSchema;
        }
    }
}
