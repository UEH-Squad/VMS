using System.Collections.Generic;

namespace VMS.Domain.Models
{
    public class AddressPath : EntityBase<int>
    {
        public string Name { get; set; }
        public int Depth { get; set; }
        public int? ParentPathId { get; set; }

        public virtual AddressPath PreviousPath { get; set; }
        public virtual ICollection<UserAddress> UserAddresses { get; set; }
        public virtual ICollection<ActivityAddress> ActivityAddresses { get; set; }
        public ICollection<AddressPath> SubPaths { get; set; }
    }
}