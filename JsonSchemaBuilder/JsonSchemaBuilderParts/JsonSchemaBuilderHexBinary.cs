using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JsonSchemaBuilderHexBinary : AbstractJsonSchemaBuilderPart
    {
        /// <summary>
        /// Convenience hexbinary definition in Json Schema.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="charSize"></param>
        /// <returns></returns>
        protected JsonSchema HexBinary(string title, string description, int charSize, bool topHierarchy = false)
        {
            return Factory(topHierarchy)
                .Type(JsonSchemaType.String)
                .Title(title)
                .Description(description)
                .MinLength((uint)charSize)
                .MaxLength((uint)charSize)
                .Pattern("^([0-9a-fA-F]{2})*$");
        }


        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.HexBinary;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            throw new NotImplementedException();
        }
    }
}
