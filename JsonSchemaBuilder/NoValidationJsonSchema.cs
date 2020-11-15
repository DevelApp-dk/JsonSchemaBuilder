using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder
{
    public class NoValidationJsonSchema : AbstractJsonSchema
    {
        public NoValidationJsonSchema()
        {
            JsonSchema = Manatee.Json.Schema.JsonSchema.Empty;
        }

        public override string Description
        {
            get
            {
                return "Represents an empty schema with disabled validation";
            }
        }

        public override string Module
        {
            get
            {
                return "Default";
            }
        }
    }
}
