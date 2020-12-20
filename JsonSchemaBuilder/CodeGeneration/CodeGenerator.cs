using DevelApp.JsonSchemaBuilder.Exceptions;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
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

        public CodeGenerator(string applicationRoot)
        {
            ApplicationRoot = applicationRoot;
        }

        public string ApplicationRoot { get; }

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
            switch(jsonSchemaBuilderPart.PartType)
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

                        string localFile = Path.Combine(ApplicationRoot, relativeLocalFileWithExpandedDot);
                        string uriWithoutFragmentString = jsonSchemaBuilderIriReference.IriReference.OriginalString;
                        if(!string.IsNullOrWhiteSpace(jsonSchemaBuilderIriReference.Fragment))
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
                    foreach(IJSBPart part in jsonSchemaBuilderArray.Items)
                    {
                        DoReferenceResolution(part, module);
                    }
                    break;
                case JSBPartType.Object:
                    JSBObject jsonSchemaBuilderObject = jsonSchemaBuilderPart as JSBObject;
                    foreach(IJSBPart property in jsonSchemaBuilderObject.Properties.Values)
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
    }
}
