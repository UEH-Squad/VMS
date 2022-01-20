using Microsoft.EntityFrameworkCore;

namespace VMS.Domain.Models
{
    [Keyless]
    public class Setting
    {
        public string VideoHomepageURL { get; set; }
    }
}
