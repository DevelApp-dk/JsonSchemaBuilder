﻿using DevelApp.Utility.Model;
using System;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Convenience time definition in Json Schema against several formats (ISO-8601 compatible)
    /// </summary>
    public class JsonSchemaBuilderTime : JsonSchemaBuilderString
    {
        public JsonSchemaBuilderTime(
            IdentifierString timeName, 
            string description, 
            DateTime? defaultValue = null, 
            bool isRequired = false)
        : base(timeName, 
              description, 
              format:"time",
              pattern: "^(\\d{2})\\:(\\d{2})\\:(\\d{2})(\\.\\d{1,7})?([+-](\\d{2})\\:(\\d{2}))?$", 
              defaultValue: defaultValue.HasValue? defaultValue.Value.ToString("HH:mm:ss.fffffffzzz"):null,
              isRequired: isRequired)
            {
            }

        public override JsonSchemaBuilderPartType PartType
        { 
            get
            {
                return JsonSchemaBuilderPartType.Time;
            }
        }
    }
}
