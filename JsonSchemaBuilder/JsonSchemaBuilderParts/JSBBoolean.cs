using DevelApp.Utility.Model;
using Manatee.Json.Schema;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public class JSBBoolean : AbstractJSBPart<bool?>
    {
        public JSBBoolean(IdentifierString boolName, string description, 
            bool? defaultValue = null, bool isRequired = false) : 
            base(boolName, description, isRequired, defaultValue: defaultValue, examples: null, enums: null)
        {
        }


        public override JSBPartType PartType
        {
            get
            {
                return JSBPartType.Boolean;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            JsonSchema returnSchema = InitialJsonSchema()
                .Type(JsonSchemaType.Boolean);

            return returnSchema;
        }
    }
}
