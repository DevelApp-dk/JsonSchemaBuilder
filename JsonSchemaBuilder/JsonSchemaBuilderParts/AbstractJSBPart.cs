using DevelApp.JsonSchemaBuilder.Exceptions;
using DevelApp.Utility.Model;
using Manatee.Json;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public abstract class AbstractJSBPart<T> : IJSBPart
    {
        public AbstractJSBPart(IdentifierString name, string description, bool isRequired, 
            T defaultValue, List<T> examples, List<T> enums)
        {
            var type = typeof(T);
            _tIsString = type.Name.Equals("String");
            _tIsJsonValue = type.Name.Equals("JsonValue");
            _tIsNullable = Nullable.GetUnderlyingType(type) != null;
            if (!(_tIsString || _tIsJsonValue || _tIsNullable))
            {
                throw new JsonSchemaBuilderException($"Only allowed types are nullable or classes string and JsonValue as direct use of value types gives errors");
            }

            Name = name;
            Description = description;
            IsRequired = isRequired;
            if (!Equals(defaultValue, default(T)))
            {
                DefaultValue = defaultValue;
            }
            if (examples == null)
            {
                Examples = new List<T>();
            }
            else
            {
                Examples = examples;
            }
            if (enums == null)
            {
                Enums = new List<T>();
            }
            else
            {
                Enums = enums;
            }
        }

        private bool _tIsString;
        private bool _tIsJsonValue;
        private bool _tIsNullable;

        public IdentifierString Name { get; }

        public string Description { get; }

        public bool IsRequired { get; }

        public T DefaultValue { get; }

        public List<T> Examples { get; }

        public List<T> Enums { get; }

        public abstract JSBPartType PartType { get; }

        public abstract JsonSchema AsJsonSchema();

        protected JsonSchema InitialJsonSchema()
        {
            JsonSchema returnSchema = new JsonSchema()
                .Title(Name)
                .Description(Description)
                .Comment($"Generated with JsonSchemaBuilder");
            if (Examples != null)
            {
                List<JsonValue> examplesOut = new List<JsonValue>();
                foreach (T item in Examples)
                {
                    examplesOut.Add(TAsJsonValue(item));
                }
                returnSchema.Examples(examplesOut.ToArray());
            }
            if (Enums != null)
            {
                List<JsonValue> enumsOut = new List<JsonValue>();
                foreach (T item in Enums)
                {
                    enumsOut.Add(TAsJsonValue(item));
                }
                returnSchema.Enum(enumsOut.ToArray());
            }

            return returnSchema;
        }

        private JsonValue TAsJsonValue(T item)
        {
            if (_tIsJsonValue)
            {
                return item as JsonValue;
            }
            else if (_tIsString)
            {
                return item as string;
            }
            else if (_tIsNullable)
            {
                if (item is bool?)
                {
                    return new JsonValue((item as bool?).Value);
                }
                if (item is long?)
                {
                    return new JsonValue((item as long?).Value);
                }
                if (item is double?)
                {
                    return new JsonValue((item as double?).Value);
                }
                throw new JsonSchemaBuilderException($"The nullable used ({typeof(T).Name}) is not supported");
            }
            throw new JsonSchemaBuilderException($"Should not get here as exception has been thrown in contructor");
        }


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
