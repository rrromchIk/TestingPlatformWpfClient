using System;
using System.Net.Http.Headers;
using System.Net.Http;

namespace TestingPlatformWpfClient.Services {
    public class CommonHttpClientService {
        public HttpClient HttpClient { get; }

        public CommonHttpClientService() {
            HttpClient = new HttpClient {
                BaseAddress = new Uri("https://localhost:7003/api/")
            };

            SetupHttpClient();
        }

        private void SetupHttpClient() {
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/problem+json"));
            HttpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            HttpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("ISO-8859-1"));
        }
    }
}