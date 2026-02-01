using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenApiV3Parser
{
    public sealed class OpenApiParameter
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("in")]
        public string In { get; set; } // query, path, header, cookie

        [JsonPropertyName("required")]
        public bool Required { get; set; }

        [JsonPropertyName("schema")]
        public OpenApiSchema Schema { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement> Extensions { get; set; }
    }
}