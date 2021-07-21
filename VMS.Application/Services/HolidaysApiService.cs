using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Application.Services
{
    public class HolidaysApiService : IHolidaysApiService
    {
        private readonly IHttpClientFactory _clientFactory;

        public HolidaysApiService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<HolidayResponseModel>> GetHolidays(HolidayRequestModel holidaysRequest)
        {
            // Instead of calling API, we can make calls to database to retrieve data

            string url = string.Format("https://date.nager.at/api/v2/PublicHolidays/{0}/{1}", holidaysRequest.Year, holidaysRequest.CountryCode);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/vnd.github.v3+json");

            HttpClient client = _clientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<List<HolidayResponseModel>>(stringResponse,
                    new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
            }
            else
            {
                return Array.Empty<HolidayResponseModel>();
            }
        }
    }
}