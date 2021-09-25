namespace VMS.Domain.Models
{
    public class ReasonReport
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public int FeedbackId { get; set; }
        public int RecruitmentId { get; set; }

        public virtual Feedback Feedback { get; set; }
        public virtual Recruitment Recruitment { get; set; }
    }
}
