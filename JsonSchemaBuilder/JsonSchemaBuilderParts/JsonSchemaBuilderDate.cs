using DevelApp.JsonSchemaBuilder.Model;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Convenience date definition in Json Schema in yyyy-MM-dd format (ISO-8601 compatible)
    /// </summary>
    public class JsonSchemaBuilderDate : JsonSchemaBuilderString
    {
        public JsonSchemaBuilderDate(
            IdentifierString dateName,
            string description,
            DateTime? defaultValue = null,
            bool isRequired = false)
        : base(dateName,
              description,
              format: "date",
              pattern: "^(\\d{4})-(\\d{2})-(\\d{2})$",
              defaultValue: defaultValue.HasValue ? defaultValue.Value.Date.ToString("yyyy-MM-dd") : null,
              isRequired: isRequired)
        {
        }

        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.Date;
            }
        }
    }
}
