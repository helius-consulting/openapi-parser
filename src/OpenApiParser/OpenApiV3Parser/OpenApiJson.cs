using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenApiV3Parser
{
    /// <summary>
    /// Shared JsonSerializerOptions configured for OpenAPI documents.
    /// </summary>
    public static class OpenApiJson
    {
        public static readonly JsonSerializerOptions Options = CreateOptions();

        private static JsonSerializerOptions CreateOptions()
        {
            var opts = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // Keep unknown fields so you don't lose data.
            // Converters handle odd OpenAPI shapes ($ref, oneOf/anyOf, etc.)
            opts.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            opts.Converters.Add(new OpenApiSchemaConverter());

            return opts;
        }
    }
}