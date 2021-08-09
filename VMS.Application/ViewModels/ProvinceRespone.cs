using System.Collections.Generic;

namespace VMS.Application.ViewModels
{
    public class ProvinceRespone
    {
        public string Name { get; set; }
        public string DivisionType { get; set; }
        public List<ProvinceRespone> ProvinceRespones { get; set; }
    }
}
