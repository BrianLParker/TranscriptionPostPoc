using System.IO;
using RESTFulSense.Models.Attributes;

namespace TranscriptionPostPoc
{
    public class ExternalTranscriptionRequest
    {
        [RESTFulStreamContent(name: "file")]
        public Stream Audio { get; set; }

        [RESTFulFileName(name: "file")]
        public string File { get; set; }

        [RESTFulStringContent(name: "model")]
        public string Model { get; set; }

        [RESTFulStringContent(name: "prompt", ignoreDefaultValues: true)]
        public string Prompt { get; set; }

        [RESTFulStringContent(name: "response_format", ignoreDefaultValues: true)]
        public string ResponseFormat { get; set; }

        [RESTFulStringContent(name: "temperature", ignoreDefaultValues: true)]
        public double Temperature { get; set; }

        [RESTFulStringContent(name: "language", ignoreDefaultValues: true)]
        public string Language { get; set; }
    }
}