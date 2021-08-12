using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common.Comparer;
using VMS.Common.Extensions;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class AddressPathService : BaseService, IAddressPathService
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly IConfiguration configuration;

        public AddressPathService(IRepository repository,
                                  IDbContextFactory<VmsDbContext> dbContextFactory,
                                  IMapper mapper,
                                  IHttpClientFactory httpClientFactory,
                                  IConfiguration configuration) : base(repository, dbContextFactory, mapper)
        {
            clientFactory = httpClientFactory;
            this.configuration = configuration;
        }

        public async Task InitializeAddressPathsAsync()
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();
            if (await _repository.ExistsAsync<AddressPathType>(dbContext))
            {
                return;
            }

            List<AddressPathResponse> addressPathResponses = await GetAddressPathsAsync();
            List<AddressPath> addressPaths = new();
            HashSet<AddressPathType> addressPathTypes = new(new AddressPathTypeComparer());
            foreach (var a in addressPathResponses)
            {
                AddressPathsRecursive(addressPaths, addressPathTypes, null,
                    new Division() { Name = a.Name, DivisionType = a.DivisionType, Paths = a.Paths });
            }

            await _repository.InsertAsync<AddressPathType>(dbContext, addressPathTypes);
            await _repository.InsertAsync<AddressPath>(dbContext, addressPaths);
        }

        private void AddressPathsRecursive(ICollection<AddressPath> addressPaths,
                                           ISet<AddressPathType> addressPathTypes,
                                           AddressPath parentAddressPath,
                                           Division addressPathResponse)
        {
            string divisionType = addressPathResponse.DivisionType.ToTitleCase();
            addressPathTypes.Add(new AddressPathType() { Type = divisionType });

            AddressPath addressPath = new()
            {
                Name = addressPathResponse.Name,
                AddressPathType = addressPathTypes.FirstOrDefault(x => x.Type == divisionType),
                PreviousPath = parentAddressPath
            };

            addressPaths.Add(addressPath);

            if (addressPathResponse.Paths is not null)
            {
                foreach (var path in addressPathResponse.Paths)
                {
                    AddressPathsRecursive(addressPaths, addressPathTypes, addressPath, path);
                }
            }
        }

        private async Task<List<AddressPathResponse>> GetAddressPathsAsync()
        {
            string url = configuration.GetValue<string>("AddressAPI:Url");
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            HttpClient client = clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AddressPathResponse>>(stringResponse);
            }

            return new List<AddressPathResponse>();
        }
    }
}