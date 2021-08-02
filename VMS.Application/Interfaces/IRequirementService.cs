using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Domain.Models;

namespace VMS.Application.Interfaces
{
    public interface IRequirementService
    {
        Task<List<Requirement>> GetAllRequirements();
    }
}
