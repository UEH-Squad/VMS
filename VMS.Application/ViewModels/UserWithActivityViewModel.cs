﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Application.ViewModels
{
    public class UserWithActivityViewModel
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public double Distance { get; set; }
        public int MemberQuantity { get; set; }
        public int AddressPathId { get; set; }
    }
}
