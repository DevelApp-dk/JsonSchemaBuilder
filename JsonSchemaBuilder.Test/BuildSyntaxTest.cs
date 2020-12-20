using DevelApp.JsonSchemaBuilder;
using DevelApp.JsonSchemaBuilder.JsonSchemaParts;
using DevelApp.Utility.Model;
using Manatee.Json.Schema;
using System;
using System.Collections.Generic;
using Xunit;

namespace JsonSchemaBuilder.Test
{
    public class BuildSyntaxTest
    {
        [Fact]
        public void BuildObject()
        {
            List<IJSBPart> properties = new List<IJSBPart>();
            string objectName = "ObjectName";
            string description = "ObjectDescription";
            bool isRequired = false;
            bool isExpandable = false;

            var varObject = new JSBObject(objectName, description, properties, isRequired: isRequired, isExpandable: isExpandable);
            JsonSchema varSchema = varObject.AsJsonSchema();
        }

        [Fact]
        public void BuildBoolean()
        {
            string objectName = "BooleanName";
            string description = "BooleanDescription";
            bool? defaultValue = true;
            bool isRequired = false;

            var varBoolean = new JSBBoolean(objectName, description, defaultValue, isRequired);

            JsonSchema varSchema = varBoolean.AsJsonSchema();
        }

        [Fact]
        public void BuildString()
        {
            string objectName = "StringName";
            string description = "StringDescription";
            string defaultValue = "default";
            bool isRequired = false;
            uint minLength = 4;
            uint maxLength = 8;
            string pattern = "fau";

            var varString = new JSBString(objectName, description, minLength:minLength, 
                maxLength:maxLength, pattern: pattern, defaultValue: defaultValue, isRequired: isRequired);

            JsonSchema varSchema = varString.AsJsonSchema();
        }

        [Fact]
        public void BuildString_Enum_Simple()
        {
            string objectName = "StringName";
            string description = "StringDescription";
            List<string> enums = new List<string>() { "FTP", "Custom", "NotSoCustom", "Default" };

            var varString = new JSBString(objectName, description, enums: enums);

            JsonSchema varSchema = varString.AsJsonSchema();
        }

        [Fact]
        public void BuildString_Enum()
        {
            string objectName = "StringName";
            string description = "StringDescription";
            string defaultValue = "default";
            bool isRequired = false;
            uint minLength = 4;
            uint maxLength = 8;
            string pattern = "fau";
            List<string> enums = new List<string>() { "FTP", "Custom", "NotSoCustom", "Default" };

            var varString = new JSBString(objectName, description, minLength: minLength,
                maxLength: maxLength, pattern: pattern, defaultValue: defaultValue, isRequired: isRequired, enums: enums);

            JsonSchema varSchema = varString.AsJsonSchema();
        }

        [Fact]
        public void BuildReference_Relative()
        {
            string refName = "refName";
            string refDesc = "Description of refName";
            Uri uri = new Uri("./address.schema.json", UriKind.Relative);

            var varref = new JSBRef(refName, refDesc, uri);

            JsonSchema varSchema = varref.AsJsonSchema();
        }

        [Fact]
        public void BuildReference_Relative_WithFragment()
        {
            string refName = "refName";
            string refDesc = "Description of refName";
            Uri uri = new Uri("./address.schema.json#/definitions/ftp", UriKind.Relative);

            var varref = new JSBRef(refName, refDesc, uri);

            JsonSchema varSchema = varref.AsJsonSchema();
        }

        [Fact]
        public void BuildReference_Absolute()
        {
            string refName = "refName";
            string refDesc = "Description of refName";
            Uri uri = new Uri("///E:/Projects/Funny/Onion/address.schema.json", UriKind.Relative);

            var varref = new JSBRef(refName, refDesc, uri);

            JsonSchema varSchema = varref.AsJsonSchema();
        }

        [Fact]
        public void BuildReference_Absolute_WithFragment()
        {
            string refName = "refName";
            string refDesc = "Description of refName";
            Uri uri = new Uri("///E:/Projects/Funny/Onion/address.schema.json#/definitions/ftp", UriKind.Relative);

            var varref = new JSBRef(refName, refDesc, uri);

            JsonSchema varSchema = varref.AsJsonSchema();
        }
    }
}
