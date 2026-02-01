using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OpenApiV3Parser
{
    public class Endpoint
    {
        public string Path { get; set; }
        public string Method { get; set; } // GET, POST, PUT, DELETE, etc.

        public string EntityGroup { get; set; }

        public Endpoint(string path, string method)
        {
            Path = path;
            Method = method;
        }

        public List<OpenApiParameterFlat> Parameters { get; set; }

        public string RequestSchemaName { get; set; }

        public string ResponseSchemaName { get; set; }
    }

    public class OpenApiParameterFlat
    {
        public string In { get; set; }
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public string Description { get; set; }
        public string PropertyType { get; set; }
        public string Format { get; set; }
    }
    public class OpenApiDefinition
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string OpenApiVersion { get; set; }
        public List<Endpoint> Endpoints { get; set; } = new List<Endpoint>();

        public List<string> EntityGroups
        {
            get
            {
                return (from p in Endpoints select p.EntityGroup).Distinct().ToList();
            }
        }

        public List<SchemaObjectType> Schemas { get; set; } = new List<SchemaObjectType>();
    }

    public class SchemaObjectType
    {
        public SchemaObjectType(string schemaName)
        {
            this.Name = schemaName;
        }
        public string Name { get; set; }

        public List<SchemaProperty> Properties { get; set; } = new List<SchemaProperty>();
    }

    public class SchemaProperty
    {
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public string Format { get; set; }
        public bool IsNullable { get; set; }

        public bool IsReference { get; set; }

        public string RefSchemaName { get; set; }

        public string Description { get; set; }
    }
}
