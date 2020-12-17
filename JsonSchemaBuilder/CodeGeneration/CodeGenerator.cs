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
        private string _applicationRoot;
        public CodeGenerator(string applicationRoot)
        {
            _applicationRoot = applicationRoot;
        }
        /// <summary>
        /// Generate code to memory and suggest a filename
        /// </summary>
        /// <param name="code"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public (string fileName, string code) Generate(Code code, IJsonSchemaDefinition schema)
        {
            DoReferenceResolution(schema);

            switch (code)
            {
                case Code.CSharp:
                    return CSharp.GenerateCode(_applicationRoot, schema);
                default:
                    throw new CodeGenerationException($"Code generation of {code} is not supported");
            }
        }

        /// <summary>
        /// Makes schema resolution and moves to local file structure
        /// </summary>
        /// <param name="schema"></param>
        private void DoReferenceResolution(IJsonSchemaDefinition schema)
        {
            DoReferenceResolution(schema.JsonSchemaBuilderSchema, schema.Module);
        }

        private void DoReferenceResolution(IJsonSchemaBuilderPart jsonSchemaBuilderPart, NamespaceString module)
        {
            switch(jsonSchemaBuilderPart.PartType)
            {
                case JsonSchemaBuilderPartType.IriReference:
                    JsonSchemaBuilderIriReference jsonSchemaBuilderIriReference = jsonSchemaBuilderPart as JsonSchemaBuilderIriReference;
                    if (!jsonSchemaBuilderIriReference.IriReference.AbsoluteUri.Equals(jsonSchemaBuilderIriReference.IriReference.Fragment))
                    {
                        string relativeLocalFileWithExpandedDot = jsonSchemaBuilderIriReference.RelativeLocalFile;
                        if (relativeLocalFileWithExpandedDot.StartsWith("./") || relativeLocalFileWithExpandedDot.StartsWith(".\\"))
                        {
                            relativeLocalFileWithExpandedDot = Path.Combine(module.ToFilePath, relativeLocalFileWithExpandedDot.Remove(0, 2));
                        }

                        string localFile = Path.Combine(_applicationRoot, relativeLocalFileWithExpandedDot);
                        string uriWithoutFragmentString = jsonSchemaBuilderIriReference.IriReference.AbsoluteUri;
                        if(!string.IsNullOrWhiteSpace(jsonSchemaBuilderIriReference.IriReference.Fragment))
                        {
                            uriWithoutFragmentString = uriWithoutFragmentString.Replace(jsonSchemaBuilderIriReference.IriReference.Fragment, "");
                        }
                        if (Uri.TryCreate(uriWithoutFragmentString, UriKind.RelativeOrAbsolute, out Uri uriWithoutFragment))
                        {
                            if (!File.Exists(localFile))
                            {
                                if (!jsonSchemaBuilderIriReference.IriReference.IsLoopback)
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
                case JsonSchemaBuilderPartType.Schema:
                    JsonSchemaBuilderSchema jsonSchemaBuilderSchema = jsonSchemaBuilderPart as JsonSchemaBuilderSchema;
                    foreach (IJsonSchemaBuilderPart definition in jsonSchemaBuilderSchema.Definitions.Values)
                    {
                        DoReferenceResolution(definition, module);
                    }
                    if (jsonSchemaBuilderSchema.TopPart != null)
                    {
                        DoReferenceResolution(jsonSchemaBuilderSchema.TopPart, module);
                    }
                    break;
                case JsonSchemaBuilderPartType.Array:
                    JsonSchemaBuilderArray jsonSchemaBuilderArray = jsonSchemaBuilderPart as JsonSchemaBuilderArray;
                    foreach(IJsonSchemaBuilderPart part in jsonSchemaBuilderArray.Items)
                    {
                        DoReferenceResolution(part, module);
                    }
                    break;
                case JsonSchemaBuilderPartType.Object:
                    JsonSchemaBuilderObject jsonSchemaBuilderObject = jsonSchemaBuilderPart as JsonSchemaBuilderObject;
                    foreach(IJsonSchemaBuilderPart property in jsonSchemaBuilderObject.Properties.Values)
                    {
                        DoReferenceResolution(property, module);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Generate code to a path
        /// </summary>
        /// <param name="code"></param>
        /// <param name="schema"></param>
        public void GenerateToFile(Code code, IJsonSchemaDefinition schema)
        {
            var tuple = Generate(code, schema);

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
            catch(Exception ex)
            {
                throw new CodeGenerationException($"Could not write code file to {tuple.fileName}", ex);
            }
        }
    }
}
