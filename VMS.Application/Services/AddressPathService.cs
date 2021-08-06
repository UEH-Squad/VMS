using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

        public AddressPathService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper, IHttpClientFactory clientFactory) : base(repository, dbContextFactory, mapper)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<Provinces>> GetProvinces()
        {
            // Instead of calling API, we can make calls to database to retrieve data

            string url = string.Format("https://provinces.open-api.vn/api/?depth=3");

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/vnd.github.v3+json");

            HttpClient client = _clientFactory.CreateClient();

            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string stringResponse = await response.Content.ReadAsStringAsync();

                return
                    (

                    System.Text.Json.JsonSerializer.Deserialize<List<Provinces>>(stringResponse,
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })

                    );
            }
            else
            {
                return Array.Empty<Provinces>();
            }
        }

        public async Task InsertToDatabase()
        {
            IEnumerable<Provinces> provinces = await GetProvinces();
            DbContext dbContext = _dbContextFactory.CreateDbContext();
            List<string> divitionTypes = new List<string>();
            foreach (var province in provinces)
            {

                if (string.IsNullOrEmpty(divitionTypes.FirstOrDefault(a => a == province.Division_type)))
                {
                    divitionTypes.Add(province.Division_type);
                }
                foreach (var district in province.Districts)
                {
                    if (string.IsNullOrEmpty(divitionTypes.FirstOrDefault(a => a == district.Division_type)))
                    {
                        divitionTypes.Add(district.Division_type);
                    }
                    foreach (var ward in district.Wards)
                    {
                        if (string.IsNullOrEmpty(divitionTypes.FirstOrDefault(a => a == ward.Division_type)))
                        {
                            divitionTypes.Add(ward.Division_type);
                        }
                    }
                }
            }
            List<AddressPathType> addressPathTypes = divitionTypes.Select(a => new AddressPathType
            {
                Type = a
            }).ToList();
            await _repository.InsertAsync<AddressPathType>(dbContext, addressPathTypes);
            //add data to AddressPath
            foreach (var province in provinces)
            {
                AddressPath provinceAddressPath = new AddressPath() { Name = province.Name, AddressPathTypeId = addressPathTypes.Find(a => a.Type == province.Division_type).Id };
                await _repository.InsertAsync(dbContext, provinceAddressPath);
                foreach (var district in province.Districts)
                {
                    AddressPath districtAddressPath = new AddressPath() { Name = district.Name, AddressPathTypeId = addressPathTypes.Find(a => a.Type == district.Division_type).Id, ParentPathId = provinceAddressPath.Id };
                    await _repository.InsertAsync(dbContext, districtAddressPath);
                    foreach (var ward in district.Wards)
                    {
                        AddressPath wardAddressPath = new AddressPath() { Name = ward.Name, AddressPathTypeId = addressPathTypes.Find(a => a.Type == ward.Division_type).Id, ParentPathId = districtAddressPath.Id };
                        await _repository.InsertAsync(dbContext, wardAddressPath);
                    }
                }
            }

        }

    }

}
