using DevelApp.Utility.Model;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    /// <summary>
    /// Represents Uri (absolute Uri),  Uri reference (absolute or relative Uri), 
    /// Iri (absolute internationalized Uri),  Iri reference (absolute or relative internationalized Uri)
    /// Add reference "$ref": "./xs.schema.json#/definitions/xs:decimal" with "./xs.schema.json" as the local file and 
    /// "#/definitions/xs:decimal" as fragment getting xs:decimal from the definition
    /// </summary>
    public class JSBRef : AbstractJSBPart<string>
    {
        public JSBRef(IdentifierString referenceName, string description, Uri iriReference, 
            string defaultValue = null, List<string> examples = null, List<string> enums = null, bool isRequired = false) 
            : base(referenceName, description, isRequired, defaultValue, examples, enums)
        {
            IriReference = iriReference;
            RelativeLocalFile = TransformToLocalFilePlacement(iriReference);
            string fragment = string.Empty;
            if (iriReference.IsAbsoluteUri)
            {
                fragment = iriReference.Fragment;
            }
            else
            {
                if (iriReference.OriginalString.IndexOf('#') > -1)
                {
                    fragment = iriReference.OriginalString.Substring(iriReference.OriginalString.IndexOf('#'));
                }
            }
            if(fragment.StartsWith("#"))
            {
                fragment = fragment.Substring(1);
            }
            Fragment = fragment;
        }

        /// <summary>
        /// Local File Placement of base from Application Root starting with path separator
        /// Consists of IdnHost + PathAndQuery of the Uri transformed so that . and / is a local path separator
        /// IdnHost: www.contoso.com transforms to \www\contoso\com on windows
        /// PathAndQuery: /Home/Index.htm?q1=v1&q2=v2 transforms to \Home\Index.htmq1=v1&q2=v2.schema.json
        /// Local file placement for nonlocal absolute Iri giving \www\contoso\com\Home\Index.htmq1=v1&q2=v2.schema.json
        /// When Uri is a loopback (local resource) the IdnHost is ignored
        /// </summary>
        /// <param name="iriReference"></param>
        /// <returns></returns>
        public string TransformToLocalFilePlacement(Uri iriReference)
        {
            string host = string.Empty;
            //Is external file
            if (iriReference.IsAbsoluteUri && !iriReference.IsLoopback)
            {
                host = "/" + iriReference.IdnHost.Replace(".", "/");
            }
            string localPath = string.Empty;
            if (iriReference.IsAbsoluteUri)
            {
                localPath = iriReference.PathAndQuery.Replace("?", "");
            }
            else
            {
                localPath = iriReference.OriginalString;
                if(localPath.Contains("#"))
                {
                    string fragment = iriReference.OriginalString.Substring(iriReference.OriginalString.IndexOf('#'));
                    localPath = localPath.Replace(fragment, "");
                }
            }
            if (!string.IsNullOrWhiteSpace(localPath) && !localPath.EndsWith(".schema.json"))
            {
                if (localPath.EndsWith(".json"))
                {
                    localPath = localPath.Replace(".json", ".schema.json");
                }
                else
                {
                    localPath = localPath + ".schema.json";
                }
            }

            return (host + localPath).Replace('/', Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Fragment without # with #Item denoting entire all content of the local Item object or Item
        /// </summary>
        public string Fragment { get; }

        /// <summary>
        /// Relative Local File of base from Application Root starting with path separator
        /// Consists of IdnHost + PathAndQuery of the Uri transformed so that . and / is a local path separator
        /// IdnHost: www.contoso.com transforms to \www\contoso\com on windows
        /// PathAndQuery: /Home/Index.htm?q1=v1&q2=v2 transforms to \Home\Index.htmq1=v1&q2=v2.schema.json
        /// Total placement for nonlocal absolute Iri giving \www\contoso\com\Home\Index.htmq1=v1&q2=v2.schema.json
        /// </summary>
        public string RelativeLocalFile { get; }

        /// <summary>
        /// Represents an iri reference
        /// </summary>
        public Uri IriReference { get; }

        public override JSBPartType PartType
        {
            get
            {
                return JSBPartType.IriReference;
            }
        }

        public bool IsFragmentOnly 
        { 
            get
            {
                return string.IsNullOrWhiteSpace(RelativeLocalFile)  && !string.IsNullOrWhiteSpace(Fragment);
            }
        }

        public override JsonSchema AsJsonSchema()
        {
            JsonSchema returnSchema = InitialJsonSchema()
                .Type(JsonSchemaType.String);
            if(IriReference.IsAbsoluteUri)
            {
                returnSchema.Format("iri");
            }
            else
            {
                returnSchema.Format("iri-reference");
            }
            returnSchema.Ref(IriReference.OriginalString);
            return returnSchema;
        }

    }
}
