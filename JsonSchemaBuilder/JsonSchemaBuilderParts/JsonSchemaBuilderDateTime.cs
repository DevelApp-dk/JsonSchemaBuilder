using DevelApp.Utility.Model;
using System;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Convenience datetime definition in Json Schema against several formats (ISO-8601 compatible)
    /// </summary>
    public class JsonSchemaBuilderDateTime : JsonSchemaBuilderString
    {
        public JsonSchemaBuilderDateTime(
            IdentifierString dateTimeName, 
            string description, 
            DateTime? defaultValue = null, 
            bool isRequired = false)
        : base(dateTimeName, 
              description, 
              format:"date-time",
              pattern: "^(\\d{4})-(\\d{2})-(\\d{2})T(\\d{2})\\:(\\d{2})\\:(\\d{2})(\\.\\d{1,7})?([+-](\\d{2})\\:(\\d{2}))?$", 
              defaultValue: defaultValue.HasValue? defaultValue.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz"):null,
              isRequired: isRequired)
            {
            }

        public override JsonSchemaBuilderPartType PartType
        { 
            get
            {
                return JsonSchemaBuilderPartType.DateTime;
            }
        }
    }
}
