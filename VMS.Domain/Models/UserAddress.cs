namespace VMS.Domain.Models
{
    public class UserAddress : DeleteEntity<int>
    {
        public string UserId { get; set; }
        public int AddressPathId { get; set; }

        public virtual User User { get; set; }
        public virtual AddressPath AddressPath { get; set; }
    }
}