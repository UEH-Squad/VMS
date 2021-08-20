using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IGeoLocationService
    {
        Task<Coordinate> GetCoordinateAsync(string address);
    }
}