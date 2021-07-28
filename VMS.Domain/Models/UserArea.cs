namespace VMS.Domain.Models
{
    public class UserArea : DeleteEntity<int>
    {
        public string UserId { get; set; }
        public int AreaId { get; set; }

        public virtual User User { get; set; }
        public virtual Area Area { get; set; }
    }
}