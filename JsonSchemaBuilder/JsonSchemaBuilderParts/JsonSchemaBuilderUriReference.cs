﻿using DevelApp.JsonSchemaBuilder.Model;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Add reference "$ref": "./xs.schema.json#/definitions/xs:decimal" with "./xs.schema.json" as the local file and "#/definitions/xs:decimal" getting xs:decimal from the definition
    /// </summary>
    public class JsonSchemaBuilderUriReference : AbstractJsonSchemaBuilderPart<string>
    {
        public JsonSchemaBuilderUriReference(IdentifierString referenceName, string description, string objectReference = null, 
            string localFileLocation = null, string defaultValue = null, List<string> examples = null, List<string> enums = null, bool isRequired = false) 
            : base(referenceName, description, isRequired, defaultValue, examples, enums)
        {
            if(localFileLocation.EndsWith("#"))
            {
                localFileLocation = localFileLocation.Substring(0,localFileLocation.Length-2);
            }
            LocalFileLocation = localFileLocation;
            if(objectReference.StartsWith("#"))
            {
                objectReference = objectReference.Substring(1);
            }
            ObjectReference = objectReference;
        }

        public string ObjectReference { get; }

        public string LocalFileLocation { get; }

        public override JsonSchemaBuilderPartType PartType
        {
            get
            {
                return JsonSchemaBuilderPartType.UriReference;
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            JsonSchema returnSchema = InitialJsonSchema()
                .Type(JsonSchemaType.String);
            if (string.IsNullOrWhiteSpace(LocalFileLocation))
            {
                returnSchema.Ref("#" + ObjectReference);
            }
            else if(string.IsNullOrWhiteSpace(ObjectReference))
            {
                returnSchema.Ref($"{LocalFileLocation}#");
            }
            else
            {
                returnSchema.Ref($"{LocalFileLocation}#{ObjectReference}");
            }
            return returnSchema;
        }

    }
}
