using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class AddressPathService : BaseService, IAddressPathService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public AddressPathService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(repository, dbContextFactory, mapper)
        {
            _clientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task InitializeAddressPathsAsync()
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();
            if (await _repository.GetCountAsync<AddressPathType>(dbContext) != 0) return;
            List<AddressPathResponse> addressPathResponses = await GetAddressPathsAsync();
            List<AddressPath> addressPaths = new();
            List<AddressPathType> addressPathTypes = new();
            foreach (var a in addressPathResponses)
            {
                AddressPathsRecursive(addressPaths, addressPathTypes, null, new Division() { Name = a.Name, DivisionType = a.DivisionType, Paths = a.Paths });
            }
            await _repository.InsertAsync<AddressPathType>(dbContext, addressPathTypes);
            await _repository.InsertAsync<AddressPath>(dbContext, addressPaths);
        }

        private void AddressPathsRecursive(List<AddressPath> addressPaths, List<AddressPathType> addressPathTypes, AddressPath parentAddressPath, Division addressPathResponse)
        {
            TextInfo textInfo = new CultureInfo("vi-VN", false).TextInfo;
            string divisionType = textInfo.ToTitleCase(addressPathResponse.DivisionType);
            AddressPathType addressPathType = addressPathTypes.FirstOrDefault(t => t.Type == divisionType);
            if (addressPathType is null)
            {
                addressPathType = new() { Type = divisionType};
                addressPathTypes.Add(addressPathType);
            }

            AddressPath addressPath = new()
            {
                Name = addressPathResponse.Name,
                AddressPathType = addressPathType,
                PreviousPath = parentAddressPath
            };
            addressPaths.Add(addressPath);

            if (addressPathResponse.Paths is null) return;

            foreach (var apr in addressPathResponse.Paths)
            {
                AddressPathsRecursive(addressPaths, addressPathTypes, addressPath, apr);
            }
        }

        private async Task<List<AddressPathResponse>> GetAddressPathsAsync()
        {
            string url = _configuration.GetValue<string>("AddressAPI:Url");

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            HttpClient client = _clientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();

                List<AddressPathResponse> addressPathResponses = JsonConvert.DeserializeObject<List<AddressPathResponse>>(stringResponse);

                return addressPathResponses;
            }

            return new List<AddressPathResponse>(); 
        }
    }

}
