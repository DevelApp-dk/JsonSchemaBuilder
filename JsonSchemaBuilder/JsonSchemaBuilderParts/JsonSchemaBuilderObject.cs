﻿using DevelApp.JsonSchemaBuilder.Model;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Defines JsonSchema object
    /// </summary>
    public class JsonSchemaBuilderObject : AbstractJsonSchemaBuilderPart
    {
        public JsonSchemaBuilderObject(IdentifierString objectName, string description, Dictionary<IdentifierString, IJsonSchemaBuilderPart> properties = null, bool isRequired = false, bool isExpandable = false) : base(objectName, description, isRequired)
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
            JsonSchema returnSchema = new JsonSchema()
                .Type(JsonSchemaType.Object)
                .Title(Name)
                .Description(Description)
                .AdditionalProperties(IsExpandable);
            //Add properties
            foreach (IJsonSchemaBuilderPart property in Properties.Values)
            {
                returnSchema.Properties().Add(StartWithSmallLetter(property.Name),property.AsJsonSchema() );
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
