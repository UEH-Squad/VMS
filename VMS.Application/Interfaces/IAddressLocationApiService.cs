using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IAddressLocationApiService
    {
        Task<AddressLocationReponse> GetAddressLocationAsync(string address);
    }
}
