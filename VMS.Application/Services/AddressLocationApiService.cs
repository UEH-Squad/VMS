using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Application.Services
{
    public class AddressLocationApiService : IAddressLocationApiService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _apiKey = "Sra1zcTjUuhm1suxPB0mXKF-vyajcClci_jHqiT9ycU";

        public AddressLocationApiService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<AddressLocationReponse> GetAddressLocationAsync(string address)
        {
            string url = string.Format("https://geocode.search.hereapi.com/v1/geocode?q={0}&apiKey={1}", address, _apiKey);

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            HttpClient client = _clientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();

                dynamic deserialized = JsonConvert.DeserializeObject(stringResponse);
                dynamic postion = deserialized["items"][0]["position"];

                return new AddressLocationReponse() { Latitude = postion["lat"], Longitude = postion["lng"] };
            }
            else
            {
                return new AddressLocationReponse();
            }
        }
    }
}
