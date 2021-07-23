using System.Collections.Generic;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class CreateProjectViewModel
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public int CategoryId { get; set; }
    }
}