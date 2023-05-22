using Newtonsoft.Json;

namespace TranscriptionPostPoc
{
    public class ExternalTranscriptionResponse
    {
        [JsonProperty(propertyName: "text")]
        public string Text { get; set; }
    }
}