using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JsonSchemaBuilderInteger : AbstractJsonSchemaBuilderPart
    {
        /// <summary>
        /// Convenience integer definition in Json Schema.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="topHierarchy">Is the top of hierarchy</param>
        /// <returns></returns>
        protected JsonSchema Integer(string title, string description, bool topHierarchy = false)
        {
            return Factory(topHierarchy)
                .Type(JsonSchemaType.Integer)
                .Title(title)
                .Description(description);
        }



        public override JsonSchemaBuilderPartType PartType => throw new NotImplementedException();

        public override JsonSchema AsJsonSchema()
        {
            throw new NotImplementedException();
        }
    }
}
