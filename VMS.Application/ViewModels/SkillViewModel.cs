using System.Collections.Generic;

namespace VMS.Application.ViewModels
{
    public class SkillViewModel
    {
        public int Id { get; set; }
        public int? ParentSkillId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public List<SkillViewModel> SubSkills { get; set; }

        public override bool Equals(object obj) => obj != null && obj is SkillViewModel obj2 && Name == obj2.Name;

        public override int GetHashCode() => Id;
    }
}
