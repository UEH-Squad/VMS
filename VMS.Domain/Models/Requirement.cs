using System.Collections.Generic;

namespace VMS.Domain.Models
{
    public class Requirement : DeleteEntity<int>
    {
        public string Name { get; set; }

        public ICollection<ActivityRequirement> ActivityRequirements { get; set; }
    }
}