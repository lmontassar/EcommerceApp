using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EcommerceApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "http://192.168.43.66:8089/api";

        //private const string BaseUrl = "http://192.168.0.192:8089/api";

        public ApiService()
        {
            _client = new HttpClient();
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{endpoint}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{BaseUrl}/{endpoint}", content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public async Task<T> PutAsync<T>(string endpoint, object data = null)
        {
            HttpResponseMessage response;
            if (data != null)
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await _client.PutAsync($"{BaseUrl}/{endpoint}", content);
            }
            else
            {
                response = await _client.PutAsync($"{BaseUrl}/{endpoint}", null);
            }
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public async Task<T> DeleteAsync<T>(string endpoint)
        {
            var response = await _client.DeleteAsync($"{BaseUrl}/{endpoint}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}

