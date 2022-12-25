using System.Collections.Generic;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class RecruitmentViewModel
    {
        public int Id { get; set; }
        public double? Rating { get; set; }
        public double? RatingByOrg { get; set; }
        public string CommentByOrg { get; set; }
        public string CommentByUser { get; set; }
        public bool IsGift { get; set; }
        public bool IsCommit { get; set; }
        public UserViewModel User { get; set; }

        public UserViewModel Organizer { get; set; }

        public ActivityViewModel Activity { get; set; }

        public List<RecruitmentRatingViewModel> RecruitmentRatings { get; set; }
    }
}
