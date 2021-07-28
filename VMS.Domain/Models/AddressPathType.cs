using System.Collections.Generic;

namespace VMS.Domain.Models
{
    public class AddressPathType : EntityBase<int>
    {
        public string Type { get; set; }

        public virtual ICollection<AddressPath> AddressPaths { get; set; }
    }
}