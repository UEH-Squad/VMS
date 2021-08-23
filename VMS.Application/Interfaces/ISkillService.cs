using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface ISkillService
    {
        Task<List<SkillViewModel>> GetAllSkillsAsync();
        Task<List<SkillViewModel>> GetAllSubSkillsAsync(int parentSkillId);
    }
}
