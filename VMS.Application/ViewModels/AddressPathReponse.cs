using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class Provinces
    {

        public string Name { get; set; }
        public string Division_type { get; set; }
        public List<Districts> Districts { get; set; }
    }
    public class Districts
    {
        public string Name { get; set; }
        public string Division_type { get; set; }
        public List<Wards> Wards { get; set; }
    }
    public class Wards
    {
        public string Name { get; set; }
        public string Division_type { get; set; }
    }
}
