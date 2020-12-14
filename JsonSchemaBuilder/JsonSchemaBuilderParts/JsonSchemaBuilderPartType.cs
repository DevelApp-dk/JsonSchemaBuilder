using System;
using System.Collections.Generic;
using System.Text;

namespace DevelApp.JsonSchemaBuilder.JsonSchemaParts
{
    public enum JsonSchemaBuilderPartType
    {
        Object = 1,
        String = 2,
        Date = 3,
        DateTime = 4,
        Integer = 5,
        Array = 6,
        Number = 7,
        Boolean = 8,
        Base64Binary = 10,
        Email = 11,
        UriReference = 12,
        Schema = 13,
        Time = 14
    }
}
