using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace OpenApiV3Parser.Mappings
{
    public static class OpenApiDefinitionMapper
    {
        public static OpenApiDefinition Map(OpenApiV3Parser.OpenApiDocument document)
        {
            var definition = new OpenApiDefinition();
            foreach (var pathItem in document.Paths)
            {
                definition.Title = document.Info.Title;
                definition.Version = document.Info.Version;
                definition.Description = document.Info.Description;

                foreach (var method in pathItem.Value.EnumerateOperations())
                {
                    var endpoint = new Endpoint(pathItem.Key, method.Method.ToUpper())
                    {
                        EntityGroup = method.Operation.Tags.FirstOrDefault(),
                        Parameters = GetParameters(method.Operation),
                        RequestSchemaName = GetRequestNameFromSchema(method.Operation.RequestBody),
                        ResponseSchemaName = GetResponseNameFromSchema(method.Operation.Responses)
                    };

                    definition.Endpoints.Add(endpoint);
                }

                //var path = pathItem.Key;
                //var operations = pathItem.EnumerateOperations();
                //foreach (var operationItem in operations)
                //{
                //    var method = operationItem.Key.ToString().ToUpper();
                //    var operation = operationItem.Value;
                //    var endpoint = new Endpoint(path, method)
                //    {
                //        EntityGroup = ExtractEntityGroup(operation.Tags),
                //        Parameters = operation.Parameters,
                //        Request = operation.RequestBody,
                //        Response = operation.Responses.ContainsKey("200") ? operation.Responses["200"] : null
                //    };
                //    definition.Endpoints.Add(endpoint);
                //}
            }

            definition.Schemas = ExtractSchemas(document);

            return definition;
        }

        private static List<OpenApiParameterFlat> GetParameters(OpenApiOperation operation)
        {
            if (operation.Parameters == null)
            {
                return new List<OpenApiParameterFlat>();
            }

            if (operation.Parameters.Count > 0)
            {
                return (from p in operation.Parameters
                        select new OpenApiParameterFlat()
                        {
                            Name = p.Name,
                            In = p.In,
                            IsRequired = p.Required,
                            Description = p.Description,
                            PropertyType = p.Schema?.Type,
                            Format = p.Schema?.Format
                        }).ToList();
            }

            return new List<OpenApiParameterFlat>();
        }

        private static List<SchemaObjectType> ExtractSchemas(OpenApiV3Parser.OpenApiDocument document)
        {
            var schemas = new List<SchemaObjectType>();
            if (document.Components?.Schemas != null)
            {
                var allKeys = document.Components.Schemas.Keys;
                foreach (var schemaName in allKeys)
                {
                    var schemaObject = GetFromSchema(document, schemaName);
                    schemas.Add(schemaObject);
                }
            }
            return schemas;
        }

        private static string GetRequestNameFromSchema(OpenApiRequestBody requestBody)
        {
            if (requestBody == null)
            {
                return null;
            }

            var schema = requestBody.Content.ContainsKey("application/json")
                ? requestBody.Content["application/json"].Schema
                : null;

            if (schema != null)
            {
                return schema.RefSchemaName;
            }

            return null;
        }

        private static string GetResponseNameFromSchema(Dictionary<string, OpenApiResponse> responses)
        {
            var responseOK = responses.ContainsKey("200") ? responses["200"] : null;

            if (responseOK == null)
            {
                return null;
            }

            var schema = responseOK.Content.ContainsKey("application/json")
                ? responseOK.Content["application/json"].Schema
                : null;

            if (schema != null)
            {
                return schema.RefSchemaName;
            }

            return null;
        }

        private static SchemaObjectType GetFromSchema(OpenApiDocument doc, string schemaName)
        {
            var schemaObject = new SchemaObjectType(schemaName);
            if (doc.Components?.Schemas != null && doc.Components.Schemas.TryGetValue(schemaName, out var requestType))
            {
                foreach (var key in requestType.Properties.Keys)
                {
                    var prop1 = requestType.Properties[key];

                    var mapping1 = new SchemaProperty()
                    {
                        PropertyName = key,
                        PropertyType = prop1.Type,
                        Description = prop1.Description,
                        Format = prop1.Format,
                        IsReference = prop1.Items != null && !string.IsNullOrEmpty(prop1.Items.RefSchemaName),
                        RefSchemaName = prop1.Items != null ? prop1.Items.RefSchemaName : prop1.RefSchemaName,
                        IsNullable = prop1.Nullable.HasValue ? prop1.Nullable.Value : false,
                    };

                    schemaObject.Properties.Add(mapping1);
                }
            }

            return schemaObject;
        }
    }


}
