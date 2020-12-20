using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;

namespace DevelApp.JsonSchemaBuilder
{
    public class NoValidationJsonSchema : AbstractJsonSchema
    {
        public NoValidationJsonSchema()
        {
        }

        public override string Description
        {
            get
            {
                return "Represents an empty schema with disabled validation";
            }
        }

        public override NamespaceString Module
        {
            get
            {
                return "DevelApp.Module.Default";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            return new JSBSchema(Name, Description);
        }
    }
}
