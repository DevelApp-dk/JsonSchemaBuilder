using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JsonSchemaBuilderReference : AbstractJsonSchemaBuilderPart
    {
        /// <summary>
        /// Add reference "$ref": "./xs.schema.json#/definitions/xs:decimal" with "./xs.schema.json" as the local file and "#/definitions/xs:decimal" getting xs:decimal from the definition
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="objectReference"></param>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        protected JsonSchema Reference(string title, string description, string objectReference, string fileLocation = "", bool topHierarchy = false)
        {
            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                return Factory(topHierarchy)
                        .Type(JsonSchemaType.Object)
                    .Title(title)
                    .Description(description)
                    .Ref(objectReference);
            }
            else
            {
                return Factory(topHierarchy)
                        .Type(JsonSchemaType.Object)
                    .Title(title)
                    .Description(description)
                    .Ref(fileLocation + objectReference);
            }
        }
        public override JsonSchemaBuilderPartType PartType => throw new NotImplementedException();

        public override JsonSchema AsJsonSchema()
        {
            throw new NotImplementedException();
        }
    }
}
