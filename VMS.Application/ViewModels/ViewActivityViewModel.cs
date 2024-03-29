﻿using System;
using System.Collections.Generic;
using VMS.Domain.Models;

namespace VMS.Application.ViewModels
{
    public class ViewActivityViewModel
    {
        public int Id { get; set; }
        public string OrgId { get; set; }
        public string Name { get; set; }
        public int AreaId { get; set; }
        public string CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public string Address { get; set; }
        public int MemberQuantity { get; set; }
        public string Description { get; set; }
        public string Mission { get; set; }
        public string Commission { get; set; }
        public string Requirement { get; set; }
        public bool IsVirtual { get; set; }
        public string Website { get; set; }
        public string Banner { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDenied { get; set; }
        public bool IsClosed { get; set; }
        public string Targets { get; set; }
        public bool IsPoint { get; set; }
        public bool IsDay { get; set; }
        public int NumberOfDays { get; set; }
        public bool IsSingleChoice { get; set; }


        public Area Area { get; set; }
        public User Organizer { get; set; }
        public IList<Skill> Skills { get; set; } = new List<Skill>();
        public IList<AddressPath> AddressPaths { get; set; } = new List<AddressPath>();
    }
}