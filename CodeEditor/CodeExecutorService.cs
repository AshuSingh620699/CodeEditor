using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CodeEditor
{
    public class CodeExecutorService
    {
        private readonly HttpClient _httpClient;

        public CodeExecutorService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> ExecuteCodeAsync(string language, string code, string input)
        {
            var requestBody = new
            {
                language = language,
                code = code,
                input = input
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("http://localhost:8000/api/execute/", content);
                response.EnsureSuccessStatusCode(); // Throw if not 200-299

                var responseBody = await response.Content.ReadAsStringAsync();

                // Parse the JSON result and extract the output
                dynamic result = JsonConvert.DeserializeObject(responseBody);
                return result.output;
            }
            catch (HttpRequestException ex)
            {
                return $"HTTP error: {ex.Message}";
            }
            catch (TaskCanceledException)
            {
                return "Execution timed out.";
            }
            catch (Exception ex)
            {
                return $"Unexpected error: {ex.Message}";
            }
        }
    }
}