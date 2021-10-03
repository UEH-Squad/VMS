using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;

namespace VMS.Pages.Volunteer.EditUserProfile
{
    public partial class Index
    {
        [Inject]
        private ISkillService SkillService { get; set; }

        private async Task<IEnumerable<SkillViewModel>> SearchSkills(string searchText)
        {
            return await SkillService.GetAllSkillsByNameAsync(searchText);
        }
    }
}
