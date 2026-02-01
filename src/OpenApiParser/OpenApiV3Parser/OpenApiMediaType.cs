using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenApiV3Parser
{
    public sealed class OpenApiMediaType
    {
        [JsonPropertyName("schema")]
        public OpenApiSchema Schema { get; set; }

        [JsonPropertyName("example")]
        public JsonElement? Example { get; set; }

        [JsonPropertyName("examples")]
        public Dictionary<string, JsonElement> Examples { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement> Extensions { get; set; }
    }
}