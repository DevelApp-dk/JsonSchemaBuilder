using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JsonSchemaBuilderSchema : AbstractJsonSchemaBuilderPart
    {
        private JsonSchema Factory(bool topHierarchy)
        {
            if (topHierarchy)
            {
                return new JsonSchema()
                    .Id(Name)
                    .Schema("http://json-schema.org/draft-07/schema#");
            }
            else
            {
                return new JsonSchema();
            }
        }


        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.Schema;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            throw new NotImplementedException();
        }
    }
}
