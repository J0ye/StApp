using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StApp
{
    public static class JsonHelper
    {
        public static T FromJson<T>(string json)
        {
            string trimmedJson = TrimString(json);
            var rootObject = JsonConvert.DeserializeObject<T>(trimmedJson);
            return rootObject;
        }

        public static Dictionary<string, dynamic> FromJson(string json)
        {
            var rootObject = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
            return rootObject;
        }


        public static async Task<Dictionary<string, dynamic>> MakeRequest(string token, string requestType, bool logRequest = false)
        {
            return await MakeRequest(token, requestType, HttpMethod.Get, logRequest);
        }

        public static async Task<Dictionary<string, dynamic>> MakeRequest(string token, string requestType, HttpMethod method, bool logRequest, dynamic body = null)
        {
            // Create the HTTP request message with the specified method and URL
            var options = new HttpRequestMessage(method, "https://api.spacetraders.io/v2/" + requestType);
            // Add authorization header if token is provided
            if(!String.IsNullOrEmpty(token)) options.Headers.Add("Authorization", "Bearer " + token);

            // If a body is provided, serialize it to JSON and set it as the content of the request
            if (body != null)
            {
                options.Content = new StringContent(JsonConvert.SerializeObject(body));
            }

            using (var client = new HttpClient())
            {
                if(logRequest) Console.WriteLine("Request: " + options.RequestUri);
                try
                {
                    // Send the HTTP request and await the response
                    var response = await client.SendAsync(options);
                    // Ensure the response indicates success (status code 2xx)
                    response.EnsureSuccessStatusCode();
                    // Read the response content as a string
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    // Deserialize the JSON response into a Dictionary and return it
                    return FromJson(jsonResponse);
                }
                catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Handle 404 Not Found
                    throw new Exception("Resource not found at: " + options.RequestUri, ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message} at {options.RequestUri}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Trims the input string by removing leading characters until the second '{' and deletes the last '}'.
        /// </summary>
        /// <param name="input">The input string to be trimmed.</param>
        /// <returns>The trimmed string.</returns>
        public static string TrimString(string input)
        {
            int firstBrace = input.IndexOf('{');
            int secondBrace = input.IndexOf('{', firstBrace + 1);
            if (secondBrace == -1) return input; // No second '{' found

            string trimmed = input.Substring(secondBrace);
            if (trimmed.EndsWith("}"))
            {
                trimmed = trimmed.Substring(0, trimmed.Length - 1); // Remove last '}'
            }
            return trimmed;
        }
    }
}