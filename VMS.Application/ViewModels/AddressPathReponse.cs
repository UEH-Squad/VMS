using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class Province
    {

        public string Name { get; set; }
        public string Division_type { get; set; }
        public List<District> Districts { get; set; }
    }
    public class District
    {
        public string Name { get; set; }
        public string Division_type { get; set; }
        public List<Ward> Wards { get; set; }
    }
    public class Ward
    {
        public string Name { get; set; }
        public string Division_type { get; set; }
    }
}
