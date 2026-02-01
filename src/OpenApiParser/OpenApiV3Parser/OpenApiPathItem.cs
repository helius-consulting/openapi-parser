using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenApiV3Parser
{
    /// <summary>
    /// A PathItem contains operations (get/post/put/delete/etc) for a single path.
    /// </summary>
    public sealed class OpenApiPathItem
    {
        // OpenAPI supports these methods; add more if you want.
        [JsonPropertyName("get")]
        public OpenApiOperation Get { get; set; }

        [JsonPropertyName("post")]
        public OpenApiOperation Post { get; set; }

        [JsonPropertyName("put")]
        public OpenApiOperation Put { get; set; }

        [JsonPropertyName("delete")]
        public OpenApiOperation Delete { get; set; }

        [JsonPropertyName("patch")]
        public OpenApiOperation Patch { get; set; }

        [JsonPropertyName("head")]
        public OpenApiOperation Head { get; set; }

        [JsonPropertyName("options")]
        public OpenApiOperation Options { get; set; }

        [JsonPropertyName("trace")]
        public OpenApiOperation Trace { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement> Extensions { get; set; }

        public IEnumerable<(string Method, OpenApiOperation Operation)> EnumerateOperations()
        {
            if (Get != null) yield return ("GET", Get);
            if (Post != null) yield return ("POST", Post);
            if (Put != null) yield return ("PUT", Put);
            if (Delete != null) yield return ("DELETE", Delete);
            if (Patch != null) yield return ("PATCH", Patch);
            if (Head != null) yield return ("HEAD", Head);
            if (Options != null) yield return ("OPTIONS", Options);
            if (Trace != null) yield return ("TRACE", Trace);
        }

        internal void Normalize()
        {
            // currently nothing required; this exists if you want to enforce defaults later.
        }
    }
}