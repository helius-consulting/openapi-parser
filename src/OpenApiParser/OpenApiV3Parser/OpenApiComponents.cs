using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenApiV3Parser
{
    public sealed class OpenApiComponents
    {
        /// <summary>
        /// Map: schema name -> schema definition (the "types").
        /// Example: "LoginRequest" -> { type: "object", properties: ... }
        /// </summary>
        [JsonPropertyName("schemas")]
        public Dictionary<string, OpenApiSchema> Schemas { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement> Extensions { get; set; }
    }
}