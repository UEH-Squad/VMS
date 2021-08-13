using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
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

        private readonly string pattern;
        private readonly Dictionary<int, List<string>> divisionDepth = new()
        {
            { 1, new List<string> { "Thành Phố Trung Ương", "Tỉnh" } },
            { 2, new List<string> { "Thành Phố", "Quận", "Huyện", "Thị Xã" } },
            { 3, new List<string> { "Phường", "Xã", "Thị Trấn" } }
        };

        public AddressPathService(IRepository repository,
                                  IDbContextFactory<VmsDbContext> dbContextFactory,
                                  IMapper mapper,
                                  IHttpClientFactory httpClientFactory,
                                  IConfiguration configuration) : base(repository, dbContextFactory, mapper)
        {
            clientFactory = httpClientFactory;
            this.configuration = configuration;
            pattern = divisionDepth.Aggregate(string.Empty, (acc, next) =>
            {
                acc += string.Join('|', next.Value);
                return acc + '|';
            }).TrimEnd('|');
        }

        public async Task InitializeAddressPathsAsync()
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();
            if (await _repository.ExistsAsync<AddressPath>(dbContext))
            {
                return;
            }

            List<AddressPathResponse> addressPathResponses = await GetAddressPathsAsync();
            List<AddressPath> addressPaths = new();
            foreach (var a in addressPathResponses)
            {
                AddressPathsRecursive(addressPaths, null,
                    new Division() { Name = a.Name, DivisionType = a.DivisionType, Paths = a.Paths });
            }

            await _repository.InsertAsync<AddressPath>(dbContext, addressPaths);
        }

        private void AddressPathsRecursive(ICollection<AddressPath> addressPaths,
                                           AddressPath parentAddressPath,
                                           Division addressPathResponse)
        {
            string addressPathName = Regex.Replace(addressPathResponse.Name, $@"^({pattern})\s\W*", "", RegexOptions.IgnoreCase);
            if (Regex.IsMatch(addressPathName, @"^\d"))
            {
                addressPathName = $"{addressPathResponse.DivisionType.ToTitleCase()} {addressPathName}";
            }

            AddressPath addressPath = new()
            {
                Name = addressPathName,
                Depth = divisionDepth.FirstOrDefault(x => x.Value.Contains(addressPathResponse.DivisionType.ToTitleCase())).Key,
                PreviousPath = parentAddressPath
            };

            addressPaths.Add(addressPath);

            if (addressPathResponse.Paths is not null)
            {
                foreach (var path in addressPathResponse.Paths)
                {
                    AddressPathsRecursive(addressPaths, addressPath, path);
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