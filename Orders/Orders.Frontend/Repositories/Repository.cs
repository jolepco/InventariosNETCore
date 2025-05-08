using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Orders.Frontend.AuthenticationProviders;
using Orders.Frontend.Helpers;
using Orders.Shared.Entities;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Orders.Frontend.Repositories
{
    public class Repository:IRepository
    {
        private readonly IHttpClientFactory  _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IJSRuntime _jSRuntime;
        //private readonly HttpClient cliente;
        private JsonSerializerOptions _jsonDefaultOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        public Repository(IHttpClientFactory  httpClient)
        {

            _httpClient = httpClient;
        }
        public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url)
        {
           
            var cliente = _httpClient.CreateClient("BackEnd");
            
            var responseHttp = await cliente.GetAsync(url);
            
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswerAsync<T>(responseHttp);
                return new HttpResponseWrapper<T>(response, false, responseHttp);
            }

            return new HttpResponseWrapper<T>(default, true, responseHttp);
        }

        public async Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model)
        {
            var cliente = _httpClient.CreateClient("BackEnd");
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await cliente.PostAsync(url, messageContet);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model)
        {

            var cliente = _httpClient.CreateClient("BackEnd");
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await cliente.PostAsync(url, messageContet);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswerAsync<TActionResponse>(responseHttp);
                return new HttpResponseWrapper<TActionResponse>(response, false, responseHttp);
            }

            return new HttpResponseWrapper<TActionResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model)
        {
            var cliente = _httpClient.CreateClient("BackEnd");
            var messageJson = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");
            var responseHttp = await cliente.PutAsync(url, messageContent);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TActionResponse>> PutAsync<T, TActionResponse>(string url, T model)
        {
            var cliente = _httpClient.CreateClient("BackEnd");
            var messageJson = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");
            var responseHttp = await cliente.PutAsync(url, messageContent);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswerAsync<TActionResponse>(responseHttp);
                return new HttpResponseWrapper<TActionResponse>(response, false, responseHttp);
            }
            return new HttpResponseWrapper<TActionResponse>(default, true, responseHttp);
        }


        private async Task<T> UnserializeAnswerAsync<T>(HttpResponseMessage responseHttp)
        {
            var response = await responseHttp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(response, _jsonDefaultOptions)!;
        }

        public async Task<HttpResponseWrapper<object>> DeleteAsync<T>(string url)
        {
            var cliente = _httpClient.CreateClient("BackEnd");
            var responseHttp = await cliente.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<object>> GetAsync(string url)
        {
            var cliente = _httpClient.CreateClient("BackEnd");
            var responseHTTP = await cliente.GetAsync(url);
            return new HttpResponseWrapper<object>(null, !responseHTTP.IsSuccessStatusCode, responseHTTP);
        }

    }
}
