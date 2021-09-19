using NetTopologySuite.Geometries;
using System.Threading.Tasks;

namespace VMS.Application.Interfaces
{
    public interface IGeoLocationService
    {
        Task<Coordinate> GetCoordinateAsync(string address);
    }
}