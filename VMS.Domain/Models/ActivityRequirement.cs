namespace VMS.Domain.Models
{
    public class ActivityRequirement : DeleteEntity<int>
    {
        public int ActivityId { get; set; }
        public int RequirementId { get; set; }
        public virtual Activity Activity { get; set; }
        public virtual Requirement Requirement { get; set; }
    }
}