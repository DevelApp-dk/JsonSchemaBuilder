using DevelApp.Utility.Model;
using Manatee.Json;
using Manatee.Json.Schema;
using System.Collections.Generic;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Defines JsonSchema object
    /// </summary>
    public class JSBObject : AbstractJSBPart<JsonValue>
    {
        public JSBObject(IdentifierString objectName, string description, 
            List<IJSBPart> props = null, bool isRequired = false, 
            JsonValue defaultValue = null, List<JsonValue> examples = null, List<JsonValue> enums = null, bool isExpandable = false)
            : base(objectName, description, isRequired, defaultValue: defaultValue, examples: examples, enums: enums)
        {
            if (props != null)
            {
                foreach(IJSBPart part in props)
                {
                    Properties.Add(part.Name, part);
                }
            }
            IsExpandable = isExpandable;
        }

        /// <summary>
        /// Stores the properties as children
        /// </summary>
        public Dictionary<IdentifierString, IJSBPart> Properties { get; } = new Dictionary<IdentifierString, IJSBPart>();

        /// <summary>
        /// Can the object be extended with outher properties
        /// </summary>
        public bool IsExpandable { get; }

        public override JSBPartType PartType
        {
            get
            {
                return JSBPartType.Object;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            JsonSchema returnSchema = InitialJsonSchema()
                .Type(JsonSchemaType.Object)
                .AdditionalProperties(IsExpandable);
            //Add properties
            foreach (IJSBPart property in Properties.Values)
            {
                returnSchema.Property(StartWithSmallLetter(property.Name), property.AsJsonSchema());
            }
            //Add required
            List<string> requiredNames = new List<string>();
            foreach (IJSBPart property in Properties.Values)
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
