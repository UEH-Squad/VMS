using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class OrgActViewModel
    {
        public string Title { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string ActivityBanner { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public DateTime EndDate { get; set; }
        public bool Isclosed { get; set; }
    }
}
