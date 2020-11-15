using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JsonSchemaBuilderArray : AbstractJsonSchemaBuilderPart
    {
        /// <summary>
        /// Convenience array definition in Json Schema. Requires Items definition.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="topHierarchy">Is the top of hierarchy</param>
        /// <returns></returns>
        protected JsonSchema Array(string title, string description, bool topHierarchy = false)
        {
            return Factory(topHierarchy)
                .Type(JsonSchemaType.Array)
                .Title(title)
                .Description(description);
        }

        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.Array;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            throw new NotImplementedException();
        }
    }
}
