using DevelApp.Utility.Model;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Comvinience email definition in Json Schema.
    /// </summary>
    public class JSBEmail : JSBString
    {
        public JSBEmail(
                IdentifierString emailName,
                string description,
                string defaultValue = null,
                bool isRequired = false)
            : base(emailName,
                description,
                format: "email",
                defaultValue: defaultValue,
                pattern: "^[a-z0-9\\._%+!$&*=^|~#%{}/\\-]+@([a-z0-9\\-]+\\.){1,}([a-z]{2,22})$",
                isRequired: isRequired)
        {
        }

        public override JSBPartType PartType
        {
            get
            {
                return JSBPartType.Email;
            }
        }
    }
}
