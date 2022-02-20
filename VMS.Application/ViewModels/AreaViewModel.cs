using System.ComponentModel.DataAnnotations;

namespace VMS.Application.ViewModels
{
    public class AreaViewModel
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPinned { get; set; }

        [Required(ErrorMessage = "Icon không được để trống")]
        public string Icon { get; set; }
        [Required(ErrorMessage = "Tên lĩnh vực không được để trống")]
        public string Name { get; set; }
    }
}