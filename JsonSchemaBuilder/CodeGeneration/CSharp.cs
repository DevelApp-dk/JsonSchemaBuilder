using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using System;
using System.Collections.Generic;
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
                properties.Add(schema.Name, schema.TopPart);

                JsonSchemaBuilderObject encasingObject = new JsonSchemaBuilderObject(schema.TopPart.Name, schema.TopPart.Description, properties: properties);
                GenerateCodeForBuilderPart(codeBuilder, schema.TopPart.Name, encasingObject, schema.Definitions);
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
                .L("using Ardalis.SmartEnum;")
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

        private void GenerateCodeForBuilderPart(CodeBuilder codeBuilder, IdentifierString key, IJsonSchemaBuilderPart value, Dictionary<IdentifierString, IJsonSchemaBuilderPart> definitions = null)
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
                case JsonSchemaBuilderPartType.Base64Binary:
                    JsonSchemaBuilderBase64Binary jsonSchemaBuilderBase64Binary = value as JsonSchemaBuilderBase64Binary;
                    if (jsonSchemaBuilderBase64Binary.Enums != null && jsonSchemaBuilderBase64Binary.Enums.Count > 0)
                    {
                        GenerateEnumBase64Binary(codeBuilder, key, jsonSchemaBuilderBase64Binary);
                    }
                    else
                    {
                        GenerateOrdinaryBase64Binary(codeBuilder, key, jsonSchemaBuilderBase64Binary);
                    }
                    break;
                case JsonSchemaBuilderPartType.Boolean:
                    JsonSchemaBuilderBoolean jsonSchemaBuilderBoolean = value as JsonSchemaBuilderBoolean;
                    if(jsonSchemaBuilderBoolean.Enums != null && jsonSchemaBuilderBoolean.Enums.Count > 0)
                    {
                        GenerateEnumBoolean(codeBuilder, key, jsonSchemaBuilderBoolean);
                    }
                    else
                    {
                        GenerateOrdinaryBoolaen(codeBuilder, key, jsonSchemaBuilderBoolean);
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
                        GenerateOrdinaryDateTime(codeBuilder, key, jsonSchemaBuilderDateTime);
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
                        GenerateOrdinaryInteger(codeBuilder, key, jsonSchemaBuilderInteger);
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
                        GenerateOrdinaryNumber(codeBuilder, key, jsonSchemaBuilderNumber);
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
                        GenerateOrdinaryObject(codeBuilder, key, jsonSchemaBuilderObject, definitions);
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
            throw new NotImplementedException();
        }

        #endregion

        #region Generate Base64 Binary

        private void GenerateOrdinaryBase64Binary(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderBase64Binary jsonSchemaBuilderBase64Binary)
        {
            throw new NotImplementedException();
        }

        private void GenerateEnumBase64Binary(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderBase64Binary jsonSchemaBuilderBase64Binary)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        private void GenerateEnumDate(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderDate jsonSchemaBuilderDate)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Generate DateTime

        private void GenerateOrdinaryDateTime(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderDateTime jsonSchemaBuilderDateTime)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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

        private void GenerateOrdinaryObject(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderObject jsonSchemaBuilderObject, Dictionary<IdentifierString, IJsonSchemaBuilderPart> definitions)
        {
            codeBuilder
                //TODO Add comment from description split on lines
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


            if (definitions == null)// Assume not top part
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
            GenerateComments(codeBuilder, key, jsonSchemaBuilderString);

            codeBuilder
                .L($"public enum {TransformToTitleCase(key)}")
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
                .EmptyLine()
                .L($"[JsonProperty(\"{TransformToCamelCase(key)}\"), JsonConverter(typeof(StringEnumConverter))]") 
                .L($"public {TransformToTitleCase(key)} {TransformToTitleCase(key)} {{ get; set; }}{GenerateDefaultIfExisting(key, jsonSchemaBuilderString)}")
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
            throw new NotImplementedException();
        }

        #endregion

        #region Generate Uri Reference

        private void GenerateEnumUriReference(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderUriReference jsonSchemaBuilderUriReference)
        {
            throw new NotImplementedException();
        }

        private void GenerateOrdinaryUriReference(CodeBuilder codeBuilder, IdentifierString key, JsonSchemaBuilderUriReference jsonSchemaBuilderUriReference)
        {
            throw new NotImplementedException();
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

            foreach(string commentLine in commentLines)
            {
                codeBuilder.L($"/// {commentLine}");
            }
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
