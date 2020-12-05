using DevelApp.JsonSchemaBuilder.Exceptions;
using DevelApp.JsonSchemaBuilder.Model;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public abstract class AbstractJsonSchemaBuilderPart : IJsonSchemaBuilderPart
    {
        public AbstractJsonSchemaBuilderPart(IdentifierString name, string description, bool isRequired)
        {
            Name = name;
            Description = description;
            IsRequired = isRequired;
        }

        public IdentifierString Name { get; }

        public string Description { get; }

        public bool IsRequired { get; }


        public abstract JsonSchemaBuilderPartType PartType { get; }

        public abstract JsonSchema AsJsonSchema();

        /// <summary>
        /// Returns a identifier with small letter as start
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string StartWithSmallLetter(IdentifierString name)
        {
            return name.ToString().Substring(0, 1).ToLowerInvariant() + name.ToString().Substring(1);
        }
    }
}
