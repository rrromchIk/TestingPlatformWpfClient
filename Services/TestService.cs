using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TestingPlatformWpfClient.Exceptions;
using TestingPlatformWpfClient.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TestingPlatformWpfClient.Services {
    public class TestService : ITestService {
        private readonly HttpClient _httpClient;

        public TestService(CommonHttpClientService commonHttpClientService) {
            _httpClient = commonHttpClientService.HttpClient;
        }

        public async Task<IEnumerable<Test>> GetAllTestsAsync() {
            var response = await _httpClient.GetAsync("tests");
            return await HandleResponseAsync<IEnumerable<Test>>(response);
        }

        public async Task<Test> GetTestByIdAsync(int id) {
            var response = await _httpClient.GetAsync($"tests/{id}");
            return await HandleResponseAsync<Test>(response);
        }

        public async Task<Test> CreateTestAsync(Test test) {
            var response = await _httpClient.PostAsJsonAsync("tests", test);
            return await HandleResponseAsync<Test>(response);
        }

        public async Task UpdateTestAsync(int id, Test test) {
            var response = await _httpClient.PutAsJsonAsync($"tests/{id}", test);
            await HandleResponseAsync<HttpResponseMessage>(response);
        }

        public async Task DeleteTestAsync(int id) {
            var response = await _httpClient.DeleteAsync($"tests/{id}");
            await HandleResponseAsync<HttpResponseMessage>(response);
        }

        private static async Task<T> HandleResponseAsync<T>(HttpResponseMessage response) {
            if (!response.IsSuccessStatusCode) {
                if (response.StatusCode == HttpStatusCode.NotFound) {
                    return default(T);
                }

                var content = await response.Content.ReadAsStringAsync();
                var errorDetails = JsonConvert.DeserializeObject<ApiErrorDetails>(content);
                throw new ApiException(errorDetails.Title, (int)response.StatusCode);
            }

            if (typeof(T) == typeof(HttpResponseMessage)) {
                // When T is HttpResponseMessage, return the response itself
                return (T)(object)response;
            }

            await using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
