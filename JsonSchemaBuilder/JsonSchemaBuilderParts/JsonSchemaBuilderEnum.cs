using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JsonSchemaBuilderEnum:AbstractJsonSchemaBuilderPart
    {
        /// <summary>
        /// Convenience enum definition in Json Schema. First value en enumString is the default
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="enumStrings"></param>
        /// <returns></returns>
        protected JsonSchema Enum(string title, string description, bool topHierarchy = false, params string[] enumStrings)
        {
            List<JsonValue> enumJsonValues = new List<JsonValue>();
            foreach (string enumString in enumStrings)
            {
                enumJsonValues.Add(enumString);
            }

            return Factory(topHierarchy)
                .Type(JsonSchemaType.String)
                .Title(title)
                .Description(description)
                .Enum(enumJsonValues.ToArray());
        }

        public override JsonSchemaBuilderPartType PartType => throw new NotImplementedException();

        public override JsonSchema AsJsonSchema()
        {
            throw new NotImplementedException();
        }

    }
}
