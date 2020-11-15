using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JsonSchemaBuilderDateTime : AbstractJsonSchemaBuilderPart
    {
        /// <summary>
        /// Convenience datetime definition in Json Schema against several formats (ISO-8601 compatible)
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="topHierarchy">Is the top of hierarchy</param>
        /// <returns></returns>
        protected JsonSchema DateTime(string title, string description, bool topHierarchy = false)
        {
            return Factory(topHierarchy)
                .Type(JsonSchemaType.String)
                .Title(title)
                .Description(description)
                .Format(DateTimeFormatValidator.Instance);
        }

        public override JsonSchemaBuilderPartType PartType
        { 
            get
            {
                return JsonSchemaBuilderPartType.DateTime;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            throw new NotImplementedException();
        }
    }
}
