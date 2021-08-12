using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VMS.Domain.Models;

namespace VMS.Common.Comparer
{
    public class AddressPathTypeComparer : IEqualityComparer<AddressPathType>
    {
        public bool Equals(AddressPathType x, AddressPathType y)
        {
            return x.Type.Equals(y.Type, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode([DisallowNull] AddressPathType obj)
        {
            return obj.Type.GetHashCode();
        }
    }
}