using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Application.ViewModels
{
    public class FilterVltViewModel
    {
        public string SearchValue { get; set; }
        public bool IsSearch { get; set; }
        public string FacultyName { get; set; }
        public string Course { get; set; }

        public List<AreaViewModel> Areas { get; set; }
        public List<SkillViewModel> Skills { get; set; }

        public FilterVltViewModel()
        {
            Areas = new();
            Skills = new();
        }
    }
}
