using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VMS.Application.ViewModels
{
    public class SkillViewModel
    {
        [Required(ErrorMessage = "Tên kỹ năng không được để trống")]
        public string Name { get; set; }

        public int Id { get; set; }
        public int? ParentSkillId { get; set; }
        public string Icon { get; set; }
        public List<SkillViewModel> SubSkills { get; set; }

        public bool IsDeleted { get; set; }

        public override bool Equals(object obj) => obj != null && obj is SkillViewModel obj2 && Name == obj2.Name;

        public override int GetHashCode() => Id;
    }
}
