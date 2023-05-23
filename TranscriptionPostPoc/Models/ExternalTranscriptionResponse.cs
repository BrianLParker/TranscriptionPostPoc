// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace TranscriptionPostPoc.Models
{
    public class ExternalTranscriptionResponse
    {
        [JsonProperty(propertyName: "text")]
        public string Text { get; set; }
    }
}
