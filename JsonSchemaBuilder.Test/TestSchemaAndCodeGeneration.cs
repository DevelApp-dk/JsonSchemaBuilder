using DevelApp.JsonSchemaBuilder;
using System;
using System.Collections.Generic;
using Xunit;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using System.Linq;
using DevelApp.JsonSchemaBuilder.CodeGeneration;
using System.IO;

namespace JsonSchemaBuilder.Test
{
    public class TestSchemaAndCodeGeneration
    {
        [Fact]
        public void BuildFromAssembly()
        {
            string pathString = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar;
            string assemblyPath = GetType().Assembly.Location;
            string pathStringExpanded = Path.GetFullPath(pathString, assemblyPath);

            LoadAllJsonSchemaBuildersAndWriteSchemasToFile(pathStringExpanded);

            LoadAllJsonSchemaBuildersAndGenerateCSharpCodeToFile(pathStringExpanded);
        }


        #region Assembly load all schemas and write

        private static void LoadAllJsonSchemaBuildersAndGenerateCSharpCodeToFile(string pathString)
        {
            CodeGenerator codeGenerator = new CodeGenerator(pathString);
            foreach (Type codeDefinedType in GetInterfaceTypes(typeof(IJsonSchemaDefinition)))
            {
                IJsonSchemaDefinition jsonSchema = GetJsonSchemaInstance(codeDefinedType);
                codeGenerator.Register(jsonSchema);
            }
            codeGenerator.Generate(Code.CSharp);
        }

        private static void LoadAllJsonSchemaBuildersAndWriteSchemasToFile(string pathString)
        {
            foreach (Type codeDefinedType in GetInterfaceTypes(typeof(IJsonSchemaDefinition)))
            {
                IJsonSchemaDefinition jsonSchema = GetJsonSchemaInstance(codeDefinedType);
                jsonSchema.WriteSchemaToFile(pathString);
            }
        }

        private static IEnumerable<Type> GetInterfaceTypes(Type interfaceType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);
        }

        private static IJsonSchemaDefinition GetJsonSchemaInstance(Type jsonSchemaType)
        {
            //Create dataowner instance
            IJsonSchemaDefinition jsonSchemaInstance = (IJsonSchemaDefinition)Activator.CreateInstance(jsonSchemaType);

            return jsonSchemaInstance;
        }

