using OpenApiV3Parser;
using OpenApiV3Parser.Mappings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace OpenApiV3Parser
{
    public static class OpenApiParser
    {
        public static OpenApiDefinition Parse(string openApiJsonPath)
        {
            var doc = ParseFromFile(openApiJsonPath);
            var definition = OpenApiDefinitionMapper.Map(doc);
            return definition;
        }

        public static OpenApiDocument ParseFromFile(string openApiJsonPath)
        {
            if (string.IsNullOrWhiteSpace(openApiJsonPath))
                throw new ArgumentException("Path is required.", nameof(openApiJsonPath));

            if (!File.Exists(openApiJsonPath))
                throw new FileNotFoundException("OpenAPI file not found.", openApiJsonPath);

            // Avoid File.ReadAllText default encoding ambiguity in some environments
            var json = File.ReadAllText(openApiJsonPath, Encoding.UTF8);
            return ParseFromJson(json);
        }

        public static OpenApiDocument ParseFromJson(string openApiJson)
        {
            if (openApiJson == null) throw new ArgumentNullException(nameof(openApiJson));

            var doc = JsonSerializer.Deserialize<OpenApiDocument>(openApiJson, OpenApiJson.Options)
                      ?? throw new InvalidOperationException("Failed to deserialize OpenAPI document.");

            if (doc.Paths == null)
                doc.Paths = new Dictionary<string, OpenApiPathItem>(StringComparer.OrdinalIgnoreCase);
            if (doc.Components == null)
                doc.Components = new OpenApiComponents();
            if (doc.Components.Schemas == null)
                doc.Components.Schemas = new Dictionary<string, OpenApiSchema>(StringComparer.OrdinalIgnoreCase);

            foreach (var p in doc.Paths.Values)
                p.Normalize();

            return doc;
        }

        public static OpenApiDocument ParseFromStream(Stream openApiJsonStream)
        {
            if (openApiJsonStream == null) throw new ArgumentNullException(nameof(openApiJsonStream));

            using (var ms = new MemoryStream())
            {
                openApiJsonStream.CopyTo(ms);
                var json = Encoding.UTF8.GetString(ms.ToArray());
                return ParseFromJson(json);
            }
        }
    }
}