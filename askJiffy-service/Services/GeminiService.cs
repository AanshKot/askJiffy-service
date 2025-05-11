
using askJiffy_service.Configuration;
using askJiffy_service.Models.Responses.Chat;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace askJiffy_service.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly string _geminiAPIKey;
        private readonly string _geminiModel;
        private readonly HttpClient _httpClient;
        
        public GeminiService(HttpClient httpClient, IOptions<GeminiServiceOptions> options)
        {
            _geminiAPIKey = options.Value.ApiKey;
            _geminiModel = options.Value.Model;
            _httpClient = httpClient;

            //typed clients https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-9.0
            _httpClient.BaseAddress = new Uri(options.Value.GeminiDefaultBaseDomain);
        }

        // makes POST request to generate a title based on provided prompt
        public async Task<string> GenerateTitleAsync(string prompt)
        {
            var requestUri = $"v1beta/models/{_geminiModel}:generateContent?key={_geminiAPIKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    { 
                        parts = new { text = prompt }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUri, content);

            if(!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Gemini API call failed: { error }");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
             );
            var title = geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;   

            if(title == null) {
                throw new ApplicationException($"Gemini API call failed: Failed to retrieve title");
            }

            return title;
        }

        // Streams a sequence of strings asynchronously
        // yielding one at a time
        // allowing the ChatBL to consume these responses with an 'await foreach' loop
        public async IAsyncEnumerable<string> StreamAnswerAsync(string vehicleContext, string message)
        {
            var requestUri = $"v1beta/models/{_geminiModel}:streamGenerateContent?alt=sse&key={_geminiAPIKey}";

            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var question = $"This is the vehicle {vehicleContext} that the question is referencing, use this for background to answer the user's question/message: {message}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new { text = question }
                    }
                }
            };

            var jsonBody = JsonSerializer.Serialize(requestBody);

            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // HttpClient returns as soon as the headers are available, (look at docs for further description)
            // allowing the caller to start processing the response stream immediately
            // response body is not buffered 
            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            var stream = await response.Content.ReadAsStreamAsync();

            var streamReader = new StreamReader(stream);

            // look at google doc for approximate return type AND explanation of conditions
            // line is the server-sent event (SSE) message recieved from Gemini's api, has data: prefix indicating start of actual data in message 
            // each message may add new text
            // stream ends with event: done

            while (!streamReader.EndOfStream)
            {
                var line = await streamReader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data:")) continue;

                // Uses the range operator ".." to remove the "data:" prefix, leaving only the actual JSON payload.
                var json = line["data:".Length..].Trim();

                //last message in response stream, signifies end of stream
                if (json == "{}") break;

                var obj = JsonSerializer.Deserialize<GeminiResponse>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                var text = obj?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

                if (!string.IsNullOrEmpty(text))
                {
                    // yield return allows creation of an iterator method
                    // method that lazily returns values one at a time
                    // went with IAsyncEnumerable<string> return type because streaming response from async Gemini API
                    yield return text;
                }
            }
        }
    }
}
