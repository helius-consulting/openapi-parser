using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenApiV3Parser
{
    /// <summary>
    /// Represents an OpenAPI Schema object.
    /// This can represent inline schemas or references via $ref.
    /// </summary>
    [JsonConverter(typeof(OpenApiSchemaConverter))]
    public sealed class OpenApiSchema
    {
        /// <summary>
        /// $ref like "#/components/schemas/LoginRequest"
        /// </summary>
        public string Ref { get; set; }

        public string Title { get; set; }
        public string Type { get; set; }           // object, array, string, integer, number, boolean
        public string Format { get; set; }         // int32, int64, date-time, etc.
        public bool? Nullable { get; set; }

        public Dictionary<string, OpenApiSchema> Properties { get; set; }
        public List<string> Required { get; set; }

        public OpenApiSchema Items { get; set; }   // for arrays
        public List<OpenApiSchema> OneOf { get; set; }
        public List<OpenApiSchema> AnyOf { get; set; }
        public List<OpenApiSchema> AllOf { get; set; }

        public JsonElement? Enum { get; set; }      // keep as raw json (string/int enums etc.)
        public JsonElement? Default { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Captures any other schema keywords you didn't explicitly model.
        /// </summary>
        public Dictionary<string, JsonElement> Extensions { get; set; }

        public bool IsRef => !string.IsNullOrWhiteSpace(Ref);

        public string RefSchemaName
        {
            get { 
                return GetRefSchemaName() ?? string.Empty;
            }
        }

        public string GetRefSchemaName()
        {
            // "#/components/schemas/LoginRequest" -> "LoginRequest"
            if (string.IsNullOrWhiteSpace(Ref)) return null;
            var parts = Ref.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            return parts.LastOrDefault();
        }
    }
}