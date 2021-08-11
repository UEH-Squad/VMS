using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IAddressLocationService
    {
        Task<CoordinateResponse> GetCoordinateAsync(string address);
    }
}
