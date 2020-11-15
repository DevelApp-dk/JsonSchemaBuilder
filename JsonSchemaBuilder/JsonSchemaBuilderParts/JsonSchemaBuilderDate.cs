using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JsonSchemaBuilderDate : AbstractJsonSchemaBuilderPart
    {
        /// <summary>
        /// Convenience date definition in Json Schema in yyyy-MM-dd format (ISO-8601 compatible)
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="topHierarchy">Is the top of hierarchy</param>
        /// <returns></returns>
        protected JsonSchema Date(string title, string description, bool topHierarchy = false)
        {
            return Factory(topHierarchy)
                .Type(JsonSchemaType.String)
                .Title(title)
                .Description(description)
                .Format(DateFormatValidator.Instance);
        }




        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.Date;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            throw new NotImplementedException();
        }
    }
}
