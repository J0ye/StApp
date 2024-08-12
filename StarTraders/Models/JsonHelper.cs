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


        public static async Task<Dictionary<string, dynamic>> MakeRequest(string token, string requestType)
        {
            return await MakeRequest(token, requestType, HttpMethod.Get);
        }

        public static async Task<Dictionary<string, dynamic>> MakeRequest(string token, string requestType, HttpMethod method, dynamic body = null)
        {
            var options = new HttpRequestMessage(method, "https://api.spacetraders.io/v2/" + requestType);
            options.Headers.Add("Authorization", "Bearer " + token);
            if (body != null)
            {
                options.Content = new StringContent(JsonConvert.SerializeObject(body));
            }

            using (var client = new HttpClient())
            {
                var response = await client.SendAsync(options);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return FromJson(jsonResponse);
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