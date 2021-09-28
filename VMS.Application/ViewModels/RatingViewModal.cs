using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel.DataAnnotations;

namespace VMS.Application.ViewModels
{
    public class Rating
    {
        public string KeyWord { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string CommentDefault { get; set; } = "Chưa nhận xét";
        public string MemberComment { get; set; } = "Tuyệt vời tuyệt vời quá mệt quá quá mệt Tuyệt vời tuyệt vời quá mệt quá quá mệtTuyệt vời tuyệt vời quá mệt quá quá mệtTuyệt vời tuyệt vời quá mệt quá quá mệt";
        public string OrganizationComment { get; set; } = "Send";
        public bool Select1 { get; set; }
        public bool Select2 { get; set; }
        public bool Select3 { get; set; }
        public bool Select4 { get; set; }
        public bool Select5 { get; set; }
        public bool Select6 { get; set; }
        public bool Select7 { get; set; }
    }

}
