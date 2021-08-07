using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IApiService
    {
        Task<CoordinateReponse> GetCoordinateAsync(string address);
    }
}
