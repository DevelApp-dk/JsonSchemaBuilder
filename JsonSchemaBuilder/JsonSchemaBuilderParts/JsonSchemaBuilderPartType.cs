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
        Email = 11,
        IriReference = 12,
        Schema = 13,
        Time = 14
    }
}
