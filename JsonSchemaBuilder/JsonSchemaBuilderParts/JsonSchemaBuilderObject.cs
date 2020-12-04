using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JsonSchemaBuilderObject : AbstractJsonSchemaBuilderPart
    {
        /// <summary>
        /// Convenience schema definition. TopObject means this is the root of the schema. Expandable is used to define if schema can be inherited
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="topHierarchy">Is the top of hierarchy</param>
        /// <param name="expandable"></param>
        /// <returns></returns>
        protected JsonSchema Object(string title, string description, bool topHierarchy = false, bool expandable = false)
        {
            return Factory(topHierarchy)
                .Type(JsonSchemaType.Object)
                .Title(title)
                .Description(description)
                .AdditionalProperties(expandable);
        }


        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.Object;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            throw new NotImplementedException();
        }
    }
}
