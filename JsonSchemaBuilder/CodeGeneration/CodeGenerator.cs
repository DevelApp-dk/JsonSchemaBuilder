using DevelApp.JsonSchemaBuilder.Exceptions;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using Manatee.Json;
using Manatee.Json.Schema;
using Manatee.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.CodeGeneration
{
    public class CodeGenerator
    {
        public Dictionary<string, IJsonSchemaDefinition> RegisteredJsonSchemas { get; } = new Dictionary<string, IJsonSchemaDefinition>();

        public CodeGenerator(string applicationRoot, string jsonSchemaApplicationRoot)
        {
            ApplicationRoot = applicationRoot;
            JsonSchemaApplicationRoot = jsonSchemaApplicationRoot;
        }

        public string ApplicationRoot { get; }
        public string JsonSchemaApplicationRoot { get; }

        /// <summary>
        /// Generate a list of code to memory and suggest a filename from registered IJsonSchemaDefinitions
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<(string fileName, string code)> Generate(Code code)
        {
            DoReferenceResolution();

            switch (code)
            {
                case Code.CSharp:
                    return CSharp.GenerateCode(this);
                default:
                    throw new CodeGenerationException($"Code generation of {code} is not supported");
            }
        }

        /// <summary>
        /// Makes schema resolution and moves to local file structure
        /// </summary>
        /// <param name="schema"></param>
        private void DoReferenceResolution()
        {
            foreach (IJsonSchemaDefinition schema in RegisteredJsonSchemas.Values)
            {
                DoReferenceResolution(schema.JsonSchemaBuilderSchema, schema.Module);
            }
        }

        /// <summary>
        /// Register all the IJsonSchemaDefinition generated in one go to avoid ordering problems for references
        /// </summary>
        /// <param name="jsonSchema"></param>
        public void Register(IJsonSchemaDefinition jsonSchema)
        {
            RegisteredJsonSchemas.Add(jsonSchema.FileName, jsonSchema);
        }

        private void DoReferenceResolution(IJSBPart jsonSchemaBuilderPart, NamespaceString module)
        {
            switch (jsonSchemaBuilderPart.PartType)
            {
                case JSBPartType.IriReference:
                    JSBRef jsonSchemaBuilderIriReference = jsonSchemaBuilderPart as JSBRef;
                    if (!jsonSchemaBuilderIriReference.IsFragmentOnly)
                    {
                        string relativeLocalFileWithExpandedDot = jsonSchemaBuilderIriReference.RelativeLocalFile;
                        if (relativeLocalFileWithExpandedDot.StartsWith("./") || relativeLocalFileWithExpandedDot.StartsWith(".\\"))
                        {
                            relativeLocalFileWithExpandedDot = Path.Combine(module.ToFilePath, relativeLocalFileWithExpandedDot.Remove(0, 2));
                        }

                        string localFile = Path.Combine(JsonSchemaApplicationRoot, TransformToCamelCase(relativeLocalFileWithExpandedDot));
                        string uriWithoutFragmentString = jsonSchemaBuilderIriReference.IriReference.OriginalString;
                        if (!string.IsNullOrWhiteSpace(jsonSchemaBuilderIriReference.Fragment))
                        {
                            uriWithoutFragmentString = uriWithoutFragmentString.Replace("#" + jsonSchemaBuilderIriReference.Fragment, "");
                        }
                        if (Uri.TryCreate(uriWithoutFragmentString, UriKind.RelativeOrAbsolute, out Uri uriWithoutFragment))
                        {
                            if (!File.Exists(localFile))
                            {
                                if (jsonSchemaBuilderIriReference.IriReference.IsAbsoluteUri && !jsonSchemaBuilderIriReference.IriReference.IsLoopback)
                                {
                                    try
                                    {
                                        FileInfo fileInfo = new FileInfo(localFile);
                                        string directory = fileInfo.Directory.FullName;
                                        if (!Directory.Exists(directory))
                                        {
                                            Directory.CreateDirectory(directory);
                                        }
                                        using (WebClient myWebClient = new WebClient())
                                        {
                                            // Download the Web resource and save it into the current filesystem folder.
                                            myWebClient.DownloadFile(uriWithoutFragment, localFile);
                                            //TODO generate builderparts from schema and register in _registeredJsonSchemas
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new CodeGenerationException($"Resource at {uriWithoutFragment} could not be downloaded", ex);
                                    }
                                }
                                else
                                {
                                    throw new CodeGenerationException($"Iri reference {uriWithoutFragment} was not found locally at {localFile}");
                                }
                            }
                        }
                    }
                    break;
                case JSBPartType.Schema:
                    JSBSchema jsonSchemaBuilderSchema = jsonSchemaBuilderPart as JSBSchema;
                    foreach (IJSBPart definition in jsonSchemaBuilderSchema.Definitions.Values)
                    {
                        DoReferenceResolution(definition, module);
                    }
                    if (jsonSchemaBuilderSchema.TopPart != null)
                    {
                        DoReferenceResolution(jsonSchemaBuilderSchema.TopPart, module);
                    }
                    break;
                case JSBPartType.Array:
                    JSBArray jsonSchemaBuilderArray = jsonSchemaBuilderPart as JSBArray;
                    foreach (IJSBPart part in jsonSchemaBuilderArray.Items)
                    {
                        DoReferenceResolution(part, module);
                    }
                    break;
                case JSBPartType.Object:
                    JSBObject jsonSchemaBuilderObject = jsonSchemaBuilderPart as JSBObject;
                    foreach (IJSBPart property in jsonSchemaBuilderObject.Properties.Values)
                    {
                        DoReferenceResolution(property, module);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Generate code to a path from registered IJsonSchemaDefinitions
        /// </summary>
        /// <param name="code"></param>
        public void GenerateToFile(Code code)
        {
            List<(string fileName, string code)> tupleList = Generate(code);

            foreach (var tuple in tupleList)
            {
                FileInfo fileInfo = new FileInfo(tuple.fileName);
                string directory = fileInfo.Directory.FullName;
                try
                {
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                }
                catch (Exception ex)
                {
                    throw new CodeGenerationException($"Could not create path {directory}", ex);
                }

                try
                {
                    using (StreamWriter outputFile = new StreamWriter(tuple.fileName))
                    {
                        outputFile.Write(tuple.code);
                        outputFile.Flush();
                    }
                }
                catch (Exception ex)
                {
                    throw new CodeGenerationException($"Could not write code file to {tuple.fileName}", ex);
                }
            }
        }

        /// <summary>
        /// Returns null if not found
        /// </summary>
        /// <param name="relativeFileNameWithExpandedDot"></param>
        /// <param name="jSBRef"></param>
        /// <returns></returns>
        public (IJSBPart refPart, JsonValue schemaValue) LookupReferencedPart(string relativeFileNameWithExpandedDot, JSBRef jSBRef)
        {
            if (RegisteredJsonSchemas.TryGetValue(relativeFileNameWithExpandedDot, out IJsonSchemaDefinition jsonSchemaDefinition))
            {
                if (jSBRef.Fragment.ToLowerInvariant().StartsWith("/definitions/"))
                {
                    string definitionKey = TransformToTitleCase(jSBRef.Fragment.Substring("/definitions/".Length));
                    if (jsonSchemaDefinition.JsonSchemaBuilderSchema.Definitions.TryGetValue(definitionKey, out IJSBPart referencedPart))
                    {
                        return (referencedPart, null);
                    }
                }
                else
                {
                    return (jsonSchemaDefinition.JsonSchemaBuilderSchema, null);
                }
            }

            //Search for the schema file as .schema.json and .json and load it when found
            string schemaString = string.Empty;

            string localfile = Path.Combine(JsonSchemaApplicationRoot, relativeFileNameWithExpandedDot);
            if (File.Exists(localfile))
            {
                schemaString = File.ReadAllText(localfile);
            }
            else
            {
                throw new CodeGenerationException($"Schema could not be found at the path {localfile}");
            }

            // make a schema and generate code from that
            JsonValue jsonValueOfSchema = JsonValue.Parse(schemaString);
            if (string.IsNullOrWhiteSpace(jSBRef.Fragment) ||
                jSBRef.Fragment.Equals("/"))
            {
                return (null, jsonValueOfSchema);
            }
            else if (jSBRef.Fragment.ToLowerInvariant().StartsWith("/definitions/"))
            {
                //TODO Avoid serialization step
                JsonSerializer jsonSerializer = new JsonSerializer();
                JsonSchema jsonSchema = jsonSerializer.Deserialize<JsonSchema>(jsonValueOfSchema);
                string afterDefinitions = jSBRef.Fragment.ToLowerInvariant().Replace("/definitions/", "");
                if (jsonSchema.Definitions() != null && jsonSchema.Definitions().TryGetValue(afterDefinitions, out JsonSchema subSchema))
                {
                    //Process subschema Json
                    return (null, subSchema.ToJson(jsonSerializer));
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

        /// <summary>
        /// Transform string from TitleCase to camelCase. Ignores everything before \ or / if existing
        /// </summary>
        /// <param name="stringToTransform"></param>
        /// <returns></returns>
        public string TransformToCamelCase(string stringToTransform)
        {
            if(string.IsNullOrWhiteSpace(stringToTransform))
            {
                return stringToTransform;
            }
            int index = stringToTransform.LastIndexOfAny(new char[] { '\\', '/' });

            if (index >= 0 && stringToTransform.Length > index+2)
            {
                return stringToTransform.Substring(0, index + 1) + stringToTransform.Substring(index + 1, 1).ToLowerInvariant() + stringToTransform.Substring(index + 2);
            }
            else
            {
                return stringToTransform.Substring(0, 1).ToLowerInvariant() + stringToTransform.Substring(1);
            }
        }

        /// <summary>
        /// Transform string from camelCase to TitleCase. Ignores everything before \ or / if existing
        /// </summary>
        /// <param name="stringToTransform"></param>
        /// <returns></returns>
        public string TransformToTitleCase(string stringToTransform)
        {
            if (string.IsNullOrWhiteSpace(stringToTransform))
            {
                return stringToTransform;
            }
            int index = stringToTransform.LastIndexOfAny(new char[] { '\\', '/' });

            if (index >= 0 && stringToTransform.Length > index + 2)
            {
                return stringToTransform.Substring(0, index + 1) + stringToTransform.Substring(index + 1, 1).ToUpperInvariant() + stringToTransform.Substring(index + 2);
            }
            else
            {
                return stringToTransform.Substring(0, 1).ToUpperInvariant() + stringToTransform.Substring(1);
            }
        }


    }
}
