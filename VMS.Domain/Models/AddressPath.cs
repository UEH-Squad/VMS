using System.Collections.Generic;

namespace VMS.Domain.Models
{
    public class AddressPath : EntityBase<int>
    {
        public string Name { get; set; }
        public int AddressPathTypeId { get; set; }
        public int NextPathId { get; set; }
        public int PreviousPathId { get; set; }

        public virtual AddressPath NextPath { get; set; }
        public virtual AddressPath PreviousPath { get; set; }
        public virtual AddressPathType AddressPathType { get; set; }
        public virtual ICollection<UserAddress> UserAddresses { get; set; }
        public virtual ICollection<ActivityAddress> ActivityAddresses { get; set; }
        public ICollection<AddressPath> ParentNextPaths { get; set; }
        public ICollection<AddressPath> ParentPreviousPaths { get; set; }
    }
}