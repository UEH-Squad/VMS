namespace VMS.Domain.Models
{
    public class ImageReport
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public int FeedbackId { get; set; }

        public virtual Feedback Feedback { get; set; }
    }
}
