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
        private readonly string _apiKey = "Sra1zcTjUuhm1suxPB0mXKF-vyajcClci_jHqiT9ycU";

        public ApiService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<CoordinateResponse> GetCoordinateAsync(string address)
        {
            string url = string.Format("https://geocode.search.hereapi.com/v1/geocode?q={0}&apiKey={1}", address, _apiKey);

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
