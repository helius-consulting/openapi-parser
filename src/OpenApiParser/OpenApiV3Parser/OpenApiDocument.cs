using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenApiV3Parser
{
    /// <summary>
    /// Root document for OpenAPI.
    /// </summary>
    public sealed class OpenApiDocument
    {
        [JsonPropertyName("openapi")]
        public string OpenApiVersion { get; set; }

        [JsonPropertyName("info")]
        public OpenApiInfo Info { get; set; }

        /// <summary>
        /// Map: path template -> path item (operations).
        /// Example: "/api/v1/Auth/Login" -> { post: ... }
        /// </summary>
        [JsonPropertyName("paths")]
        public Dictionary<string, OpenApiPathItem> Paths { get; set; }

        [JsonPropertyName("components")]
        public OpenApiComponents Components { get; set; }

        // Keep any top-level extensions/unknown fields
        [JsonExtensionData]
        public Dictionary<string, JsonElement> Extensions { get; set; }
    }
}