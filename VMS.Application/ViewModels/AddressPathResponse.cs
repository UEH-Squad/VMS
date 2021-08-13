using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public partial class AddressPathResponse
    {
        public string Name { get; set; }
        [JsonProperty("division_type")]
        public string DivisionType { get; set; }
        [JsonProperty("districts")]
        public Division[] Paths { get; set; }
    }

    public partial class Division
    {
        public string Name { get; set; }
        [JsonProperty("division_type")]
        public string DivisionType { get; set; }
        [JsonProperty("wards")]
        public Division[] Paths { get; set; }
    }
}
