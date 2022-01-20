using System.ComponentModel.DataAnnotations;

namespace VMS.Domain.Models
{
    public class Setting
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
