using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenApiV3Parser
{
    /// <summary>
    /// Custom converter to properly handle "$ref" alongside normal schema fields.
    /// System.Text.Json can't map "$ref" to "Ref" automatically without help.
    /// </summary>
    public sealed class OpenApiSchemaConverter : JsonConverter<OpenApiSchema>
    {
        public override OpenApiSchema Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException($"Expected schema object, got {reader.TokenType}.");

            using (var doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;

                var schema = new OpenApiSchema();

                // $ref
                if (root.TryGetProperty("$ref", out var refProp) && refProp.ValueKind == JsonValueKind.String)
                    schema.Ref = refProp.GetString();

                // Standard fields
                if (root.TryGetProperty("title", out var title) && title.ValueKind == JsonValueKind.String)
                    schema.Title = title.GetString();

                if (root.TryGetProperty("type", out var type) && type.ValueKind == JsonValueKind.String)
                    schema.Type = type.GetString();

                if (root.TryGetProperty("format", out var format) && format.ValueKind == JsonValueKind.String)
                    schema.Format = format.GetString();

                if (root.TryGetProperty("nullable", out var nullable) && (nullable.ValueKind == JsonValueKind.True || nullable.ValueKind == JsonValueKind.False))
                    schema.Nullable = nullable.GetBoolean();

                if (root.TryGetProperty("description", out var desc) && desc.ValueKind == JsonValueKind.String)
                    schema.Description = desc.GetString();

                // properties
                if (root.TryGetProperty("properties", out var props) && props.ValueKind == JsonValueKind.Object)
                {
                    schema.Properties = new Dictionary<string, OpenApiSchema>(StringComparer.OrdinalIgnoreCase);
                    foreach (var p in props.EnumerateObject())
                    {
                        schema.Properties[p.Name] = JsonSerializer.Deserialize<OpenApiSchema>(p.Value.GetRawText(), options)
                                                    ?? new OpenApiSchema();
                    }
                }

                // required
                if (root.TryGetProperty("required", out var req) && req.ValueKind == JsonValueKind.Array)
                {
                    schema.Required = req.EnumerateArray()
                                        .Where(x => x.ValueKind == JsonValueKind.String)
                                        .Select(x => x.GetString())
                                        .ToList();
                }

                // items (arrays)
                if (root.TryGetProperty("items", out var items) && items.ValueKind == JsonValueKind.Object)
                {
                    schema.Items = JsonSerializer.Deserialize<OpenApiSchema>(items.GetRawText(), options);
                }

                // oneOf/anyOf/allOf
                schema.OneOf = ReadSchemaArray(root, "oneOf", options);
                schema.AnyOf = ReadSchemaArray(root, "anyOf", options);
                schema.AllOf = ReadSchemaArray(root, "allOf", options);

                // enum/default as raw JSON
                if (root.TryGetProperty("enum", out var en))
                    schema.Enum = en.Clone();

                if (root.TryGetProperty("default", out var def))
                    schema.Default = def.Clone();

                // Capture unknown schema fields (extensions)
                var known = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "$ref","title","type","format","nullable","description",
            "properties","required","items","oneOf","anyOf","allOf","enum","default"
        };

                var extras = new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
                foreach (var p in root.EnumerateObject())
                {
                    if (!known.Contains(p.Name))
                        extras[p.Name] = p.Value.Clone();
                }
                if (extras.Count > 0)
                    schema.Extensions = extras;

                return schema;

            }

        }

        private static List<OpenApiSchema> ReadSchemaArray(JsonElement root, string propName, JsonSerializerOptions options)
        {
            if (!root.TryGetProperty(propName, out var arr) || arr.ValueKind != JsonValueKind.Array)
                return null;

            var list = new List<OpenApiSchema>();
            foreach (var el in arr.EnumerateArray())
            {
                if (el.ValueKind == JsonValueKind.Object)
                {
                    var item = JsonSerializer.Deserialize<OpenApiSchema>(el.GetRawText(), options);
                    if (item != null) list.Add(item);
                }
            }
            return list.Count > 0 ? list : null;
        }

        public override void Write(Utf8JsonWriter writer, OpenApiSchema value, JsonSerializerOptions options)
            => throw new NotSupportedException("Serialization not implemented (parsing-only model).");
    }
}