using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RESTFulSense.Clients;
using Standard.AI.OpenAI.Models.Configurations;

namespace TranscriptionPostPoc
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IRESTFulApiFactoryClient apiClient = CreateApiClient();

            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(name: "TranscriptionPostPoc.Resources.test.mp3");

            ExternalTranscriptionRequest someTranscription = new ExternalTranscriptionRequest
            {
                Audio = stream,
                File = "test.mp3",
                Model = "whisper-1",
                Language = "en",
            };

            var response =
                 await apiClient.PostFormAsync<ExternalTranscriptionRequest, ExternalTranscriptionResponse>(
                     relativeUrl: "v1/audio/transcriptions",
                     someTranscription);

            Console.WriteLine(response.Text);
        }

        private static IRESTFulApiFactoryClient CreateApiClient()
        {
            OpenAIConfigurations openAIConfigurations = GetApiConfigFromEnvironmentVariables();

            var httpClient = new HttpClient()
            {
                BaseAddress =
                       new Uri(uriString: openAIConfigurations.ApiUrl),
            };

            httpClient.DefaultRequestHeaders.Authorization =
                   new AuthenticationHeaderValue(
                       scheme: "Bearer",
                       parameter: openAIConfigurations.ApiKey);

            httpClient.DefaultRequestHeaders.Add(
                name: "OpenAI-Organization",
                value: openAIConfigurations.OrganizationId);

            IRESTFulApiFactoryClient apiClient = new RESTFulApiFactoryClient(httpClient);

            return apiClient;
        }

        private static OpenAIConfigurations GetApiConfigFromEnvironmentVariables()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                    .Build();

            OpenAIConfigurations openAIConfigurations =
                config.GetSection(key: "OpenAI").Get<OpenAIConfigurations>();

            return openAIConfigurations;
        }
    }
}
