using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JsonSchemaBuilderBoolean : AbstractJsonSchemaBuilderPart
    {
        /// <summary>
        /// Convenience bool definition in Json Schema. Default value is false if not provided
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        protected JsonSchema Boolean(string title, string description, bool topHierarchy = false)
        {
            return Factory(topHierarchy)
                    .Type(JsonSchemaType.Boolean)
                    .Title(title)
                    .Description(description);
        }



        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.Boolean;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            throw new NotImplementedException();
        }
    }
}
