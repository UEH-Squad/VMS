using Newtonsoft.Json;

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