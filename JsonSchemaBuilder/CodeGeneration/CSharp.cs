using DevelApp.JsonSchemaBuilder.Exceptions;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using Manatee.Json;
using Manatee.Json.Schema;
using Manatee.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DevelApp.JsonSchemaBuilder.CodeGeneration
{
    public class CSharp
    {
        private const string FILE_ENDING = ".cs";

        /// <summary>
        /// Generate fileName and code
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        internal static (string fileName, string code) GenerateCode(string fileLocation, IJsonSchemaDefinition schema)
        {
            string fileName = fileLocation + FILE_ENDING;

            CSharp cSharp = new CSharp(schema.Module);
            return (fileName, cSharp.GenerateCode(schema.JsonSchemaBuilderSchema));
        }

        private NamespaceString _startNameSpace;
        private CSharp(NamespaceString startNameSpace)
        {
            _startNameSpace = startNameSpace;
        }

        /// <summary>
        /// Generate code from JsonSchemaBuilderSchema
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        private string GenerateCode(JsonSchemaBuilderSchema schema)
        {
            CodeBuilder codeBuilder = new CodeBuilder();
            GenerateStartOfSchema(codeBuilder, schema);
            if (schema.TopPart.PartType == JsonSchemaBuilderPartType.Object)
            {
                GenerateCodeForBuilderPart(codeBuilder, schema.TopPart.Name, schema.TopPart, schema.Definitions);
            }
            else
            {
                Dictionary<IdentifierString, IJsonSchemaBuilderPart> properties = new Dictionary<IdentifierString, IJsonSchemaBuilderPart>();
                properties.Add(schema.TopPart.Name, schema.TopPart);

                JsonSchemaBuilderObject encasingObject = new JsonSchemaBuilderObject(schema.Name, schema.Description, properties: properties);
                GenerateCodeForBuilderPart(codeBuilder, schema.Name, encasingObject, schema.Definitions);
            }
            GenerateEndOfSchema(codeBuilder, schema);
            return codeBuilder.Build();
        }

        /// <summary>
        /// Generates using parameters and initial class specification
        /// </summary>
        /// <param name="codeBuilder"></param>
        /// <param name="schema"></param>
        private void GenerateStartOfSchema(CodeBuilder codeBuilder, JsonSchemaBuilderSchema schema)
        {
            //TODO run through schema children to get all references to make sure they are included
            codeBuilder
                .L("using System;")
                .L("using Newtonsoft.Json;")
                .L("using Newtonsoft.Json.Converters;")
                .L("using Ardalis.SmartEnum;")
                .L("using System.Collections.Generic;")
                .L("using System.Net.Mail;")
                .EmptyLine()
                .L($"namespace {_startNameSpace}")
                .L("{")
                .IndentIncrease();
        }

        private void GenerateEndOfSchema(CodeBuilder codeBuilder, JsonSchemaBuilderSchema schema)
        {
            codeBuilder
                .IndentDecrease()
                .L("}");
        }

        private void GenerateCodeForBuilderPart(CodeBuilder codeBuilder, IdentifierString key, IJsonSchemaBuilderPart value, Dictionary<IdentifierString, IJsonSchemaBuilderPart> definitions = null, bool parentIsArray = false)
        {
            switch (value.PartType)
            {
                case JsonSchemaBuilderPartType.Array:
                    JsonSchemaBuilderArray jsonSchemaBuilderArray = value as JsonSchemaBuilderArray;
                    if(jsonSchemaBuilderArray.Enums != null && jsonSchemaBuilderArray.Enums.Count > 0)
                    {
                        GenerateEnumArray(codeBuilder, key, jsonSchemaBuilderArray);
                    }
                    else
                    {
                        GenerateOrdinaryArray(codeBuilder, key, jsonSchemaBuilderArray);
                    }
                    break;
                case JsonSchemaBuilderPartType.Boolean:
                        JsonSchemaBuilderBoolean jsonSchemaBuilderBoolean = value as JsonSchemaBuilderBoolean;
                    if (jsonSchemaBuilderBoolean.Enums != null && jsonSchemaBuilderBoolean.Enums.Count > 0)
                    {
                        GenerateEnumBoolean(codeBuilder, key, jsonSchemaBuilderBoolean);
                    }
                    else
                    {
                        if (!parentIsArray)
                        {
                            GenerateOrdinaryBoolaen(codeBuilder, key, jsonSchemaBuilderBoolean);
                        }
                    }
                    break;
                case JsonSchemaBuilderPartType.Date:
                    JsonSchemaBuilderDate jsonSchemaBuilderDate = value as JsonSchemaBuilderDate;
                    if(jsonSchemaBuilderDate.Enums != null && jsonSchemaBuilderDate.Enums.Count > 0)
                    {
                        GenerateEnumDate(codeBuilder, key, jsonSchemaBuilderDate);
                    }
                    else
                    {
                        GenerateOrdinaryDate(codeBuilder, key, jsonSchemaBuilderDate);
                    }
                    break;
                case JsonSchemaBuilderPartType.DateTime:
                    JsonSchemaBuilderDateTime jsonSchemaBuilderDateTime = value as JsonSchemaBuilderDateTime;
                    if(jsonSchemaBuilderDateTime.Enums != null && jsonSchemaBuilderDateTime.Enums.Count > 0)
                    {
                        GenerateEnumDateTime(codeBuilder, key, jsonSchemaBuilderDateTime);
                    }
                    else
                    {
                        if (!parentIsArray)
                        {
                            GenerateOrdinaryDateTime(codeBuilder, key, jsonSchemaBuilderDateTime);
                        }
                    }
                    break;
                case JsonSchemaBuilderPartType.Email:
                    JsonSchemaBuilderEmail jsonSchemaBuilderEmail = value as JsonSchemaBuilderEmail;
                    if(jsonSchemaBuilderEmail.Enums != null && jsonSchemaBuilderEmail.Enums.Count > 0)
                    {
                        GenerateEnumEmail(codeBuilder, key, jsonSchemaBuilderEmail);
                    }
                    else
                    {
                        GenerateOrdinaryEmail(codeBuilder, key, jsonSchemaBuilderEmail);
                    }
                    break;
                case JsonSchemaBuilderPartType.Integer:
                    JsonSchemaBuilderInteger jsonSchemaBuilderInteger = value as JsonSchemaBuilderInteger;
                    if(jsonSchemaBuilderInteger.Enums != null && jsonSchemaBuilderInteger.Enums.Count > 0)
                    {
                        GenerateEnumInteger(codeBuilder, key, jsonSchemaBuilderInteger);
                    }
                    else
                    {
                        if (!parentIsArray)
                        {
                            GenerateOrdinaryInteger(codeBuilder, key, jsonSchemaBuilderInteger);
                        }
                    }
                    break;
                case JsonSchemaBuilderPartType.Number:
                    JsonSchemaBuilderNumber jsonSchemaBuilderNumber = value as JsonSchemaBuilderNumber;
                    if(jsonSchemaBuilderNumber.Enums != null && jsonSchemaBuilderNumber.Enums.Count > 0)
                    {
                        GenerateEnumNumber(codeBuilder, key, jsonSchemaBuilderNumber);
                    }
                    else
                    {
                        if (!parentIsArray)
                        {
                            GenerateOrdinaryNumber(codeBuilder, key, jsonSchemaBuilderNumber);
                        }
                    }
                    break;
                case JsonSchemaBuilderPartType.Object:
                    JsonSchemaBuilderObject jsonSchemaBuilderObject = value as JsonSchemaBuilderObject;
                    if(jsonSchemaBuilderObject.Enums != null && jsonSchemaBuilderObject.Enums.Count > 0)
                    {
                        GenerateEnumObject(codeBuilder, key, jsonSchemaBuilderObject, definitions);
                    }
                    else
                    {
                        GenerateOrdinaryObject(codeBuilder, key, jsonSchemaBuilderObject, definitions, parentIsArray);
                    }
                    break;
                case JsonSchemaBuilderPartType.String:
                    JsonSchemaBuilderString jsonSchemaBuilderString = value as JsonSchemaBuilderString;
                    if(jsonSchemaBuilderString.Enums != null && jsonSchemaBuilderString.Enums.Count > 0)
                    {
                        GenerateEnumString(codeBuilder, key, jsonSchemaBuilderString);
                    }
                    else
                    {
                        GenerateOrdinaryString(codeBuilder, key, jsonSchemaBuilderString);
                    }
                    break;
                case JsonSchemaBuilderPartType.Time:
                    JsonSchemaBuilderTime jsonSchemaBuilderTime = value as JsonSchemaBuilderTime;
                    if(jsonSchemaBuilderTime.Enums != null && jsonSchemaBuilderTime.Enums.Count > 0)
                    {
                        GenerateEnumTime(codeBuilder, key, jsonSchemaBuilderTime);
                    }
                    else
                    {
                        GenerateOrdinaryTime(codeBuilder, key, jsonSchemaBuilderTime);
                    }
                    break;
                case JsonSchemaBuilderPartType.UriReference:
                    JsonSchemaBuilderUriReference jsonSchemaBuilderUriReference = value as JsonSchemaBuilderUriReference;
                    if(jsonSchemaBuilderUriReference.Enums != null && jsonSchemaBuilderUriReference.Enums.Count > 0)
                    {
                        GenerateEnumUriReference(codeBuilder, key, jsonSchemaBuilderUriReference);
                    }
                    else
                    {
                        GenerateOrdinaryUriReference(codeBuilder, key, jsonSchemaBuilderUriReference);
                    }
                    break;
                default:
                    codeBuilder.L($"throw new NotImplementedException(\"PartType {value.PartType} is not implemented\");");
                    break;
            }
        }

        #region Generate Array

        private void GenerateEnumArray(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderArray jsonSchemaBuilderArray)
        {
            throw new NotImplementedException();
        }

        private void GenerateOrdinaryArray(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderArray jsonSchemaBuilderArray)
        {
            foreach(IJsonSchemaBuilderPart part in jsonSchemaBuilderArray.Items)
            {
                GenerateCodeForBuilderPart(codeBuilder, part.Name, part, parentIsArray: true);
            }
            GenerateComments(codeBuilder, key, jsonSchemaBuilderArray);

            if (jsonSchemaBuilderArray.Items.Count == 1)
            {
                codeBuilder
                    .L($"[JsonProperty(\"{TransformToCamelCase(key)}\")]")
                    .L($"public List<{MakeCorrectItemType(jsonSchemaBuilderArray.Items[0])}> {TransformToTitleCase(key)} {{ get; set; }}{GenerateDefaultIfExisting(key, jsonSchemaBuilderArray)}")
                    .EmptyLine();
            }
            else
            {
                throw new NotImplementedException("Array with more than one item is not implemented");
            }
        }

        private string MakeCorrectItemType(IJsonSchemaBuilderPart jsonSchemaBuilderPart)
        {
            switch(jsonSchemaBuilderPart.PartType)
            {
                case JsonSchemaBuilderPartType.Array:
                    //TODO support multiple items
                    return $"List<{MakeCorrectItemType((jsonSchemaBuilderPart as JsonSchemaBuilderArray).Items.First())}>";
                case JsonSchemaBuilderPartType.Object:
                    return jsonSchemaBuilderPart.Name;
                case JsonSchemaBuilderPartType.Integer:
                    return "long";
                case JsonSchemaBuilderPartType.Number:
                    return "double";
                case JsonSchemaBuilderPartType.Boolean:
                    return "bool";
                case JsonSchemaBuilderPartType.String:
                    return "string";
                case JsonSchemaBuilderPartType.Date:
                    return "DateTime";
                case JsonSchemaBuilderPartType.DateTime:
                    return "DateTime";
                case JsonSchemaBuilderPartType.Email:
                    return "Email";
                case JsonSchemaBuilderPartType.Time:
                    return "DateTime";
                default:
                    throw new NotImplementedException();
            }

        }

        #endregion

        #region Generate Boolean

        private void GenerateOrdinaryBoolaen(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderBoolean jsonSchemaBuilderBoolean)
        {
            GenerateComments(codeBuilder, key, jsonSchemaBuilderBoolean);

            codeBuilder
                .L($"[JsonProperty(\"{TransformToCamelCase(key)}\")]")
                .L($"public bool{BuildRequired(jsonSchemaBuilderBoolean.IsRequired)} {TransformToTitleCase(key)} {{ get; set; }}{GenerateDefaultIfExisting(key, jsonSchemaBuilderBoolean)}")
                .EmptyLine();
        }

        private void GenerateEnumBoolean(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderBoolean jsonSchemaBuilderBoolean)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Generate Date

        private void GenerateOrdinaryDate(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderDate jsonSchemaBuilderDate)
        {
            GenerateComments(codeBuilder, key, jsonSchemaBuilderDate);

            codeBuilder
                .L($"[JsonProperty(\"{TransformToCamelCase(key)}\")]")
                .L($"public DateTime{BuildRequired(jsonSchemaBuilderDate.IsRequired)} {TransformToTitleCase(key)} {{ get; set; }}{GenerateDefaultIfExisting(key, jsonSchemaBuilderDate)}")
                .EmptyLine();
        }

        private void GenerateEnumDate(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderDate jsonSchemaBuilderDate)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Generate DateTime

        private void GenerateOrdinaryDateTime(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderDateTime jsonSchemaBuilderDateTime)
        {
            GenerateComments(codeBuilder, key, jsonSchemaBuilderDateTime);

            codeBuilder
                .L($"[JsonProperty(\"{TransformToCamelCase(key)}\")]")
                .L($"public DateTime{BuildRequired(jsonSchemaBuilderDateTime.IsRequired)} {TransformToTitleCase(key)} {{ get; set; }}{GenerateDefaultIfExisting(key, jsonSchemaBuilderDateTime)}")
                .EmptyLine();
        }

        private void GenerateEnumDateTime(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderDateTime jsonSchemaBuilderDateTime)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Generate Email

        private void GenerateEnumEmail(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderEmail jsonSchemaBuilderEmail)
        {
            throw new NotImplementedException();
        }

        private void GenerateOrdinaryEmail(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderEmail jsonSchemaBuilderEmail)
        {
            GenerateComments(codeBuilder, key, jsonSchemaBuilderEmail);

            codeBuilder
                .L($"[JsonProperty(\"{TransformToCamelCase(key)}\")]")
                .L($"public MailAddress {TransformToTitleCase(key)} {{ get; set; }}{GenerateDefaultIfExisting(key, jsonSchemaBuilderEmail)}")
                .EmptyLine();
        }

        #endregion

        #region Generate Integer

        private void GenerateOrdinaryInteger(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderInteger jsonSchemaBuilderInteger)
        {
            GenerateComments(codeBuilder, key, jsonSchemaBuilderInteger);

            codeBuilder
                .L($"[JsonProperty(\"{TransformToCamelCase(key)}\")]")
                .L($"public long{BuildRequired(jsonSchemaBuilderInteger.IsRequired)} {TransformToTitleCase(key)} {{ get; set; }}{GenerateDefaultIfExisting(key, jsonSchemaBuilderInteger)}")
                .EmptyLine();
        }

        private void GenerateEnumInteger(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderInteger jsonSchemaBuilderInteger)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Generate Number

        private void GenerateOrdinaryNumber(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderNumber jsonSchemaBuilderNumber)
        {
            GenerateComments(codeBuilder, key, jsonSchemaBuilderNumber);

            codeBuilder
                .L($"[JsonProperty(\"{TransformToCamelCase(key)}\")]")
                .L($"public double{BuildRequired(jsonSchemaBuilderNumber.IsRequired)} {TransformToTitleCase(key)} {{ get; set; }}{GenerateDefaultIfExisting(key, jsonSchemaBuilderNumber)}")
                .EmptyLine();
        }

        private void GenerateEnumNumber(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderNumber jsonSchemaBuilderNumber)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Generate Objects

        private void GenerateEnumObject(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderObject jsonSchemaBuilderObject, Dictionary<IdentifierString, IJsonSchemaBuilderPart> definitions)
        {
            throw new NotImplementedException();
        }

        private void GenerateOrdinaryObject(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderObject jsonSchemaBuilderObject, Dictionary<IdentifierString, IJsonSchemaBuilderPart> definitions, bool parentIsArray)
        {
            GenerateComments(codeBuilder, key, jsonSchemaBuilderObject);

            codeBuilder
                .L($"public partial class {TransformToTitleCase(key)}")
                .L("{")
                .IndentIncrease();

            //Add definitions
            if (definitions != null)
            {
                foreach (KeyValuePair<IdentifierString, IJsonSchemaBuilderPart> pair in definitions)
                {
                    GenerateCodeForBuilderPart(codeBuilder, pair.Key, pair.Value);
                    codeBuilder.EmptyLine();
                }
            }

            //TODO Add own code

            //Add children
            foreach (KeyValuePair<IdentifierString, IJsonSchemaBuilderPart> pair in jsonSchemaBuilderObject.Properties)
            {
                GenerateCodeForBuilderPart(codeBuilder, pair.Key, pair.Value);
                codeBuilder.EmptyLine();
            }

            codeBuilder
                .IndentDecrease()
                .L("}");


            if (definitions == null && !parentIsArray)// Assume not top part
            {
                codeBuilder
                    .L($"[JsonProperty(\"{TransformToCamelCase(key)}\")]")
                    .L($"public {TransformToTitleCase(key)} {TransformToTitleCase(key)}Prop {{ get; set; }} ")
                    .EmptyLine();
            }
        }

        #endregion

        #region Generate String

        private void GenerateEnumString(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderString jsonSchemaBuilderString)
        {
            codeBuilder
                .L($"public enum {TransformToTitleCase(key)}Enum")
                .L("{")
                .IndentIncrease();
            for(int counter = 0;counter < jsonSchemaBuilderString.Enums.Count;counter += 1)
            {
                bool last = counter + 1 == jsonSchemaBuilderString.Enums.Count;
                string enumString = TransformToTitleCase(jsonSchemaBuilderString.Enums[counter]) + (last?"":",");
                codeBuilder
                    .L(enumString);
            }
            codeBuilder
                .IndentDecrease()
                .L("}")
                .EmptyLine();
            GenerateComments(codeBuilder, key, jsonSchemaBuilderString);
            codeBuilder
                .L($"[JsonProperty(\"{TransformToCamelCase(key)}\"), JsonConverter(typeof(StringEnumConverter))]") 
                .L($"public {TransformToTitleCase(key)}Enum {TransformToTitleCase(key)} {{ get; set; }}{GenerateDefaultIfExisting(key, jsonSchemaBuilderString)}")
                .EmptyLine();
        }

        private void GenerateOrdinaryString(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderString jsonSchemaBuilderString)
        {
            GenerateComments(codeBuilder, key, jsonSchemaBuilderString);

            codeBuilder
                .L($"[JsonProperty(\"{TransformToCamelCase(key)}\")]")
                .L($"public string {TransformToTitleCase(key)} {{ get; set; }}{GenerateDefaultIfExisting(key, jsonSchemaBuilderString)}")
                .EmptyLine();
        }

        #endregion

        #region Generate Time

        private void GenerateEnumTime(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderTime jsonSchemaBuilderTime)
        {
            throw new NotImplementedException();
        }

        private void GenerateOrdinaryTime(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderTime jsonSchemaBuilderTime)
        {
            GenerateComments(codeBuilder, key, jsonSchemaBuilderTime);

            codeBuilder
                .L($"[JsonProperty(\"{TransformToCamelCase(key)}\")]")
                .L($"public DateTime{BuildRequired(jsonSchemaBuilderTime.IsRequired)} {TransformToTitleCase(key)} {{ get; set; }}{GenerateDefaultIfExisting(key, jsonSchemaBuilderTime)}")
                .EmptyLine();
        }

        #endregion

        #region Generate Uri Reference

        private void GenerateEnumUriReference(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderUriReference jsonSchemaBuilderUriReference)
        {
            throw new NotImplementedException();
        }

        private void GenerateOrdinaryUriReference(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderUriReference jsonSchemaBuilderUriReference)
        {
            try
            {
                if (Uri.TryCreate(jsonSchemaBuilderUriReference.LocalFileLocation, UriKind.RelativeOrAbsolute, out Uri uri))
                {
                    if (uri.IsLoopback && uri.IsFile)
                    {
                        //Search for the schema file as .schema.json and .json and load it when found
                        string pathAndFileString = uri.LocalPath;
                        string schemaString = string.Empty;
                        if (File.Exists(pathAndFileString))
                        {
                            schemaString = File.ReadAllText(pathAndFileString);
                        }
                        else if (File.Exists(pathAndFileString + ".schema.json"))
                        {
                            schemaString = File.ReadAllText(pathAndFileString + ".schema.json");
                        }
                        else if (File.Exists(pathAndFileString + ".json"))
                        {
                            schemaString = File.ReadAllText(pathAndFileString + ".json");
                        }
                        else
                        {
                            throw new CodeGenerationException($"Schema could not be found at the path {pathAndFileString}");
                        }
                        // make a schema and generate code from that
                        JsonValue jsonValueOfSchema = JsonValue.Parse(schemaString);
                        if(string.IsNullOrWhiteSpace(jsonSchemaBuilderUriReference.ObjectReference) ||
                            jsonSchemaBuilderUriReference.ObjectReference.Equals("/"))
                        {
                            //Process schema Json
                            GenerateCodeFromSchema(codeBuilder, jsonValueOfSchema, key, jsonSchemaBuilderUriReference);
                        }
                        else if(jsonSchemaBuilderUriReference.ObjectReference.ToLowerInvariant().StartsWith("/definitions/"))
                        {
                            JsonSerializer jsonSerializer = new JsonSerializer();
                            JsonSchema jsonSchema = jsonSerializer.Deserialize<JsonSchema>(jsonValueOfSchema);
                            string afterDefinitions = jsonSchemaBuilderUriReference.ObjectReference.ToLowerInvariant().Replace("/definitions/", "");
                            if(jsonSchema.Definitions() != null && jsonSchema.Definitions().TryGetValue(afterDefinitions, out JsonSchema subSchema))
                            {
                                //Process subschema Json
                                GenerateCodeFromSchema(codeBuilder, subSchema.ToJson(jsonSerializer), key, jsonSchemaBuilderUriReference);
                            }
                            else
                            {
                                throw new CodeGenerationException($"Could not find {afterDefinitions} in the definitions of the schema");
                            }
                        }
                        else
                        {
                            throw new NotImplementedException($"Uri reference is not supported other than /definitions/ or entire schema");
                        }
                    }
                    else
                    {
                        throw new NotImplementedException($"Uri {uri} is not supported");
                    }
                }
                else
                {
                    throw new CodeGenerationException($"{nameof(jsonSchemaBuilderUriReference.LocalFileLocation)} provides a invalid Uri");
                }
            }
            catch (CodeGenerationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CodeGenerationException($"GenerateOrdinaryUriReference failed for some reason", ex);
            }
        }

        private void GenerateCodeFromSchema(CodeBuilder codeBuilder, JsonValue jsonValueOfSchema, IdentifierString key, JsonSchemaBuilderUriReference jsonSchemaBuilderUriReference)
        {
            if (jsonValueOfSchema.Type == JsonValueType.Object)
            {
                JsonObject schemaObject = jsonValueOfSchema.Object;

                GenerateComments(codeBuilder, key, jsonSchemaBuilderUriReference);

                codeBuilder
                    .L($"[JsonProperty(\"{TransformToCamelCase(key)}\")]")
                    .L($"public {MakeCorrectItemType(jsonValueOfSchema)} {TransformToTitleCase(key)} {{ get; set; }}{GenerateDefaultIfExisting(key, jsonSchemaBuilderUriReference)}")
                    .EmptyLine();
            }
            else
            {
                throw new CodeGenerationException($"GenerateOrdinaryUriReference references an invalid schema");
            }
        }

        private string MakeCorrectItemType(JsonValue jsonValueOfSchema)
        {
            JsonObject jsonObjectOfSchema = jsonValueOfSchema.Object;
            string name = string.Empty;
            if (jsonObjectOfSchema.TryGetValue("$id", out JsonValue jsonValueId))
            {
                name = jsonValueId.String;
            }
            else if (jsonObjectOfSchema.TryGetValue("title", out JsonValue jsonValueTitle))
            {
                name = jsonValueTitle.String;
            }
            else
            {
                throw new CodeGenerationException($"GenerateOrdinaryUriReference has neither title nor id");
            }
            string type = string.Empty;
            if (jsonObjectOfSchema.TryGetValue("type", out JsonValue jsonValueType))
            {
                type = jsonValueType.String;
            }
            else
            {
                throw new CodeGenerationException($"GenerateOrdinaryUriReference has no type");
            }
            string format = string.Empty;
            if (jsonObjectOfSchema.TryGetValue("format", out JsonValue jsonValueFormat))
            {
                format = jsonValueFormat.String;
            }
            switch(type)
            {
                case "string":
                    if(string.IsNullOrWhiteSpace(format))
                    {
                        return "string";
                    }
                    switch(format)
                    {
                        case "time":
                        case "date":
                        case "datetime":
                            return "DateTime";
                        case "email":
                            return "Email";
                    }
                    break;
                case "boolean":
                    return "bool";
                case "object":
                    return TransformToTitleCase(name);
                case "number":
                    return "double";
                case "integer":
                    return "long";
            }
            throw new NotImplementedException($"Type {type} with name {name} and format {format} is not supported");
        }
    

        #endregion

        #region Generate Comments

        /// <summary>
        /// Generates comments for the parts that need it
        /// </summary>
        /// <param name="codeBuilder"></param>
        /// <param name="key"></param>
        /// <param name="jsonSchemaBuilderPart"></param>
        private void GenerateComments(CodeBuilder codeBuilder, IdentifierString key, IJsonSchemaBuilderPart jsonSchemaBuilderPart)
        {
            List<string> commentLines = GenerateCommentLines(key, jsonSchemaBuilderPart.Description, minSplitLength: 70, maxSplitLength: 90);

            codeBuilder.L("/// <summary>");
            foreach (string commentLine in commentLines)
            {
                codeBuilder.L($"/// {commentLine}");
            }
            codeBuilder.L("/// </summary>");
        }

        private List<string> GenerateCommentLines(IdentifierString key, string description, int minSplitLength, int maxSplitLength)
        {
            string stringToSplit = $"{key}: {description}";

            return Regex.Split(stringToSplit, @"(.{1," + maxSplitLength.ToString() + @"})(?:\s|$)|(.{" + maxSplitLength.ToString() + @"})")
              .Where(x => x.Length > 0)
              .ToList();
        }

        #endregion

        #region Generate Defaults

        private string GenerateDefaultIfExisting(IdentifierString key, JsonSchemaBuilderString jsonSchemaBuilderString)
        {
            return string.IsNullOrWhiteSpace(jsonSchemaBuilderString.DefaultValue) ? string.Empty : $" = \"{jsonSchemaBuilderString.DefaultValue}\";";
        }

        private string GenerateDefaultIfExisting(IdentifierString key, JsonSchemaBuilderEmail jsonSchemaBuilderEmail)
        {
            return string.IsNullOrWhiteSpace(jsonSchemaBuilderEmail.DefaultValue) ? string.Empty : $" = new MailAddress(\"{jsonSchemaBuilderEmail.DefaultValue}\");";
        }

        private string GenerateDefaultIfExisting(IdentifierString key, JsonSchemaBuilderDateTime jsonSchemaBuilderDateTime)
        {
            return string.IsNullOrWhiteSpace(jsonSchemaBuilderDateTime.DefaultValue) ? string.Empty : $" = DateTime.Parse(\"{jsonSchemaBuilderDateTime.DefaultValue}\");";
        }
        private string GenerateDefaultIfExisting(IdentifierString key, JsonSchemaBuilderDate jsonSchemaBuilderDate)
        {
            return string.IsNullOrWhiteSpace(jsonSchemaBuilderDate.DefaultValue) ? string.Empty : $" = DateTime.Parse(\"{jsonSchemaBuilderDate.DefaultValue}\");";
        }
        private string GenerateDefaultIfExisting(IdentifierString key, JsonSchemaBuilderTime jsonSchemaBuilderTime)
        {
            return string.IsNullOrWhiteSpace(jsonSchemaBuilderTime.DefaultValue) ? string.Empty : $" = DateTime.Parse(\"{jsonSchemaBuilderTime.DefaultValue}\");";
        }

        private string GenerateDefaultIfExisting(IdentifierString key, JsonSchemaBuilderBoolean jsonSchemaBuilderBoolean)
        {
            if (jsonSchemaBuilderBoolean.DefaultValue.HasValue)
            {
                string defaultValue = jsonSchemaBuilderBoolean.DefaultValue.Value ? "true" : "false";
                return $" = {defaultValue};";
            }
            else
            {
                return string.Empty;
            }
        }

        private string GenerateDefaultIfExisting(IdentifierString key, JsonSchemaBuilderInteger jsonSchemaBuilderInteger)
        {
            if (jsonSchemaBuilderInteger.DefaultValue.HasValue)
            {
                return $" = {jsonSchemaBuilderInteger.DefaultValue.Value};";
            }
            else
            {
                return string.Empty;
            }
        }

        private string GenerateDefaultIfExisting(IdentifierString key, JsonSchemaBuilderNumber jsonSchemaBuilderNumber)
        {
            if (jsonSchemaBuilderNumber.DefaultValue.HasValue)
            {
                return $" = {jsonSchemaBuilderNumber.DefaultValue.Value};";
            }
            else
            {
                return string.Empty;
            }
        }

        private string GenerateDefaultIfExisting(IdentifierString key, JsonSchemaBuilderArray jsonSchemaBuilderArray)
        {
            if (jsonSchemaBuilderArray.DefaultValue != null)
            {
                return $" = new List<{MakeCorrectItemType(jsonSchemaBuilderArray.Items[0])}>();{{{jsonSchemaBuilderArray.DefaultValue.ToString()}}}";
            }
            else
            {
                return $" = new List<{MakeCorrectItemType(jsonSchemaBuilderArray.Items[0])}>();";
            }
        }

        private string GenerateDefaultIfExisting(IdentifierString key, JsonSchemaBuilderUriReference jsonSchemaBuilderUriReference)
        {
            if(jsonSchemaBuilderUriReference.DefaultValue != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                return string.Empty;
            }
        }



        #endregion

        #region HelperFunctions

        /// <summary>
        /// Transform string from TitleCase to camelCase
        /// </summary>
        /// <param name="stringToTransform"></param>
        /// <returns></returns>
        private string TransformToCamelCase(string stringToTransform)
        {
            return stringToTransform.Substring(0, 1).ToLowerInvariant() + stringToTransform.Substring(1);
        }

        /// <summary>
        /// Transform string from camelCase to TitleCase
        /// </summary>
        /// <param name="stringToTransform"></param>
        /// <returns></returns>
        private string TransformToTitleCase(string stringToTransform)
        {
            return stringToTransform.Substring(0, 1).ToUpperInvariant() + stringToTransform.Substring(1);
        }

        /// <summary>
        /// Returns ? if required to build Nullable valuetypes if not required
        /// </summary>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        private string BuildRequired(bool isRequired)
        {
            return isRequired ? string.Empty: "?";
        }

        #endregion
    }
}
