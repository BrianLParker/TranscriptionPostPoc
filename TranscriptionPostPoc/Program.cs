// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RESTFulSense.Clients;
using Standard.AI.OpenAI.Models.Configurations;
using TranscriptionPostPoc.Models;

namespace TranscriptionPostPoc
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IRESTFulApiFactoryClient apiClient = CreateApiClient();

            Stream m4aStream = GetEmbeddedResourceStream();

            ExternalTranscriptionRequest someTranscription = new ExternalTranscriptionRequest
            {
                Audio = m4aStream,
                File = "Welcome.m4a",
                Model = "whisper-1",
                Language = "en"
            };

            var response =
                 await apiClient.PostFormAsync<ExternalTranscriptionRequest, ExternalTranscriptionResponse>(
                     relativeUrl: "v1/audio/transcriptions",
                     someTranscription);

            Console.WriteLine(response.Text);
        }

        private static Stream GetEmbeddedResourceStream()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            return assembly.GetManifestResourceStream(name: "TranscriptionPostPoc.Resources.Welcome.m4a");
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
