using Microsoft.Extensions.Configuration;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using VMS.Application.Interfaces;

namespace VMS.Application.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public GeoLocationService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _clientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Coordinate> GetCoordinateAsync(string address)
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
                    dynamic position = deserialized["items"][0]["position"];
                    return new Coordinate() { X = position["lng"], Y = position["lat"] };
                }
                catch (Exception)
                {
                    return new Coordinate();
                }
            }

            return new Coordinate();
        }
    }
}