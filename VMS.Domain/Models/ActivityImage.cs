namespace VMS.Domain.Models
{
    public class ActivityImage : EntityBase<int>
    {
        public int ActivityId { get; set; }
        public string Image { get; set; }

        public virtual Activity Activity { get; set; }
    }
}