using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Domain.Models;

namespace VMS.Application.Interfaces
{
    public interface IAddressService
    {
        Task<List<AddressPath>> GetAllProvincesAsync();
        Task<List<AddressPath>> GetAllDistrictsByParentIdAsync(int parentId);
    }
}
