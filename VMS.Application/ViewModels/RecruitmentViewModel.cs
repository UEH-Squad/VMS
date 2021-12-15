using System.Collections.Generic;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class RecruitmentViewModel
    {
        public int Id { get; set; }
        public double? Rating { get; set; }
        
        public UserViewModel User {  get; set; }

        public ActivityViewModel Activity { get; set; }

        public List<RecruitmentRatingViewModel> RecruitmentRatings { get; set; }
    }
}
