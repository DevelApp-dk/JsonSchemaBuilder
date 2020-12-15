using DevelApp.Utility.Model;
using Manatee.Json;
using Manatee.Json.Schema;
using System.Collections.Generic;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Defines JsonSchema object
    /// </summary>
    public class JsonSchemaBuilderObject : AbstractJsonSchemaBuilderPart<JsonValue>
    {
        public JsonSchemaBuilderObject(IdentifierString objectName, string description, 
            Dictionary<IdentifierString, IJsonSchemaBuilderPart> properties = null, bool isRequired = false, 
            JsonValue defaultValue = null, List<JsonValue> examples = null, List<JsonValue> enums = null, bool isExpandable = false)
            : base(objectName, description, isRequired, defaultValue: defaultValue, examples: examples, enums: enums)
        {
            if (properties != null)
            {
                Properties = properties;
            }
            else
            {
                Properties = new Dictionary<IdentifierString, IJsonSchemaBuilderPart>();
            }
            IsExpandable = isExpandable;
        }

        /// <summary>
        /// Stores the properties as children
        /// </summary>
        public Dictionary<IdentifierString, IJsonSchemaBuilderPart> Properties { get; }

        /// <summary>
        /// Can the object be extended with outher properties
        /// </summary>
        public bool IsExpandable { get; }

        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.Object;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            JsonSchema returnSchema = InitialJsonSchema()
                .Type(JsonSchemaType.Object)
                .AdditionalProperties(IsExpandable);
            //Add properties
            foreach (IJsonSchemaBuilderPart property in Properties.Values)
            {
                returnSchema.Property(StartWithSmallLetter(property.Name), property.AsJsonSchema());
            }
            //Add required
            List<string> requiredNames = new List<string>();
            foreach (IJsonSchemaBuilderPart property in Properties.Values)
            {
                if (property.IsRequired)
                {
                    requiredNames.Add(StartWithSmallLetter(property.Name));
                }
            }
            if (requiredNames.Count > 0)
            {
                returnSchema.Required(requiredNames.ToArray());
            }
            return returnSchema;
        }
    }
}
