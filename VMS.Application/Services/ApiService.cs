using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Application.Services
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public ApiService(IHttpClientFactory clientFactory,IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        public async Task<CoordinateResponse> GetCoordinateAsync(string address)
        {
            string url = string.Format(_configuration.GetValue<string>("GeocodingAPI:Url"), address, _configuration.GetValue<string>("GeocodingAPI:Key"));

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            HttpClient client = _clientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();

                dynamic deserialized = JsonConvert.DeserializeObject(stringResponse);
                try
                {
                    dynamic postion = deserialized["items"][0]["position"];
                    return new CoordinateResponse() { Latitude = postion["lat"], Longitude = postion["lng"] };
                }
                catch (Exception)
                {
                    return new CoordinateResponse();
                }
            }
            else
            {
                return new CoordinateResponse();
            }
        }
    }
}
