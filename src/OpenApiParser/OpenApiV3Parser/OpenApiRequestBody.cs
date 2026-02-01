using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenApiV3Parser
{
    public sealed class OpenApiRequestBody
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("required")]
        public bool? Required { get; set; }

        /// <summary>
        /// Map: media type ("application/json", "text/json", "application/*+json") -> MediaType
        /// </summary>
        [JsonPropertyName("content")]
        public Dictionary<string, OpenApiMediaType> Content { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement> Extensions { get; set; }
    }
}