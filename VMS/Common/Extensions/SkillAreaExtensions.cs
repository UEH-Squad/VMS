using System.Collections.Generic;
using System.Linq;
using VMS.Application.ViewModels;

namespace VMS.Common.Extensions
{
    public static class SkillAreaExtensions
    {
        public static string GetListNames(this List<AreaViewModel> areas)
        {
            return areas.Select(x => x.Name).Aggregate((a, b) => a + "; " + b);
        }

        public static string GetListNames(this List<SkillViewModel> skills)
        {
            return skills.Select(x => x.Name).Aggregate((a, b) => a + "; " + b);
        }
    }
}
