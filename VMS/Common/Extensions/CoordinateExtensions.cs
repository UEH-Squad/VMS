using NetTopologySuite.Geometries;
using VMS.Application.ViewModels;

namespace VMS.Common.Extensions
{
    public static class CoordinateExtensions
    {
        public static Coordinate ToCoordinate(this CoordinateJs @this)
        {
            if (@this == null)
            {
                return null;
            }

            return new Coordinate
            {
                X = @this.Longitude,
                Y = @this.Latitude
            };
        }
    }
}