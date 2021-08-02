using System;

namespace VMS.Application.ViewModels
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDay { get; set; }
        public int MemberQuantity { get; set; }
        public string Description { get; set; }
        public string Mission { get; set; }
        public string Banner { get; set; }

    }
}
