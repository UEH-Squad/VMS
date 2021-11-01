namespace VMS.Application.ViewModels
{
    public class RecruitmentRatingViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public double Rank { get; set; }
        public string Comment {  get; set; }
        public bool IsOrgRating { get; set; }
    }
}
