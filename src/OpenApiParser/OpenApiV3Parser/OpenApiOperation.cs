using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenApiV3Parser
{
    public sealed class OpenApiOperation
    {
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("operationId")]
        public string OperationId { get; set; }

        [JsonPropertyName("parameters")]
        public List<OpenApiParameter> Parameters { get; set; }

        [JsonPropertyName("requestBody")]
        public OpenApiRequestBody RequestBody { get; set; }

        /// <summary>
        /// Map: HTTP status code ("200", "400", "default") -> response
        /// </summary>
        [JsonPropertyName("responses")]
        public Dictionary<string, OpenApiResponse> Responses { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement> Extensions { get; set; }
    }
}
