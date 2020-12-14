using DevelApp.Utility.Model;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Convenience base 64 binary string definition in Json Schema.
    /// </summary>
    public class JsonSchemaBuilderBase64Binary : JsonSchemaBuilderString
    {
        public JsonSchemaBuilderBase64Binary(
                IdentifierString timeName,
                string description,
                uint? charSize = null,
                bool isRequired = false)
            : base(timeName,
                description,
                format: "base64binary",
                minLength: charSize.HasValue ? charSize.Value : 0,
                maxLength: charSize,
                pattern: "^([0-9a-fA-F]{2})*$",
                isRequired: isRequired)
        {
        }

        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.Base64Binary;
            }
        }
    }
}