        #endregion
    }

    //public class DataOwnerDefinitionJsonSchema : AbstractJsonSchema
    //{
    //    protected override JSBSchema BuildJsonSchema()
    //    {
    //        List<IJSBPart> wfProps = new List<IJSBPart>();
    //        wfProps.Add(new JSBString("Name", "Returns the DataOwner unique name", maxLength: 100, isRequired: true));
    //        wfProps.Add(new JSBString("Version", "Returns the version of the DataOwner", isRequired: true));
    //        wfProps.Add(new JSBArray("ModuleDefinitions", "Returns the embedded module definitions", items: new List<IJSBPart>() {
    //         new JSBRef("ModuleDefinition", "Definition of an module", iriReference: new Uri("./ModuleDefinition.schema.json", UriKind.Relative))}, isRequired: true));
    //        JSBObject objectPart = new JSBObject("DataOwnerDefinition", "The easy definition of the schema of a dataOwner", props: wfProps);

    //        return new JSBSchema("DataOwnerDefinition", Description, topPart: objectPart);
    //    }

    //    public override string Description
    //    {
    //        get
    //        {
    //            return "The easy definition of the schema of a dataOwner";
    //        }
    //    }

    //    public override NamespaceString Module
    //    {
    //        get
    //        {
    //            return "Default";
    //        }
    //    }
    //}

    public class FtpInformationJsonSchema : AbstractJsonSchema
    {
        protected override JSBSchema BuildJsonSchema()
        {
            List<IJSBPart> definitions = new List<IJSBPart>();
            definitions.Add(new JSBString("FtpType", "Type of FTP", enums: new List<string>() { "Auto", "FTP", "FTPS" }));
            List<IJSBPart> properties = new List<IJSBPart>();
            properties.Add(new JSBString("SourceFileName", "File to move or copy", isRequired: true));
            properties.Add(new JSBString("SourceDirectory", "Location of the file to move or copy", isRequired: true));
            properties.Add(new JSBString("DestinationFileName", "File after move or copy. If filled out a rename will be done under the move or copy operation"));
            properties.Add(new JSBString("DestinationDirectory", "Location of the file should end after move or copy", isRequired: true));
            properties.Add(new JSBString("FtpUser", "User registered on the server", isRequired: true));
            properties.Add(new JSBString("FtpPassword", "Password for the user registered on the server", isRequired: true));
            properties.Add(new JSBString("FtpServer", "Ftp host server", isRequired: true));
            properties.Add(new JSBString("FtpFolderOnServer", "Ftp folder on server if needed"));
            properties.Add(new JSBRef("FtpType", "Type of FTP", iriReference: new Uri("#/definitions/ftpType", UriKind.Relative), isRequired: true));

            JSBObject jsonObject = new JSBObject("FtpInformation", "Data For Copy and Move file operations on FTP", props: properties);
            return new JSBSchema("FtpInformation", Description, topPart: jsonObject, defs: definitions);
        }

        public override NamespaceString Module
        {
            get
            {
                return "Funny.Onion";
            }
        }

        public override string Description
        {
            get
            {
                return "Data For FTP operations";
            }
        }
    }

    public class UriReferenceAsTopPartJsonSchema : AbstractJsonSchema
    {
        public override NamespaceString Module
        {
            get
            {
                return "Funny.Onion";
            }
        }

        public override string Description
        {
            get
            {
                return "Used to test uri reference as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            if (Uri.TryCreate("./dateAsTopPart", UriKind.RelativeOrAbsolute, out Uri uri))
            {

                JSBRef uriReferencePart = new JSBRef("MyTopPartUriReference", "TopPart", iriReference: uri);

                return new JSBSchema("UriReferencesATopPart", Description, topPart: uriReferencePart);
            }
            else
            {
                throw new Exception("Uri not valid");
            }
        }
    }

    public class TimeAsTopPartJsonSchema : AbstractJsonSchema
    {
        public override NamespaceString Module
        {
            get
            {
                return "Funny.Onion";
            }
        }

        public override string Description
        {
            get
            {
                return "Used to test time as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            JSBTime timePart = new JSBTime("MyTopPartTime", "TopPart", defaultValue: new DateTime(2020, 1, 1, 15, 30, 48, 765));

            return new JSBSchema("TimeAsATopPart", Description, topPart: timePart);
        }
    }

    public class StringAsTopPartJsonSchema : AbstractJsonSchema
    {
        public override NamespaceString Module
        {
            get
            {
                return "Funny.Onion";
            }
        }

        public override string Description
        {
            get
            {
                return "Used to test string as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            JSBString stringPart = new JSBString("MyTopPartString", "TopPart");

            return new JSBSchema("StringAsATopPart", Description, topPart: stringPart);
        }
    }

    public class StringEnumAsTopPartJsonSchema : AbstractJsonSchema
    {
        public override NamespaceString Module
        {
            get
            {
                return "Funny.Onion";
            }
        }

        public override string Description
        {
            get
            {
                return "Used to test string enum as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            JSBString stringPart = new JSBString("MyTopPartString", "TopPart", enums: new List<string> { "Monster", "item", "Nusense" });

            return new JSBSchema("StringEnumAsATopPart", Description, topPart: stringPart);
        }
    }

    public class ObjectAsTopPartJsonSchema : AbstractJsonSchema
    {
        public override NamespaceString Module
        {
            get
            {
                return "Funny.Onion";
            }
        }

        public override string Description
        {
            get
            {
                return "Used to test object as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            JSBBoolean booleanPart = new JSBBoolean("BooleanPart", "BooleanPart for testing", isRequired: true);
            JSBInteger integerPart = new JSBInteger("IntegerPart", "IntegerPart for testing");

            List<IJSBPart> properties = new List<IJSBPart>();
            properties.Add(booleanPart);
            properties.Add(integerPart);

            JSBObject objectPart = new JSBObject("MyTopPartObject", "TopPart", props: properties);

            return new JSBSchema("ObjectAsATopPart", Description, topPart: objectPart);
        }
    }

    public class EmailAsTopPartJsonSchema : AbstractJsonSchema
    {
        public override NamespaceString Module
        {
            get
            {
                return "Funny.Onion";
            }
        }

        public override string Description
        {
            get
            {
                return "Used to test email as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            JSBEmail emailPart = new JSBEmail("MyTopPartEmail", "TopPart", defaultValue: "riger@support.com");

            return new JSBSchema("EmailAsATopPart", Description, topPart: emailPart);
        }
    }

    public class DateTimeAsTopPartJsonSchema : AbstractJsonSchema
    {
        public override NamespaceString Module
        {
            get
            {
                return "Funny.Onion";
            }
        }

        public override string Description
        {
            get
            {
                return "Used to test datetime as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            JSBDateTime dateTimePart = new JSBDateTime("MyTopPartDateTime", "TopPart", defaultValue: new DateTime(2015, 05, 16));

            return new JSBSchema("DateTimeAsATopPart", Description, topPart: dateTimePart);
        }
    }

    public class DateAsTopPartJsonSchema : AbstractJsonSchema
    {
        public override NamespaceString Module
        {
            get
            {
                return "Funny.Onion";
            }
        }

        public override string Description
        {
            get
            {
                return "Used to test date as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            JSBDate datePart = new JSBDate("MyTopPartDate", "TopPart", defaultValue: new DateTime(2015, 05, 16));

            return new JSBSchema("DateAsATopPart", Description, topPart: datePart);
        }
    }

    public class ArrayAsTopPartJsonSchema : AbstractJsonSchema
    {
        public override NamespaceString Module
        {
            get
            {
                return "Funny.Onion";
            }
        }

        public override string Description
        {
            get
            {
                return "Used to test string as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            List<IJSBPart> items = new List<IJSBPart>();

            items.Add(new JSBInteger("SwanNumber", "Swans are relevant in the world"));

            JSBArray arrayPart = new JSBArray("MyTopPartArray", "TopPart", items);

            return new JSBSchema("ArrayAsATopPart", Description, topPart: arrayPart);
        }
    }

    public class ArrayOfObjectsAsTopPartJsonSchema : AbstractJsonSchema
    {
        public override NamespaceString Module
        {
            get
            {
                return "Funny.Onion";
            }
        }

        public override string Description
        {
            get
            {
                return "Used to test object as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            JSBBoolean booleanPart = new JSBBoolean("BooleanPart", "BooleanPart for testing", isRequired: true);
            JSBInteger integerPart = new JSBInteger("IntegerPart", "IntegerPart for testing");

            List<IJSBPart> properties = new List<IJSBPart>();
            properties.Add(booleanPart);
            properties.Add(integerPart);

            JSBObject objectPart = new JSBObject("ObjectInAnArray", "ObjectInAnArray is fun", props: properties);

            List<IJSBPart> items = new List<IJSBPart>();
            items.Add(objectPart);
            JSBArray arrayPart = new JSBArray("MyTopPartArray", "TopPart", items);

            return new JSBSchema("ArrayOfObjectsAsATopPart", Description, topPart: arrayPart);
        }
    }

    public class ArrayOfArrayOfObjectsAsTopPartJsonSchema : AbstractJsonSchema
    {
        public override NamespaceString Module
        {
            get
            {
                return "Funny.Onion";
            }
        }

        public override string Description
        {
            get
            {
                return "Used to test array of arry of object as a top part";
            }
        }

        protected override JSBSchema BuildJsonSchema()
        {
            JSBBoolean booleanPart = new JSBBoolean("BooleanPart", "BooleanPart for testing", isRequired: true);
            JSBInteger integerPart = new JSBInteger("IntegerPart", "IntegerPart for testing");

            List<IJSBPart> properties = new List<IJSBPart>();
            properties.Add(booleanPart);
            properties.Add(integerPart);

            JSBObject objectPart = new JSBObject("ObjectInAnArrayOfAnArray", "ObjectInAnArrayOfAnArray is fun", props: properties);

            List<IJSBPart> innerItems = new List<IJSBPart>();
            innerItems.Add(objectPart);
            JSBArray innerArrayPart = new JSBArray("InnerArray", "InnerArrayPart", innerItems);

            List<IJSBPart> items = new List<IJSBPart>();
            items.Add(innerArrayPart);
            JSBArray arrayPart = new JSBArray("MyTopPartArrayOfArray", "TopPart", items);

            return new JSBSchema("ArrayOfObjectsAsATopPart", Description, topPart: arrayPart);
        }
    }
}
